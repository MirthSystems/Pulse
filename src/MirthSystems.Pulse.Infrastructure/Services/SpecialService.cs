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
    using MirthSystems.Pulse.Core.Models;
    using NodaTime;
    using NetTopologySuite.Geometries;
    using Azure.Maps.Search.Models;
    using Microsoft.IdentityModel.Tokens;
    using MirthSystems.Pulse.Core.Extensions;
    using MirthSystems.Pulse.Core.Utilities;

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

        public async Task<PagedResult<SearchSpecialsResult>> SearchSpecialsAsync(GetSpecialsRequest request)
        {
            try
            {
                Point? searchLocation = null;
                double? radiusInMeters = null;

                if (!string.IsNullOrEmpty(request.Address))
                {
                    var geocodeResponse = await _mapsApiService.SearchClient.GetGeocodingAsync(request.Address);
                    if (geocodeResponse.Value.Features.Count > 0)
                    {
                        var result = geocodeResponse.Value.Features[0];
                        searchLocation = new Point(result.Geometry.Coordinates.Longitude, result.Geometry.Coordinates.Latitude) { SRID = 4326 };
                        radiusInMeters = request.Radius * 1609.34;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Could not geocode address: {request.Address}");
                    }
                }
                else
                {
                    return new PagedResult<SearchSpecialsResult>
                    {
                        Items = new List<SearchSpecialsResult>(),
                        PagingInfo = PagingInfo.Create(request.Page, request.PageSize, 0)
                    };
                }

                Instant searchDateTimeInstant;
                if (!string.IsNullOrEmpty(request.SearchDateTime))
                {
                    if (DateTimeOffset.TryParse(request.SearchDateTime, out var parsedDateTime))
                    {
                        searchDateTimeInstant = Instant.FromDateTimeOffset(parsedDateTime);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Invalid search date time format: {request.SearchDateTime}");
                    }
                }
                else
                {
                    searchDateTimeInstant = SystemClock.Instance.GetCurrentInstant();
                }

                var venuesWithSpecials = await _unitOfWork.Venues.GetVenuesWithRunningSpecialsAsync(
                    searchLocation,
                    radiusInMeters.Value,
                    searchDateTimeInstant,
                    request.SearchTerm,
                    request.SpecialTypeId.HasValue ? (Core.Enums.SpecialTypes)request.SpecialTypeId.Value : null);

                var allResults = venuesWithSpecials.Select(venueWithSpecials =>
                {
                    var venue = venueWithSpecials.Venue;
                    var specials = venueWithSpecials.Specials;
                    var specialItems = specials.Select(s => s.MapToSpecialListItem(true)).ToList();
                    return new SearchSpecialsResult
                    {
                        Venue = venue.MapToVenueDetail(),
                        Specials = new SpecialMenu { Items = specialItems }
                    };
                }).ToList();

                var totalCount = allResults.Count;
                var pagedItems = allResults
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                return new PagedResult<SearchSpecialsResult>
                {
                    Items = pagedItems,
                    PagingInfo = PagingInfo.Create(
                        request.Page,
                        request.PageSize,
                        totalCount)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching specials");
                throw;
            }
        }

        public async Task<SpecialItemExtended?> GetSpecialByIdAsync(string id)
        {
            try
            {
                if (!long.TryParse(id, out long specialId))
                {
                    _logger.LogWarning("Invalid special ID format: {Id}", id);
                    return null;
                }
                var special = await _unitOfWork.Specials.GetSpecialWithVenueAsync(specialId);
                if (special == null)
                {
                    return null;
                }
                var isCurrentlyRunning = await _unitOfWork.Specials.IsSpecialCurrentlyActiveAsync(specialId, SystemClock.Instance.GetCurrentInstant());
                return special.MapToSpecialDetail(isCurrentlyRunning);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving special with ID {SpecialId}", id);
                return null;
            }
        }

        public async Task<SpecialItemExtended> CreateSpecialAsync(CreateSpecialRequest request, string userId)
        {
            try
            {
                if (!long.TryParse(request.VenueId, out long venueId))
                {
                    throw new ArgumentException("Invalid venue ID format");
                }
                var venue = await _unitOfWork.Venues.GetByIdAsync(venueId);
                if (venue == null)
                {
                    throw new KeyNotFoundException($"Venue with ID {request.VenueId} not found");
                }

                var special = request.MapToNewSpecial(userId);

                await _unitOfWork.Specials.AddAsync(special);
                await _unitOfWork.SaveChangesAsync();

                var createdSpecial = await _unitOfWork.Specials.GetSpecialWithVenueAsync(special.Id);
                if (createdSpecial == null)
                {
                    throw new InvalidOperationException("Failed to retrieve created special");
                }
                var isCurrentlyRunning = await _unitOfWork.Specials.IsSpecialCurrentlyActiveAsync(special.Id, SystemClock.Instance.GetCurrentInstant());
                return createdSpecial.MapToSpecialDetail(isCurrentlyRunning);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating special");
                throw;
            }
        }

        public async Task<SpecialItemExtended> UpdateSpecialAsync(string id, UpdateSpecialRequest request, string userId)
        {
            try
            {
                if (!long.TryParse(id, out long specialId))
                {
                    throw new ArgumentException("Invalid special ID format");
                }
                var special = await _unitOfWork.Specials.GetSpecialWithVenueAsync(specialId);
                if (special == null)
                {
                    throw new KeyNotFoundException($"Special with ID {id} not found");
                }

                request.MapAndUpdateExistingSpecial(special);
                special.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
                special.UpdatedByUserId = userId;

                _unitOfWork.Specials.Update(special);
                await _unitOfWork.SaveChangesAsync();

                var updatedSpecial = await _unitOfWork.Specials.GetSpecialWithVenueAsync(specialId);
                if (updatedSpecial == null)
                {
                    throw new InvalidOperationException("Failed to retrieve updated special");
                }
                var isCurrentlyRunning = await _unitOfWork.Specials.IsSpecialCurrentlyActiveAsync(specialId, SystemClock.Instance.GetCurrentInstant());
                return updatedSpecial.MapToSpecialDetail(isCurrentlyRunning);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating special with ID {SpecialId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteSpecialAsync(string id, string userId)
        {
            try
            {
                if (!long.TryParse(id, out long specialId))
                {
                    _logger.LogWarning("Invalid special ID format: {Id}", id);
                    return false;
                }
                var special = await _unitOfWork.Specials.GetByIdAsync(specialId);
                if (special == null)
                {
                    return false;
                }

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

        public async Task<bool> IsSpecialCurrentlyRunningAsync(string specialId, DateTimeOffset? referenceTime = null)
        {
            if (!long.TryParse(specialId, out long id))
            {
                return false;
            }

            var special = await _unitOfWork.Specials.GetByIdAsync(id);
            if (special == null)
            {
                return false;
            }

            Instant instant = referenceTime.HasValue 
                ? Instant.FromDateTimeOffset(referenceTime.Value) 
                : DateTimeUtility.GetCurrentInstant();
                
            return SpecialActivityUtility.IsSpecialActive(special, instant);
        }
    }
}
