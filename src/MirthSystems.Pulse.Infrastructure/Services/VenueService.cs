namespace MirthSystems.Pulse.Infrastructure.Services
{
    using Microsoft.Extensions.Logging;
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models;
    using NodaTime;
    using NetTopologySuite.Geometries;
    using Azure.Maps.Search.Models;
    using MirthSystems.Pulse.Core.Extensions;

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

        public async Task<PagedResult<VenueItem>> GetVenuesAsync(GetVenuesRequest request)
        {
            try
            {
                Point? searchLocation = null;
                double? distanceInMeters = null;

                // Convert address to coordinates if provided
                if (!string.IsNullOrEmpty(request.Address))
                {
                    try
                    {
                        var geocodeResponse = await _mapsApiService.SearchClient.GetGeocodingAsync(request.Address);
                        if (geocodeResponse.Value.Features.Count > 0)
                        {
                            var result = geocodeResponse.Value.Features[0];
                            searchLocation = new Point(result.Geometry.Coordinates.Longitude, result.Geometry.Coordinates.Latitude) { SRID = 4326 };
                            
                            // Convert radius from miles to meters if provided
                            if (request.RadiusInMiles.HasValue)
                            {
                                distanceInMeters = request.RadiusInMiles.Value * 1609.34;
                            }
                            else
                            {
                                // Default to 5 miles if address is provided but no radius
                                distanceInMeters = 5 * 1609.34;
                            }
                        }
                        else
                        {
                            _logger.LogWarning("Could not geocode address: {Address}", request.Address);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error geocoding address: {Address}", request.Address);
                    }
                }

                // Query the repository with all filters
                var venues = await _unitOfWork.Venues.GetPagedVenuesAsync(
                    request.Page,
                    request.PageSize,
                    searchLocation,
                    distanceInMeters,
                    request.SearchText,
                    request.OpenOnDayOfWeek,
                    request.TimeOfDay,
                    request.HasActiveSpecials,
                    request.SpecialTypeId);

                // Map to response model
                return new PagedResult<VenueItem>
                {
                    Items = venues.Select(v => v.MapToVenueListItem()).ToList(),
                    PagingInfo = PagingInfo.Create(
                        venues.CurrentPage,
                        venues.PageSize,
                        venues.TotalCount)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving venues with filters");
                throw;
            }
        }

        public async Task<VenueItemExtended?> GetVenueByIdAsync(string id)
        {
            try
            {
                if (!long.TryParse(id, out long venueId))
                {
                    _logger.LogWarning("Invalid venue ID format: {Id}", id);
                    return null;
                }
                var venue = await _unitOfWork.Venues.GetVenueWithDetailsAsync(venueId);
                if (venue == null)
                {
                    return null;
                }
                return venue.MapToVenueDetail();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving venue with ID {VenueId}", id);
                return null;
            }
        }

        public async Task<List<OperatingScheduleItem>?> GetVenueBusinessHoursAsync(string id)
        {
            try
            {
                if (!long.TryParse(id, out long venueId))
                {
                    _logger.LogWarning("Invalid venue ID format: {Id}", id);
                    return null;
                }
                var venue = await _unitOfWork.Venues.GetByIdAsync(venueId);
                if (venue == null)
                {
                    return null;
                }
                var schedules = await _unitOfWork.OperatingSchedules.GetSchedulesByVenueIdAsync(venueId);
                if (schedules == null || !schedules.Any())
                {
                    return null;
                }
                return schedules.Select(s => s.MapToOperatingScheduleListItem()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving business hours for venue with ID {VenueId}", id);
                return null;
            }
        }

        public async Task<List<SpecialItem>?> GetVenueSpecialsAsync(string id, bool includeCurrentStatus = true)
        {
            try
            {
                if (!long.TryParse(id, out long venueId))
                {
                    _logger.LogWarning("Invalid venue ID format: {Id}", id);
                    return null;
                }
                
                var venue = await _unitOfWork.Venues.GetByIdAsync(venueId);
                if (venue == null)
                {
                    return null;
                }
                
                var specials = await _unitOfWork.Specials.GetSpecialsByVenueIdAsync(venueId);
                if (specials == null || !specials.Any())
                {
                    return null;
                }
                
                var specialListItems = new List<SpecialItem>();
                
                if (includeCurrentStatus)
                {
                    var now = SystemClock.Instance.GetCurrentInstant();
                    specialListItems = await Task.WhenAll(specials.Select(async s =>
                    {
                        var isCurrentlyRunning = await _unitOfWork.Specials.IsSpecialCurrentlyActiveAsync(s.Id, now);
                        return s.MapToSpecialListItem(isCurrentlyRunning);
                    })).ContinueWith(t => t.Result.ToList());
                }
                else
                {
                    specialListItems = specials.Select(s => s.MapToSpecialListItem(false)).ToList();
                }
                
                return specialListItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving specials for venue with ID {VenueId}", id);
                return null;
            }
        }   

        public async Task<VenueItemExtended> CreateVenueAsync(CreateVenueRequest request, string userId)
        {     
            try
            {
                var fullAddress = $"{request.Address.StreetAddress}, {request.Address.Locality}, {request.Address.Region} {request.Address.Postcode}, {request.Address.Country}";
                var geocodeResponse = await _mapsApiService.SearchClient.GetGeocodingAsync(fullAddress);
                
                Point geocodedPoint;
                
                if (geocodeResponse.Value.Features.Count > 0)
                {
                    var result = geocodeResponse.Value.Features[0];
                    geocodedPoint = new Point(result.Geometry.Coordinates.Longitude, result.Geometry.Coordinates.Latitude) { SRID = 4326 };
                }
                else
                {
                    throw new InvalidOperationException("Unable to geocode address");
                }
                
                var venue = request.MapToNewVenue(userId, geocodedPoint);

                if (request.HoursOfOperation != null)
                {
                    venue.BusinessHours = request.HoursOfOperation.Select(oh => oh.MapToNewOperatingSchedule(venue.Id)).ToList();
                }
                
                await _unitOfWork.Venues.AddAsync(venue);
                await _unitOfWork.SaveChangesAsync();
                
                var createdVenue = await _unitOfWork.Venues.GetVenueWithDetailsAsync(venue.Id);
                if (createdVenue == null)
                {
                    throw new InvalidOperationException("Failed to retrieve created venue");
                }
                
                return createdVenue.MapToVenueDetail();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating venue");
                throw;
            }
        }

        public async Task<VenueItemExtended> UpdateVenueAsync(string id, UpdateVenueRequest request, string userId)
        {
            try
            {
                if (!long.TryParse(id, out long venueId))
                {
                    throw new ArgumentException("Invalid venue ID format");
                }
                
                var venue = await _unitOfWork.Venues.GetVenueWithDetailsAsync(venueId);
                if (venue == null)
                {
                    throw new KeyNotFoundException($"Venue with ID {id} not found");
                }
                
                var fullAddress = $"{request.Address.StreetAddress}, {request.Address.Locality}, {request.Address.Region} {request.Address.Postcode}, {request.Address.Country}";
                var geocodeResponse = await _mapsApiService.SearchClient.GetGeocodingAsync(fullAddress);
                
                Point geocodedPoint;
                if (geocodeResponse.Value.Features.Count > 0)
                {
                    var result = geocodeResponse.Value.Features[0];
                    geocodedPoint = new Point(result.Geometry.Coordinates.Longitude, result.Geometry.Coordinates.Latitude) { SRID = 4326 };
                }
                else
                {
                    throw new InvalidOperationException("Unable to geocode address");
                }
                
                request.MapAndUpdateExistingVenue(venue, userId, geocodedPoint);

                _unitOfWork.Venues.Update(venue);
                await _unitOfWork.SaveChangesAsync();
                
                var updatedVenue = await _unitOfWork.Venues.GetVenueWithDetailsAsync(venueId);
                if (updatedVenue == null)
                {
                    throw new InvalidOperationException("Failed to retrieve updated venue");
                }
                
                return updatedVenue.MapToVenueDetail();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating venue with ID {VenueId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteVenueAsync(string id, string userId)
        {
            try
            {
                if (!long.TryParse(id, out long venueId))
                {
                    _logger.LogWarning("Invalid venue ID format: {Id}", id);
                    return false;
                }
                
                var venue = await _unitOfWork.Venues.GetByIdAsync(venueId);
                if (venue == null)
                {
                    return false;
                }
                
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
    }
}