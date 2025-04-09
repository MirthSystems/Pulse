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

    public class VenueService : IVenueService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILocationService _locationService;
        private readonly ILogger<VenueService> _logger;

        public VenueService(
            ApplicationDbContext context,
            ILocationService locationService,
            ILogger<VenueService> logger)
        {
            _context = context;
            _locationService = locationService;
            _logger = logger;
        }

        public async Task<VenueListResponse> GetVenuesAsync(VenueQueryRequest request, string? userId)
        {
            IQueryable<Venue> query = _context.Venues
                .Include(v => v.VenueType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();
                query = query.Where(v =>
                    v.Name.ToLower().Contains(searchTerm) ||
                    v.Locality.ToLower().Contains(searchTerm) ||
                    v.Region.ToLower().Contains(searchTerm));
            }

            if (request.VenueTypeId.HasValue)
            {
                query = query.Where(v => v.VenueTypeId == request.VenueTypeId.Value);
            }

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            var venues = await query
                .OrderBy(v => v.Name)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(v => new VenueSummary
                {
                    Id = v.Id,
                    Name = v.Name,
                    VenueTypeName = v.VenueType.Name,
                    FormattedAddress = FormatAddress(v),
                    ImageLink = v.ImageLink,
                    ActiveSpecialsCount = _context.Specials.Count(s =>
                        s.VenueId == v.Id &&
                        (s.ExpirationDate == null || s.ExpirationDate >= LocalDate.FromDateTime(DateTime.UtcNow))),
                    ActivePostsCount = _context.Posts.Count(p =>
                        p.VenueId == v.Id &&
                        !p.IsExpired)
                })
                .ToListAsync();

            // If userId is provided, check if the user can manage each venue
            if (!string.IsNullOrEmpty(userId))
            {
                var externalId = userId;
                var user = await _context.Users.FirstOrDefaultAsync(u => u.ExternalId == externalId);

                if (user != null)
                {
                    // Get all venues the user can manage
                    var managedVenueIds = await _context.VenueUsers
                        .Where(vu => vu.UserId == user.Id)
                        .Select(vu => vu.VenueId)
                        .ToListAsync();

                    // Update UserCanManage property for each venue
                    foreach (var venue in venues)
                    {
                        venue.UserCanManage = managedVenueIds.Contains(venue.Id);
                    }
                }
            }

            return new VenueListResponse
            {
                Venues = venues,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = totalPages
            };
        }

        public async Task<VenueDetailResponse?> GetVenueByIdAsync(int id, string? userId)
        {
            var venue = await _context.Venues
                .Include(v => v.VenueType)
                .Include(v => v.BusinessHours)
                .Include(v => v.Specials.Where(s =>
                    (s.ExpirationDate == null || s.ExpirationDate >= LocalDate.FromDateTime(DateTime.UtcNow)) &&
                    (s.StartDate <= LocalDate.FromDateTime(DateTime.UtcNow))))
                    .ThenInclude(s => s.Tags)
                        .ThenInclude(ts => ts.Tag)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venue == null)
            {
                return null;
            }

            bool userCanManage = false;
            bool userCanManageSpecials = false;

            if (!string.IsNullOrEmpty(userId))
            {
                var externalId = userId;
                var user = await _context.Users.FirstOrDefaultAsync(u => u.ExternalId == externalId);

                if (user != null)
                {
                    // Check if user has any permissions for this venue
                    var venueUser = await _context.VenueUsers
                        .Include(vu => vu.Permissions)
                            .ThenInclude(vup => vup.Permission)
                        .FirstOrDefaultAsync(vu => vu.UserId == user.Id && vu.VenueId == id);

                    if (venueUser != null)
                    {
                        // Check specific permissions
                        userCanManage = venueUser.IsVerifiedOwner ||
                                        venueUser.Permissions.Any(p => p.Permission.Name == "manage:venue");

                        userCanManageSpecials = venueUser.IsVerifiedOwner ||
                                                venueUser.Permissions.Any(p => p.Permission.Name == "manage:specials");
                    }
                }
            }

            var activeSpecials = venue.Specials
                .Where(s => IsSpecialActive(s))
                .Select(s => new SpecialSummary
                {
                    Id = s.Id,
                    VenueId = s.VenueId,
                    VenueName = venue.Name,
                    Content = s.Content,
                    TypeName = s.Type.ToString(),
                    StartDate = s.StartDate.ToString(),
                    StartTime = s.StartTime.ToString(),
                    EndTime = s.EndTime?.ToString(),
                    IsActive = IsSpecialActive(s),
                    TimeRemaining = CalculateTimeRemaining(s),
                    TagNames = s.Tags.Select(t => "#" + t.Tag.Name).ToList(),
                    IsRecurring = s.IsRecurring
                })
                .ToList();

            var response = new VenueDetailResponse
            {
                Id = venue.Id,
                VenueTypeId = venue.VenueTypeId,
                VenueTypeName = venue.VenueType.Name,
                Name = venue.Name,
                Description = venue.Description,
                PhoneNumber = venue.PhoneNumber,
                Website = venue.Website,
                Email = venue.Email,
                ImageLink = venue.ImageLink,
                AddressLine1 = venue.AddressLine1,
                AddressLine2 = venue.AddressLine2,
                AddressLine3 = venue.AddressLine3,
                AddressLine4 = venue.AddressLine4,
                Locality = venue.Locality,
                Region = venue.Region,
                Postcode = venue.Postcode,
                Country = venue.Country,
                FormattedAddress = FormatAddress(venue),
                Latitude = venue.Location?.Y ?? 0,
                Longitude = venue.Location?.X ?? 0,
                OperatingHours = venue.BusinessHours
                    .OrderBy(oh => oh.DayOfWeek)
                    .Select(oh => new OperatingHoursResponse
                    {
                        Id = oh.Id,
                        DayOfWeek = oh.DayOfWeek,
                        TimeOfOpen = oh.TimeOfOpen,
                        TimeOfClose = oh.TimeOfClose,
                        IsClosed = oh.IsClosed,
                        FormattedHours = FormatOperatingHours(oh)
                    })
                    .ToList(),
                ActiveSpecials = activeSpecials,
                UserCanManage = userCanManage,
                UserCanManageSpecials = userCanManageSpecials
            };

            return response;
        }

        public async Task<NewVenueResponse> CreateVenueAsync(NewVenueRequest request, string userId)
        {
            // Simplified for MVP - we'll use a placeholder point (0,0)
            var point = await _locationService.GetPointFromAddressAsync("placeholder");

            var venue = new Venue
            {
                VenueTypeId = request.VenueTypeId,
                Name = request.Name,
                Description = request.Description,
                PhoneNumber = request.PhoneNumber,
                Website = request.Website,
                Email = request.Email,
                ImageLink = request.ImageLink,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                AddressLine3 = request.AddressLine3,
                AddressLine4 = request.AddressLine4,
                Locality = request.Locality,
                Region = request.Region,
                Postcode = request.Postcode,
                Country = request.Country,
                Location = point,
                BusinessHours = request.OperatingHours.Select(oh => new OperatingSchedule
                {
                    DayOfWeek = oh.DayOfWeek,
                    TimeOfOpen = oh.TimeOfOpen,
                    TimeOfClose = oh.TimeOfClose,
                    IsClosed = oh.IsClosed
                }).ToList()
            };

            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();

            // Associate the user with the venue as a verified owner
            var externalId = userId;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ExternalId == externalId);

            if (user != null)
            {
                var venueUser = new VenueUser
                {
                    UserId = user.Id,
                    VenueId = venue.Id,
                    IsVerifiedOwner = true,
                    CreatedAt = SystemClock.Instance.GetCurrentInstant(),
                    CreatedByUserId = user.Id
                };

                _context.VenueUsers.Add(venueUser);
                await _context.SaveChangesAsync();
            }

            var venueType = await _context.VenueTypes.FindAsync(venue.VenueTypeId);

            return new NewVenueResponse
            {
                Id = venue.Id,
                Name = venue.Name,
                VenueTypeId = venue.VenueTypeId,
                VenueTypeName = venueType?.Name ?? "Unknown",
                FormattedAddress = FormatAddress(venue),
                UserCanManage = true
            };
        }

        public async Task<UpdateVenueResponse?> UpdateVenueAsync(int id, UpdateVenueRequest request, string userId)
        {
            var venue = await _context.Venues
                .Include(v => v.BusinessHours)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venue == null)
            {
                return null;
            }

            // Update venue properties
            venue.VenueTypeId = request.VenueTypeId;
            venue.Name = request.Name;
            venue.Description = request.Description;
            venue.PhoneNumber = request.PhoneNumber;
            venue.Website = request.Website;
            venue.Email = request.Email;
            venue.ImageLink = request.ImageLink;
            venue.AddressLine1 = request.AddressLine1;
            venue.AddressLine2 = request.AddressLine2;
            venue.AddressLine3 = request.AddressLine3;
            venue.AddressLine4 = request.AddressLine4;
            venue.Locality = request.Locality;
            venue.Region = request.Region;
            venue.Postcode = request.Postcode;
            venue.Country = request.Country;

            // Update operating hours
            // First, remove existing hours
            _context.BusinessHours.RemoveRange(venue.BusinessHours);

            // Then, add the new ones
            venue.BusinessHours = request.OperatingHours.Select(oh => new OperatingSchedule
            {
                VenueId = venue.Id,
                DayOfWeek = oh.DayOfWeek,
                TimeOfOpen = oh.TimeOfOpen,
                TimeOfClose = oh.TimeOfClose,
                IsClosed = oh.IsClosed
            }).ToList();

            await _context.SaveChangesAsync();

            var venueType = await _context.VenueTypes.FindAsync(venue.VenueTypeId);

            return new UpdateVenueResponse
            {
                Id = venue.Id,
                Name = venue.Name,
                VenueTypeId = venue.VenueTypeId,
                VenueTypeName = venueType?.Name ?? "Unknown",
                FormattedAddress = FormatAddress(venue),
                LastUpdated = DateTime.UtcNow
            };
        }

        public async Task<VenueTypeListResponse> GetVenueTypesAsync()
        {
            var venueTypes = await _context.VenueTypes
                .OrderBy(vt => vt.Name)
                .Select(vt => new VenueTypeItem
                {
                    Id = vt.Id,
                    Name = vt.Name,
                    Description = vt.Description
                })
                .ToListAsync();

            return new VenueTypeListResponse
            {
                VenueTypes = venueTypes
            };
        }

        public async Task<VenueListResponse> GetManagedVenuesAsync(string userId)
        {
            var externalId = userId;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ExternalId == externalId);

            if (user == null)
            {
                return new VenueListResponse
                {
                    Venues = new List<VenueSummary>(),
                    TotalCount = 0,
                    Page = 1,
                    PageSize = 10,
                    TotalPages = 0
                };
            }

            var managedVenueIds = await _context.VenueUsers
                .Where(vu => vu.UserId == user.Id)
                .Select(vu => vu.VenueId)
                .ToListAsync();

            var venues = await _context.Venues
                .Include(v => v.VenueType)
                .Where(v => managedVenueIds.Contains(v.Id))
                .OrderBy(v => v.Name)
                .Select(v => new VenueSummary
                {
                    Id = v.Id,
                    Name = v.Name,
                    VenueTypeName = v.VenueType.Name,
                    FormattedAddress = FormatAddress(v),
                    ImageLink = v.ImageLink,
                    ActiveSpecialsCount = _context.Specials.Count(s =>
                        s.VenueId == v.Id &&
                        (s.ExpirationDate == null || s.ExpirationDate >= LocalDate.FromDateTime(DateTime.UtcNow))),
                    ActivePostsCount = _context.Posts.Count(p =>
                        p.VenueId == v.Id &&
                        !p.IsExpired),
                    UserCanManage = true
                })
                .ToListAsync();

            return new VenueListResponse
            {
                Venues = venues,
                TotalCount = venues.Count,
                Page = 1,
                PageSize = venues.Count,
                TotalPages = 1
            };
        }

        public async Task<bool> UserCanManageVenueAsync(int venueId, string userId)
        {
            var externalId = userId;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ExternalId == externalId);

            if (user == null)
            {
                return false;
            }

            // For MVP we'll just check if the user is a manager of this venue
            var venueUser = await _context.VenueUsers
                .Include(vu => vu.Permissions)
                    .ThenInclude(vup => vup.Permission)
                .FirstOrDefaultAsync(vu => vu.UserId == user.Id && vu.VenueId == venueId);

            return venueUser != null && (
                venueUser.IsVerifiedOwner ||
                venueUser.Permissions.Any(p => p.Permission.Name == "manage:venue")
            );
        }

        public async Task<bool> UserCanManageVenueSpecialsAsync(int venueId, string userId)
        {
            var externalId = userId;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ExternalId == externalId);

            if (user == null)
            {
                return false;
            }

            // For MVP we'll just check if the user is a manager of this venue
            var venueUser = await _context.VenueUsers
                .Include(vu => vu.Permissions)
                    .ThenInclude(vup => vup.Permission)
                .FirstOrDefaultAsync(vu => vu.UserId == user.Id && vu.VenueId == venueId);

            return venueUser != null && (
                venueUser.IsVerifiedOwner ||
                venueUser.Permissions.Any(p => p.Permission.Name == "manage:specials" || p.Permission.Name == "manage:venue")
            );
        }

        private static string FormatAddress(Venue venue)
        {
            var addressParts = new List<string>();

            addressParts.Add(venue.AddressLine1);

            if (!string.IsNullOrWhiteSpace(venue.AddressLine2))
                addressParts.Add(venue.AddressLine2);

            addressParts.Add($"{venue.Locality}, {venue.Region} {venue.Postcode}");
            addressParts.Add(venue.Country);

            return string.Join(", ", addressParts.Where(p => !string.IsNullOrWhiteSpace(p)));
        }

        private static string FormatOperatingHours(OperatingSchedule schedule)
        {
            if (schedule.IsClosed)
            {
                return "Closed";
            }

            return $"{FormatTime(schedule.TimeOfOpen)} - {FormatTime(schedule.TimeOfClose)}";
        }

        private static string FormatTime(LocalTime time)
        {
            var hour = time.Hour;
            var minute = time.Minute;
            var amPm = hour < 12 ? "AM" : "PM";

            // Convert to 12-hour format
            if (hour == 0)
                hour = 12;
            else if (hour > 12)
                hour -= 12;

            return minute == 0
                ? $"{hour} {amPm}"
                : $"{hour}:{minute:D2} {amPm}";
        }

        private static bool IsSpecialActive(Special special)
        {
            var now = SystemClock.Instance.GetCurrentInstant().InUtc().Date;
            var timeNow = SystemClock.Instance.GetCurrentInstant().InUtc().TimeOfDay;

            // Check if the special has started
            if (special.StartDate > now)
                return false;

            // Check if the special has expired
            if (special.ExpirationDate.HasValue && special.ExpirationDate.Value < now)
                return false;

            // For same-day specials, check time constraints
            if (special.StartDate == now)
            {
                if (special.StartTime > timeNow)
                    return false;
            }

            // If end time is set and we're past it on the last day, it's not active
            if (special.EndTime.HasValue &&
                (!special.ExpirationDate.HasValue || special.ExpirationDate.Value == now) &&
                special.EndTime.Value < timeNow)
                return false;

            return true;
        }

        private static string? CalculateTimeRemaining(Special special)
        {
            if (!IsSpecialActive(special))
                return null;

            var now = SystemClock.Instance.GetCurrentInstant();
            var nowDate = now.InUtc().Date;
            var nowTime = now.InUtc().TimeOfDay;

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
