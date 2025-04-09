namespace Pulse.Infrastructure.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using NodaTime;
    using Pulse.Core.Contracts;
    using Pulse.Core.Models.Entities;
    using Pulse.Core.Models.Requests;
    using Pulse.Core.Models.Responses;
    using Pulse.Core.Models;

    public class SpecialService : ISpecialService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SpecialService> _logger;

        public SpecialService(
            ApplicationDbContext context,
            ILogger<SpecialService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<SpecialListResponse> GetSpecialsAsync(SpecialQueryRequest request)
        {
            IQueryable<Special> query = _context.Specials
                .Include(s => s.Venue)
                .Include(s => s.Tags)
                    .ThenInclude(ts => ts.Tag)
                .AsQueryable();

            if (request.VenueId.HasValue)
            {
                query = query.Where(s => s.VenueId == request.VenueId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();
                query = query.Where(s =>
                    s.Content.ToLower().Contains(searchTerm) ||
                    s.Venue!.Name.ToLower().Contains(searchTerm));
            }

            if (request.SpecialTypeId.HasValue)
            {
                query = query.Where(s => (int)s.Type == request.SpecialTypeId.Value);
            }

            if (request.TagId.HasValue)
            {
                query = query.Where(s => s.Tags.Any(ts => ts.TagId == request.TagId.Value));
            }

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            var now = SystemClock.Instance.GetCurrentInstant();
            var nowDate = now.InUtc().Date;
            var nowTime = now.InUtc().TimeOfDay;

            var specials = await query
                .OrderByDescending(s => IsSpecialActive(s, nowDate, nowTime))
                .ThenBy(s => s.StartDate)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(s => new SpecialSummary
                {
                    Id = s.Id,
                    VenueId = s.VenueId,
                    VenueName = s.Venue!.Name,
                    Content = s.Content,
                    TypeName = s.Type.ToString(),
                    StartDate = s.StartDate.ToString(),
                    StartTime = s.StartTime.ToString(),
                    EndTime = s.EndTime.ToString(),
                    IsActive = IsSpecialActive(s, nowDate, nowTime),
                    TimeRemaining = CalculateTimeRemaining(s, now, nowDate, nowTime),
                    TagNames = s.Tags.Select(t => "#" + t.Tag.Name).ToList(),
                    IsRecurring = s.IsRecurring
                })
                .ToListAsync();

            return new SpecialListResponse
            {
                Specials = specials,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = totalPages
            };
        }

        public async Task<SpecialDetailResponse?> GetSpecialByIdAsync(int id)
        {
            var special = await _context.Specials
                .Include(s => s.Venue)
                .Include(s => s.Tags)
                    .ThenInclude(ts => ts.Tag)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (special == null)
            {
                return null;
            }

            var now = SystemClock.Instance.GetCurrentInstant();
            var nowDate = now.InUtc().Date;
            var nowTime = now.InUtc().TimeOfDay;

            return new SpecialDetailResponse
            {
                Id = special.Id,
                VenueId = special.VenueId,
                VenueName = special.Venue!.Name,
                Content = special.Content,
                TypeName = special.Type.ToString(),
                TypeId = (int)special.Type,
                StartDate = special.StartDate.ToString(),
                StartTime = special.StartTime.ToString(),
                EndTime = special.EndTime?.ToString(),
                ExpirationDate = special.ExpirationDate?.ToString(),
                IsActive = IsSpecialActive(special, nowDate, nowTime),
                TimeRemaining = CalculateTimeRemaining(special, now, nowDate, nowTime),
                IsRecurring = special.IsRecurring,
                RecurringSchedule = special.RecurringSchedule,
                Tags = special.Tags.Select(ts => new TagDetail
                {
                    Id = ts.TagId,
                    Name = ts.Tag.Name
                }).ToList()
            };
        }

        public async Task<NewSpecialResponse> CreateSpecialAsync(NewSpecialRequest request, string userId)
        {
            // Process tags - get existing ones and create new ones
            var tagEntities = new List<Tag>();
            foreach (var tagItem in request.Tags)
            {
                if (tagItem.Id.HasValue)
                {
                    // Existing tag
                    var tag = await _context.Tags.FindAsync(tagItem.Id.Value);
                    if (tag != null)
                    {
                        tagEntities.Add(tag);
                        tag.UsageCount++; // Increment usage count
                    }
                }
                else if (!string.IsNullOrWhiteSpace(tagItem.Name))
                {
                    // New tag - check if it already exists by name
                    var normalizedName = tagItem.Name.Trim().ToLower();
                    if (normalizedName.StartsWith("#"))
                        normalizedName = normalizedName.Substring(1);

                    var existingTag = await _context.Tags
                        .FirstOrDefaultAsync(t => t.Name.ToLower() == normalizedName);

                    if (existingTag != null)
                    {
                        tagEntities.Add(existingTag);
                        existingTag.UsageCount++; // Increment usage count
                    }
                    else
                    {
                        // Create new tag
                        var newTag = new Tag
                        {
                            Name = normalizedName,
                            UsageCount = 1,
                            CreatedAt = SystemClock.Instance.GetCurrentInstant()
                        };

                        _context.Tags.Add(newTag);
                        await _context.SaveChangesAsync(); // Save to get the ID
                        tagEntities.Add(newTag);
                    }
                }
            }

            // Create special
            var special = new Special
            {
                VenueId = request.VenueId,
                Content = request.Content,
                Type = request.Type,
                StartDate = request.StartDate,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                ExpirationDate = request.ExpirationDate,
                IsRecurring = request.IsRecurring,
                RecurringSchedule = request.RecurringSchedule
            };

            _context.Specials.Add(special);
            await _context.SaveChangesAsync(); // Save to get the ID

            // Create tag-to-special links
            foreach (var tag in tagEntities)
            {
                var tagToSpecialLink = new TagToSpecialLink
                {
                    SpecialId = special.Id,
                    TagId = tag.Id
                };

                _context.TagToSpecialLinks.Add(tagToSpecialLink);
            }

            await _context.SaveChangesAsync();

            return new NewSpecialResponse
            {
                Id = special.Id,
                VenueId = special.VenueId,
                Content = special.Content,
                TypeName = special.Type.ToString(),
                StartDate = special.StartDate.ToString(),
                StartTime = special.StartTime.ToString(),
                TagNames = tagEntities.Select(t => "#" + t.Name).ToList()
            };
        }

        public async Task<UpdateSpecialResponse?> UpdateSpecialAsync(int id, UpdateSpecialRequest request, string userId)
        {
            var special = await _context.Specials
                .Include(s => s.Tags)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (special == null)
            {
                return null;
            }

            // Update special properties
            special.Content = request.Content;
            special.Type = request.Type;
            special.StartDate = request.StartDate;
            special.StartTime = request.StartTime;
            special.EndTime = request.EndTime;
            special.ExpirationDate = request.ExpirationDate;
            special.IsRecurring = request.IsRecurring;
            special.RecurringSchedule = request.RecurringSchedule;

            // Remove existing tag links
            _context.TagToSpecialLinks.RemoveRange(special.Tags);

            // Process tags - get existing ones and create new ones
            var tagEntities = new List<Tag>();
            foreach (var tagItem in request.Tags)
            {
                if (tagItem.Id.HasValue)
                {
                    // Existing tag
                    var tag = await _context.Tags.FindAsync(tagItem.Id.Value);
                    if (tag != null)
                    {
                        tagEntities.Add(tag);
                        tag.UsageCount++; // Increment usage count
                    }
                }
                else if (!string.IsNullOrWhiteSpace(tagItem.Name))
                {
                    // New tag - check if it already exists by name
                    var normalizedName = tagItem.Name.Trim().ToLower();
                    if (normalizedName.StartsWith("#"))
                        normalizedName = normalizedName.Substring(1);

                    var existingTag = await _context.Tags
                        .FirstOrDefaultAsync(t => t.Name.ToLower() == normalizedName);

                    if (existingTag != null)
                    {
                        tagEntities.Add(existingTag);
                        existingTag.UsageCount++; // Increment usage count
                    }
                    else
                    {
                        // Create new tag
                        var newTag = new Tag
                        {
                            Name = normalizedName,
                            UsageCount = 1,
                            CreatedAt = SystemClock.Instance.GetCurrentInstant()
                        };

                        _context.Tags.Add(newTag);
                        await _context.SaveChangesAsync(); // Save to get the ID
                        tagEntities.Add(newTag);
                    }
                }
            }

            // Create new tag links
            foreach (var tag in tagEntities)
            {
                var tagToSpecialLink = new TagToSpecialLink
                {
                    SpecialId = special.Id,
                    TagId = tag.Id
                };

                _context.TagToSpecialLinks.Add(tagToSpecialLink);
            }

            await _context.SaveChangesAsync();

            return new UpdateSpecialResponse
            {
                Id = special.Id,
                VenueId = special.VenueId,
                Content = special.Content,
                TypeName = special.Type.ToString(),
                StartDate = special.StartDate.ToString(),
                StartTime = special.StartTime.ToString(),
                TagNames = tagEntities.Select(t => "#" + t.Name).ToList(),
                LastUpdated = DateTime.UtcNow
            };
        }

        public async Task<bool> DeleteSpecialAsync(int id)
        {
            var special = await _context.Specials
                .Include(s => s.Tags)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (special == null)
            {
                return false;
            }

            // Remove tag links
            _context.TagToSpecialLinks.RemoveRange(special.Tags);

            // Remove the special
            _context.Specials.Remove(special);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<SpecialTypeListResponse> GetSpecialTypesAsync()
        {
            // For MVP, we'll just return the enum values as a list
            var specialTypes = Enum.GetValues(typeof(Pulse.Core.Enums.SpecialTypes))
                .Cast<Pulse.Core.Enums.SpecialTypes>()
                .Select(st => new SpecialTypeItem
                {
                    Id = (int)st,
                    Name = st.ToString()
                })
                .OrderBy(st => st.Name)
                .ToList();

            return new SpecialTypeListResponse
            {
                SpecialTypes = specialTypes
            };
        }

        public async Task<TagListResponse> GetTagsAsync(string? searchTerm)
        {
            IQueryable<Tag> query = _context.Tags.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim().ToLower();
                if (searchTerm.StartsWith("#"))
                    searchTerm = searchTerm.Substring(1);

                query = query.Where(t => t.Name.ToLower().Contains(searchTerm));
            }

            var tags = await query
                .OrderByDescending(t => t.UsageCount)
                .Take(100) // Limit to the top 100 most used tags
                .Select(t => new TagListItem
                {
                    Id = t.Id,
                    Name = t.Name,
                    UsageCount = t.UsageCount
                })
                .ToListAsync();

            return new TagListResponse
            {
                Tags = tags,
                TotalCount = await query.CountAsync()
            };
        }

        private static bool IsSpecialActive(Special special, LocalDate nowDate, LocalTime nowTime)
        {
            // Check if the special has started
            if (special.StartDate > nowDate)
                return false;

            // Check if the special has expired
            if (special.ExpirationDate.HasValue && special.ExpirationDate.Value < nowDate)
                return false;

            // For same-day specials, check time constraints
            if (special.StartDate == nowDate)
            {
                if (special.StartTime > nowTime)
                    return false;
            }

            // If end time is set and we're past it on the last day, it's not active
            if (special.EndTime.HasValue &&
                (!special.ExpirationDate.HasValue || special.ExpirationDate.Value == nowDate) &&
                special.EndTime.Value < nowTime)
                return false;

            return true;
        }

        private static string? CalculateTimeRemaining(Special special, Instant now, LocalDate nowDate, LocalTime nowTime)
        {
            if (!IsSpecialActive(special, nowDate, nowTime))
                return null;

            // Calculate end time based on special's configuration
            if (special.ExpirationDate.HasValue)
            {
                if (special.ExpirationDate.Value == nowDate && special.EndTime.HasValue)
                {
                    // Last day and has end time
                    var endDateTime = new LocalDateTime(
                        special.ExpirationDate.Value.Year,
                        special.ExpirationDate.Value.Month,
                        special.ExpirationDate.Value.Day,
                        special.EndTime.Value.Hour,
                        special.EndTime.Value.Minute,
                        special.EndTime.Value.Second
                    );

                    var endInstant = endDateTime.InUtc().ToInstant();
                    var duration = endInstant - now;

                    if (duration.TotalHours > 24)
                    {
                        return $"{Math.Floor(duration.TotalDays)} days left";
                    }

                    if (duration.TotalHours >= 1)
                    {
                        return $"{Math.Floor(duration.TotalHours)} hours left";
                    }

                    return $"{Math.Floor(duration.TotalMinutes)} minutes left";
                }
                else
                {
                    // Has future expiration date
                    var daysRemaining = (special.ExpirationDate.Value - nowDate).Days;
                    return daysRemaining > 1 ? $"{daysRemaining} days left" : "Ends today";
                }
            }
            else if (special.EndTime.HasValue && special.StartDate == nowDate)
            {
                // Same-day special with end time
                var endDateTime = new LocalDateTime(
                    nowDate.Year,
                    nowDate.Month,
                    nowDate.Day,
                    special.EndTime.Value.Hour,
                    special.EndTime.Value.Minute,
                    special.EndTime.Value.Second
                );

                var endInstant = endDateTime.InUtc().ToInstant();
                var duration = endInstant - now;

                if (duration.TotalHours >= 1)
                {
                    return $"{Math.Floor(duration.TotalHours)} hours left";
                }

                return $"{Math.Floor(duration.TotalMinutes)} minutes left";
            }

            return "Ongoing";
        }
    }
}
