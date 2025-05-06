namespace MirthSystems.Pulse.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Cronos;
    using Microsoft.Extensions.Logging;
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models.Responses;
    using MirthSystems.Pulse.Core.Models;
    using NodaTime;
    using NetTopologySuite.Geometries;
    using Azure.Maps.Search.Models;

    public class SpecialService : ISpecialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAzureMapsApiService _mapsApiService;
        private readonly ILogger<SpecialService> _logger;

        public SpecialService(
            IUnitOfWork unitOfWork,
            IAzureMapsApiService mapsApiService,
            ILogger<SpecialService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapsApiService = mapsApiService;
            _logger = logger;
        }

        public async Task<PagedApiResponse<SpecialListItem>> GetSpecialsAsync(GetSpecialsRequest request)
        {
            try
            {
                // Geocode the user's address to get the search location
                Point? searchLocation = null;
                double? radiusInMeters = null;

                if (!string.IsNullOrEmpty(request.Address))
                {
                    var geocodeResponse = await _mapsApiService.SearchClient.GetGeocodingAsync(request.Address);

                    if (geocodeResponse.Value.Features.Count > 0)
                    {
                        var result = geocodeResponse.Value.Features[0];
                        searchLocation = new Point(result.Geometry.Coordinates.Longitude, result.Geometry.Coordinates.Latitude);

                        // Convert radius from miles to meters (1 mile = 1609.34 meters)
                        radiusInMeters = request.Radius * 1609.34;
                    }
                    else
                    {
                        return PagedApiResponse<SpecialListItem>.CreateError($"Could not geocode address: {request.Address}");
                    }
                }

                // Parse search date time if provided, otherwise use current time
                Instant searchDateTimeInstant;
                if (!string.IsNullOrEmpty(request.SearchDateTime))
                {
                    if (DateTimeOffset.TryParse(request.SearchDateTime, out var parsedDateTime))
                    {
                        searchDateTimeInstant = Instant.FromDateTimeOffset(parsedDateTime);
                    }
                    else
                    {
                        return PagedApiResponse<SpecialListItem>.CreateError($"Invalid search date time format: {request.SearchDateTime}");
                    }
                }
                else
                {
                    searchDateTimeInstant = SystemClock.Instance.GetCurrentInstant();
                }

                // Get specials with filtering
                var (specials, totalCount) = await _unitOfWork.Specials.GetPagedSpecialsAsync(
                    request.Page,
                    request.PageSize,
                    searchLocation,
                    radiusInMeters,
                    null,  // search term not provided in GetSpecialsRequest
                    request.SpecialTypeId.HasValue ? (Core.Enums.SpecialTypes)request.SpecialTypeId.Value : null,
                    !request.IsCurrentlyRunning.HasValue || !request.IsCurrentlyRunning.Value);

                // Map and check if each special is currently running
                var specialListItems = await Task.WhenAll(specials.Select(async s =>
                {
                    var isCurrentlyRunning = await _unitOfWork.Specials.IsSpecialCurrentlyActiveAsync(s.Id, searchDateTimeInstant);
                    return MapToSpecialListItem(s, isCurrentlyRunning);
                }));

                // If IsCurrentlyRunning filter is applied, filter the results
                if (request.IsCurrentlyRunning.HasValue && request.IsCurrentlyRunning.Value)
                {
                    specialListItems = specialListItems.Where(s => s.IsCurrentlyRunning).ToArray();
                }

                // Filter by venue ID if provided
                if (request.VenueId.HasValue)
                {
                    specialListItems = specialListItems.Where(s => s.VenueId == request.VenueId.Value).ToArray();
                }

                return PagedApiResponse<SpecialListItem>.CreateSuccess(
                    specialListItems.ToList(),
                    request.Page,
                    request.PageSize,
                    totalCount,
                    "Specials retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving specials");
                return PagedApiResponse<SpecialListItem>.CreateError($"Failed to retrieve specials: {ex.Message}");
            }
        }

        public async Task<SpecialDetail?> GetSpecialByIdAsync(long id)
        {
            try
            {
                var special = await _unitOfWork.Specials.GetSpecialWithVenueAsync(id);
                if (special == null)
                {
                    return null;
                }

                var isCurrentlyRunning = await _unitOfWork.Specials.IsSpecialCurrentlyActiveAsync(
                    special.Id, SystemClock.Instance.GetCurrentInstant());

                return MapToSpecialDetail(special, isCurrentlyRunning);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving special with ID {SpecialId}", id);
                return null;
            }
        }

        public async Task<SpecialDetail> CreateSpecialAsync(CreateSpecialRequest request, string userId)
        {
            try
            {
                var venue = await _unitOfWork.Venues.GetByIdAsync(request.VenueId);
                if (venue == null)
                {
                    throw new KeyNotFoundException($"Venue with ID {request.VenueId} not found");
                }

                var startDate = LocalDate.FromDateOnly(DateOnly.Parse(request.StartDate));
                var startTime = LocalTime.FromTimeOnly(TimeOnly.Parse(request.StartTime));
                LocalTime? endTime = null;
                if (!string.IsNullOrEmpty(request.EndTime))
                {
                    endTime = LocalTime.FromTimeOnly(TimeOnly.Parse(request.EndTime));
                }

                LocalDate? expirationDate = null;
                if (!string.IsNullOrEmpty(request.ExpirationDate))
                {
                    expirationDate = LocalDate.FromDateOnly(DateOnly.Parse(request.ExpirationDate));
                }

                var special = new Special
                {
                    VenueId = request.VenueId,
                    Content = request.Content,
                    Type = request.Type,
                    StartDate = startDate,
                    StartTime = startTime,
                    EndTime = endTime,
                    ExpirationDate = expirationDate,
                    IsRecurring = request.IsRecurring,
                    CronSchedule = request.CronSchedule,
                    CreatedAt = SystemClock.Instance.GetCurrentInstant(),
                    CreatedByUserId = userId
                };

                await _unitOfWork.Specials.AddAsync(special);
                await _unitOfWork.SaveChangesAsync();

                var createdSpecial = await _unitOfWork.Specials.GetSpecialWithVenueAsync(special.Id);
                var isCurrentlyRunning = await _unitOfWork.Specials.IsSpecialCurrentlyActiveAsync(
                    special.Id, SystemClock.Instance.GetCurrentInstant());

                return MapToSpecialDetail(createdSpecial!, isCurrentlyRunning);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating special");
                throw;
            }
        }

        public async Task<SpecialDetail> UpdateSpecialAsync(long id, UpdateSpecialRequest request, string userId)
        {
            var special = await _unitOfWork.Specials.GetSpecialWithVenueAsync(id);
            if (special == null)
            {
                throw new KeyNotFoundException($"Special with ID {id} not found");
            }

            try
            {
                var startDate = LocalDate.FromDateOnly(DateOnly.Parse(request.StartDate));
                var startTime = LocalTime.FromTimeOnly(TimeOnly.Parse(request.StartTime));
                LocalTime? endTime = null;
                if (!string.IsNullOrEmpty(request.EndTime))
                {
                    endTime = LocalTime.FromTimeOnly(TimeOnly.Parse(request.EndTime));
                }

                LocalDate? expirationDate = null;
                if (!string.IsNullOrEmpty(request.ExpirationDate))
                {
                    expirationDate = LocalDate.FromDateOnly(DateOnly.Parse(request.ExpirationDate));
                }

                special.Content = request.Content;
                special.Type = request.Type;
                special.StartDate = startDate;
                special.StartTime = startTime;
                special.EndTime = endTime;
                special.ExpirationDate = expirationDate;
                special.IsRecurring = request.IsRecurring;
                special.CronSchedule = request.CronSchedule;
                special.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
                special.UpdatedByUserId = userId;

                _unitOfWork.Specials.Update(special);
                await _unitOfWork.SaveChangesAsync();

                var updatedSpecial = await _unitOfWork.Specials.GetSpecialWithVenueAsync(id);
                var isCurrentlyRunning = await _unitOfWork.Specials.IsSpecialCurrentlyActiveAsync(
                    id, SystemClock.Instance.GetCurrentInstant());

                return MapToSpecialDetail(updatedSpecial!, isCurrentlyRunning);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating special with ID {SpecialId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteSpecialAsync(long id, string userId)
        {
            var special = await _unitOfWork.Specials.GetByIdAsync(id);
            if (special == null)
            {
                return false;
            }

            try
            {
                special.IsDeleted = true;
                special.DeletedAt = SystemClock.Instance.GetCurrentInstant();
                special.DeletedByUserId = userId;

                _unitOfWork.Specials.Update(special);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting special with ID {SpecialId}", id);
                return false;
            }
        }

        public async Task<bool> IsSpecialCurrentlyRunningAsync(long specialId, DateTimeOffset? referenceTime = null)
        {
            try
            {
                Instant instant;

                if (referenceTime.HasValue)
                {
                    instant = Instant.FromDateTimeOffset(referenceTime.Value);
                }
                else
                {
                    instant = SystemClock.Instance.GetCurrentInstant();
                }

                return await _unitOfWork.Specials.IsSpecialCurrentlyActiveAsync(specialId, instant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if special with ID {SpecialId} is currently running", specialId);
                return false;
            }
        }

        #region Mapping Methods

        private SpecialListItem MapToSpecialListItem(Special special, bool isCurrentlyRunning)
        {
            return new SpecialListItem
            {
                Id = special.Id,
                VenueId = special.VenueId,
                VenueName = special.Venue?.Name ?? string.Empty,
                Content = special.Content,
                Type = special.Type,
                TypeName = special.Type.ToString(),
                StartDate = special.StartDate.ToString("yyyy-MM-dd", null),
                StartTime = special.StartTime.ToString("HH:mm", null),
                EndTime = special.EndTime?.ToString("HH:mm", null),
                IsCurrentlyRunning = isCurrentlyRunning,
                IsRecurring = special.IsRecurring
            };
        }

        private SpecialDetail MapToSpecialDetail(Special special, bool isCurrentlyRunning)
        {
            return new SpecialDetail
            {
                Id = special.Id,
                VenueId = special.VenueId,
                VenueName = special.Venue?.Name ?? string.Empty,
                Content = special.Content,
                Type = special.Type,
                TypeName = special.Type.ToString(),
                StartDate = special.StartDate.ToString("yyyy-MM-dd", null),
                StartTime = special.StartTime.ToString("HH:mm", null),
                EndTime = special.EndTime?.ToString("HH:mm", null),
                ExpirationDate = special.ExpirationDate?.ToString("yyyy-MM-dd", null),
                IsRecurring = special.IsRecurring,
                CronSchedule = special.CronSchedule,
                IsCurrentlyRunning = isCurrentlyRunning,
                CreatedAt = special.CreatedAt.ToDateTimeOffset(),
                UpdatedAt = special.UpdatedAt?.ToDateTimeOffset()
            };
        }

        #endregion
    }
}
