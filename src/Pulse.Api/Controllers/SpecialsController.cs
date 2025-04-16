namespace Pulse.Api.Controllers
{
    using AutoMapper;
    using System.Net;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using NodaTime;
    using Pulse.Core.Contracts;

    using Pulse.Core.Enums;

    using Pulse.Core.Models;
    using Pulse.Core.Utilities;
    using NodaTime.Text;

    [ApiController]
    [Route("api/specials")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class SpecialsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVenueLocationService _venueLocationService;
        private readonly ILocationService _locationService;
        private readonly IClock _clock;
        private readonly IMapper _mapper;

        public SpecialsController(
            IUnitOfWork unitOfWork,
            IVenueLocationService venueLocationService,
            ILocationService locationService,
            IClock clock,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _venueLocationService = venueLocationService;
            _locationService = locationService;
            _clock = clock;
            _mapper = mapper;
        }

        /// <summary>
        /// Finds venues with active specials near a geographic point
        /// </summary>
        /// <param name="latitude">Latitude of search point</param>
        /// <param name="longitude">Longitude of search point</param>
        /// <param name="radiusMiles">Search radius in miles (default: 5)</param>
        /// <returns>List of venues with active specials</returns>
        [HttpGet("nearby")]
        [ProducesResponseType(typeof(IEnumerable<VenueWithActiveSpecials>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> FindVenuesWithSpecialsNearPoint(
            [FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] double radiusMiles = 5)
        {
            var venuesWithDistance = await _venueLocationService.FindVenuesWithActiveSpecialsNearPointAsync(
                latitude, longitude, radiusMiles);

            var result = await CreateVenueWithActiveSpecialsItems(venuesWithDistance);

            return Ok(result);
        }

        /// <summary>
        /// Finds venues with active specials near an address
        /// </summary>
        /// <param name="address">Search address</param>
        /// <param name="radiusMiles">Search radius in miles (default: 5)</param>
        /// <returns>List of venues with active specials</returns>
        [HttpGet("nearby/address")]
        [ProducesResponseType(typeof(IEnumerable<VenueWithActiveSpecials>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindVenuesWithSpecialsNearAddress(
            [FromQuery] string address,
            [FromQuery] double radiusMiles = 5)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return BadRequest("Address is required");
            }

            var venuesWithDistance = await _venueLocationService.FindVenuesWithActiveSpecialsNearAddressAsync(
                address, radiusMiles);

            var result = await CreateVenueWithActiveSpecialsItems(venuesWithDistance);

            return Ok(result);
        }

        /// <summary>
        /// Finds venues with active specials at a specific time near a geographic point
        /// </summary>
        /// <param name="latitude">Latitude of search point</param>
        /// <param name="longitude">Longitude of search point</param>
        /// <param name="dateTime">ISO-8601 formatted date and time to check for specials (UTC)</param>
        /// <param name="radiusMiles">Search radius in miles (default: 5)</param>
        /// <returns>List of venues with active specials at the specified time</returns>
        [HttpGet("nearby/future")]
        [ProducesResponseType(typeof(IEnumerable<VenueWithActiveSpecials>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindVenuesWithSpecialsForTimeNearPoint(
            [FromQuery] double latitude,
            [FromQuery] double longitude,
            [FromQuery] string dateTime,
            [FromQuery] double radiusMiles = 5)
        {
            if (string.IsNullOrWhiteSpace(dateTime))
            {
                return BadRequest("DateTime is required");
            }

            var parseResult = InstantPattern.ExtendedIso.Parse(dateTime);
            if (!parseResult.Success)
            {
                return BadRequest($"Invalid date-time format: {parseResult.Exception.Message}");
            }

            var specificTime = parseResult.Value;

            var venuesWithDistance = await _venueLocationService.FindVenuesWithActiveSpecialsForTimeNearPointAsync(
                latitude, longitude, specificTime, radiusMiles);

            var result = await CreateVenueWithActiveSpecialsItems(venuesWithDistance, specificTime);

            return Ok(result);
        }

        /// <summary>
        /// Finds venues with active specials at a specific time near an address
        /// </summary>
        /// <param name="address">Search address</param>
        /// <param name="dateTime">ISO-8601 formatted date and time to check for specials (UTC)</param>
        /// <param name="radiusMiles">Search radius in miles (default: 5)</param>
        /// <returns>List of venues with active specials at the specified time</returns>
        [HttpGet("nearby/address/future")]
        [ProducesResponseType(typeof(IEnumerable<VenueWithActiveSpecials>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindVenuesWithSpecialsForTimeNearAddress(
            [FromQuery] string address,
            [FromQuery] string dateTime,
            [FromQuery] double radiusMiles = 5)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return BadRequest("Address is required");
            }

            if (string.IsNullOrWhiteSpace(dateTime))
            {
                return BadRequest("DateTime is required");
            }

            var parseResult = InstantPattern.ExtendedIso.Parse(dateTime);
            if (!parseResult.Success)
            {
                return BadRequest($"Invalid date-time format: {parseResult.Exception.Message}");
            }

            var specificTime = parseResult.Value;

            var venuesWithDistance = await _venueLocationService.FindVenuesWithActiveSpecialsForTimeNearAddressAsync(
                address, specificTime, radiusMiles);

            var result = await CreateVenueWithActiveSpecialsItems(venuesWithDistance, specificTime);

            return Ok(result);
        }

        /// <summary>
        /// Gets active specials for a specific venue
        /// </summary>
        /// <param name="venueId">Venue ID</param>
        /// <returns>List of active specials</returns>
        [HttpGet("venues/{venueId}")]
        [ProducesResponseType(typeof(IEnumerable<SpecialItem>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetVenueSpecials(long venueId)
        {
            var venue = await _unitOfWork.Venues.GetByIdAsync(venueId);
            if (venue == null)
            {
                return NotFound();
            }

            var specials = await _unitOfWork.Specials.GetActiveForVenueAsync(venueId);
            return Ok(_mapper.Map<IEnumerable<SpecialItem>>(specials));
        }

        /// <summary>
        /// Gets active specials by type
        /// </summary>
        /// <param name="type">Special type (Food, Drink, Entertainment)</param>
        /// <returns>List of active specials by type</returns>
        [HttpGet("by-type/{type}")]
        [ProducesResponseType(typeof(IEnumerable<SpecialWithVenue>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetSpecialsByType(SpecialTypes type)
        {
            var specials = await _unitOfWork.Specials.GetActiveByTypeAsync(type);

            var result = new List<SpecialWithVenue>();
            foreach (var special in specials)
            {
                var venue = await _unitOfWork.Venues.GetByIdAsync(special.VenueId);
                if (venue != null)
                {
                    special.Venue = venue;
                    result.Add(_mapper.Map<SpecialWithVenue>(special));
                }
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets specials associated with a tag
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <returns>List of specials with the tag</returns>
        [HttpGet("by-tag/{tagName}")]
        [ProducesResponseType(typeof(IEnumerable<SpecialWithVenue>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetSpecialsByTag(string tagName)
        {
            var tag = await _unitOfWork.Tags.GetByNameAsync(tagName);
            if (tag == null)
            {
                return NotFound();
            }

            var tagWithSpecials = await _unitOfWork.Tags.GetWithSpecialsAsync(tag.Id);
            if (tagWithSpecials == null || !tagWithSpecials.Specials.Any())
            {
                return Ok(Array.Empty<SpecialWithVenue>());
            }

            var result = new List<SpecialWithVenue>();
            foreach (var specialLink in tagWithSpecials.Specials)
            {
                var special = await _unitOfWork.Specials.GetByIdAsync(specialLink.SpecialId);
                if (special != null)
                {
                    var venue = await _unitOfWork.Venues.GetByIdAsync(special.VenueId);
                    if (venue != null)
                    {
                        special.Venue = venue;
                        result.Add(_mapper.Map<SpecialWithVenue>(special));
                    }
                }
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets details for a specific special
        /// </summary>
        /// <param name="id">Special ID</param>
        /// <returns>Special details with venue information</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SpecialWithDetails), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetSpecialDetails(long id)
        {
            var special = await _unitOfWork.Specials.GetWithAllDataAsync(id);
            if (special == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<SpecialWithDetails>(special));
        }

        /// <summary>
        /// Helper method to create Items for venues with their active specials
        /// </summary>
        /// <param name="venuesWithDistance">Collection of venues with distance information</param>
        /// <param name="checkTime">Optional specific time to check for active specials (defaults to current time)</param>
        private async Task<IEnumerable<VenueWithActiveSpecials>> CreateVenueWithActiveSpecialsItems(
            IEnumerable<VenueWithDistance> venuesWithDistance,
            Instant? checkTime = null)
        {
            var result = new List<VenueWithActiveSpecials>();
            var timeToCheck = checkTime ?? _clock.GetCurrentInstant();

            foreach (var venueWithDistance in venuesWithDistance)
            {
                var venue = await _unitOfWork.Venues.GetWithSpecialsAsync(venueWithDistance.Venue.Id);
                if (venue != null)
                {
                    var activeSpecials = venue.Specials
                        .Where(s => SpecialHelper.IsActive(s, timeToCheck))
                        .ToList();

                    var tuple = (Venue: venue, DistanceMiles: venueWithDistance.DistanceMiles, ActiveSpecials: activeSpecials);
                    result.Add(_mapper.Map<VenueWithActiveSpecials>(tuple));
                }
            }

            return result.OrderBy(v => v.DistanceMiles).ToList();
        }
    }
}
