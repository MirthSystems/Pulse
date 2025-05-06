namespace MirthSystems.Pulse.Infrastructure.Services
{
    using Microsoft.Extensions.Logging;
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models.Responses;
    using MirthSystems.Pulse.Core.Models;
    using NodaTime;
    using NetTopologySuite.Geometries;
    using Azure.Maps.Search.Models;

    public class VenueService : IVenueService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAzureMapsApiService _mapsApiService;
        private readonly ILogger<VenueService> _logger;

        public VenueService(
            IUnitOfWork unitOfWork,
            IAzureMapsApiService mapsApiService,
            ILogger<VenueService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapsApiService = mapsApiService;
            _logger = logger;
        }

        public async Task<PagedApiResponse<VenueListItem>> GetVenuesAsync(int page = 1, int pageSize = 20)
        {
            try
            {
                var (venues, totalCount) = await _unitOfWork.Venues.GetPagedVenuesAsync(page, pageSize);

                var venueListItems = venues.Select(MapToVenueListItem).ToList();

                return PagedApiResponse<VenueListItem>.CreateSuccess(
                    venueListItems,
                    page,
                    pageSize,
                    totalCount,
                    "Venues retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving venues");
                return PagedApiResponse<VenueListItem>.CreateError($"Failed to retrieve venues: {ex.Message}");
            }
        }

        public async Task<VenueDetail?> GetVenueByIdAsync(long id)
        {
            try
            {
                var venue = await _unitOfWork.Venues.GetVenueWithDetailsAsync(id);
                if (venue == null)
                {
                    return null;
                }

                return MapToVenueDetail(venue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving venue with ID {VenueId}", id);
                return null;
            }
        }

        public async Task<BusinessHours?> GetVenueBusinessHoursAsync(long id)
        {
            try
            {
                var venue = await _unitOfWork.Venues.GetByIdAsync(id);
                if (venue == null)
                {
                    return null;
                }

                var schedules = await _unitOfWork.OperatingSchedules.GetSchedulesByVenueIdAsync(id);
                if (schedules == null || !schedules.Any())
                {
                    return null;
                }

                return new BusinessHours
                {
                    VenueId = venue.Id,
                    VenueName = venue.Name,
                    ScheduleItems = schedules.Select(MapToOperatingScheduleListItem).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving business hours for venue with ID {VenueId}", id);
                return null;
            }
        }

        public async Task<VenueSpecials?> GetVenueSpecialsAsync(long id, bool includeCurrentStatus = true)
        {
            try
            {
                var venue = await _unitOfWork.Venues.GetByIdAsync(id);
                if (venue == null)
                {
                    return null;
                }

                var specials = await _unitOfWork.Specials.GetSpecialsByVenueIdAsync(id);
                var specialListItems = new List<SpecialListItem>();

                if (includeCurrentStatus)
                {
                    var now = SystemClock.Instance.GetCurrentInstant();
                    specialListItems = await Task.WhenAll(specials.Select(async s =>
                    {
                        var isCurrentlyRunning = await _unitOfWork.Specials.IsSpecialCurrentlyActiveAsync(s.Id, now);
                        return MapToSpecialListItem(s, isCurrentlyRunning);
                    })).ContinueWith(t => t.Result.ToList());
                }
                else
                {
                    specialListItems = specials.Select(s => MapToSpecialListItem(s, false)).ToList();
                }

                return new VenueSpecials
                {
                    VenueId = venue.Id,
                    VenueName = venue.Name,
                    Specials = specialListItems
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving specials for venue with ID {VenueId}", id);
                return null;
            }
        }

        public async Task<VenueDetail> CreateVenueAsync(CreateVenueRequest request, string userId)
        {
            try
            {
                // Create a new Address entity and geocode it
                var geocodedAddressResponse = await _mapsApiService.SearchClient.GetGeocodingAsync(string.Join(",",
                    [
                        request.Address.StreetAddress,
                        request.Address.Locality,
                        request.Address.Region,
                        request.Address.Postcode,
                        request.Address.Country
                    ]));
                var address = new Core.Entities.Address
                {
                    StreetAddress = request.Address.StreetAddress,
                    SecondaryAddress = request.Address.SecondaryAddress,
                    Locality = request.Address.Locality,
                    Region = request.Address.Region,
                    Postcode = request.Address.Postcode,
                    Country = request.Address.Country,
                    Location = new Point(
                        geocodedAddressResponse.Value.Features[0].Geometry.Coordinates.Longitude,
                        geocodedAddressResponse.Value.Features[0].Geometry.Coordinates.Latitude
                        )
                };

                // Geocode the address using Azure Maps
                var fullAddress = $"{address.StreetAddress}, {address.Locality}, {address.Region} {address.Postcode}, {address.Country}";
                var geocodeResponse = await _mapsApiService.SearchClient.GetGeocodingAsync(fullAddress);

                if (geocodeResponse.Value.Features.Count > 0)
                {
                    var result = geocodeResponse.Value.Features[0];
                    address.Location = new Point(result.Geometry.Coordinates.Longitude, result.Geometry.Coordinates.Latitude);
                }

                // Create a new Venue entity
                var venue = new Venue
                {
                    Name = request.Name,
                    Description = request.Description,
                    PhoneNumber = request.PhoneNumber,
                    Website = request.Website,
                    Email = request.Email,
                    ProfileImage = request.ProfileImage,
                    CreatedAt = SystemClock.Instance.GetCurrentInstant(),
                    CreatedByUserId = userId,
                    Address = address
                };

                // Add the venue to the database
                await _unitOfWork.Venues.AddAsync(venue);

                // Create operating schedules
                foreach (var scheduleRequest in request.BusinessHours)
                {
                    var schedule = new OperatingSchedule
                    {
                        VenueId = venue.Id,
                        DayOfWeek = (DayOfWeek)scheduleRequest.DayOfWeek,
                        TimeOfOpen = LocalTime.FromTimeOnly(TimeOnly.Parse(scheduleRequest.TimeOfOpen)),
                        TimeOfClose = LocalTime.FromTimeOnly(TimeOnly.Parse(scheduleRequest.TimeOfClose)),
                        IsClosed = scheduleRequest.IsClosed,
                    };
                    await _unitOfWork.OperatingSchedules.AddAsync(schedule);
                }

                await _unitOfWork.SaveChangesAsync();

                // Fetch the complete venue with details for return
                var createdVenue = await _unitOfWork.Venues.GetVenueWithDetailsAsync(venue.Id);
                return MapToVenueDetail(createdVenue!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating venue");
                throw;
            }
        }

        public async Task<VenueDetail> UpdateVenueAsync(long id, UpdateVenueRequest request, string userId)
        {
            var venue = await _unitOfWork.Venues.GetVenueWithDetailsAsync(id);
            if (venue == null)
            {
                throw new KeyNotFoundException($"Venue with ID {id} not found");
            }

            try
            {
                venue.Name = request.Name;
                venue.Description = request.Description;
                venue.PhoneNumber = request.PhoneNumber;
                venue.Website = request.Website;
                venue.Email = request.Email;
                venue.ProfileImage = request.ProfileImage;
                venue.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
                venue.UpdatedByUserId = userId;
                venue.Address.StreetAddress = request.Address.StreetAddress;
                venue.Address.SecondaryAddress = request.Address.SecondaryAddress;
                venue.Address.Locality = request.Address.Locality;
                venue.Address.Region = request.Address.Region;
                venue.Address.Postcode = request.Address.Postcode;
                venue.Address.Country = request.Address.Country;

                var fullAddress = $"{venue.Address.StreetAddress}, {venue.Address.Locality}, {venue.Address.Region} {venue.Address.Postcode}, {venue.Address.Country}";
                var geocodeResponse = await _mapsApiService.SearchClient.GetGeocodingAsync(fullAddress);

                if (geocodeResponse.Value.Features.Count > 0)
                {
                    var result = geocodeResponse.Value.Features[0];
                    venue.Address.Location = new Point(result.Geometry.Coordinates.Longitude, result.Geometry.Coordinates.Latitude);
                }

                _unitOfWork.Venues.Update(venue);
                await _unitOfWork.SaveChangesAsync();

                var updatedVenue = await _unitOfWork.Venues.GetVenueWithDetailsAsync(id);
                return MapToVenueDetail(updatedVenue!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating venue with ID {VenueId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteVenueAsync(long id, string userId)
        {
            var venue = await _unitOfWork.Venues.GetByIdAsync(id);
            if (venue == null)
            {
                return false;
            }

            try
            {
                venue.IsDeleted = true;
                venue.DeletedAt = SystemClock.Instance.GetCurrentInstant();
                venue.DeletedByUserId = userId;

                _unitOfWork.Venues.Update(venue);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting venue with ID {VenueId}", id);
                return false;
            }
        }

        #region Mapping Methods

        private VenueListItem MapToVenueListItem(Venue venue)
        {
            return new VenueListItem
            {
                Id = venue.Id,
                Name = venue.Name,
                Description = venue.Description,
                Locality = venue.Address.Locality,
                Region = venue.Address.Region,
                ProfileImage = venue.ProfileImage,
                Latitude = venue.Address.Location?.Y,
                Longitude = venue.Address.Location?.X
            };
        }

        private VenueDetail MapToVenueDetail(Venue venue)
        {
            return new VenueDetail
            {
                Id = venue.Id,
                Name = venue.Name,
                Description = venue.Description,
                PhoneNumber = venue.PhoneNumber,
                Website = venue.Website,
                Email = venue.Email,
                ProfileImage = venue.ProfileImage,
                StreetAddress = venue.Address.StreetAddress,
                SecondaryAddress = venue.Address.SecondaryAddress,
                Locality = venue.Address.Locality,
                Region = venue.Address.Region,
                Postcode = venue.Address.Postcode,
                Country = venue.Address.Country,
                Latitude = venue.Address.Location?.Y,
                Longitude = venue.Address.Location?.X,
                BusinessHours = venue.BusinessHours.Select(MapToOperatingScheduleListItem).ToList(),
                CreatedAt = venue.CreatedAt.ToDateTimeOffset(),
                UpdatedAt = venue.UpdatedAt?.ToDateTimeOffset()
            };
        }

        private OperatingScheduleListItem MapToOperatingScheduleListItem(OperatingSchedule schedule)
        {
            return new OperatingScheduleListItem
            {
                Id = schedule.Id,
                DayOfWeek = schedule.DayOfWeek,
                DayName = schedule.DayOfWeek.ToString(),
                OpenTime = schedule.TimeOfOpen.ToString("HH:mm", null),
                CloseTime = schedule.TimeOfClose.ToString("HH:mm", null),
                IsClosed = schedule.IsClosed
            };
        }

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

        #endregion
    }
}
