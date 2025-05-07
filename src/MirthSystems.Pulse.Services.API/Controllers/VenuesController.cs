namespace MirthSystems.Pulse.Services.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models.Responses;
    using MirthSystems.Pulse.Core.Models;
    using MirthSystems.Pulse.Services.API.Controllers.Base;

    [Route("api/venues")]
    public class VenuesController : ApiController
    {
        private readonly IVenueService _venueService;

        public VenuesController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedApiResponse<VenueListItem>))]
        public async Task<ActionResult<PagedApiResponse<VenueListItem>>> GetVenues([FromQuery] PageQueryParams pageQuery)
        {
            pageQuery.PageSize = pageQuery.PageSize > 10000 ? 10000 : pageQuery.PageSize;
            var venues = await _venueService.GetVenuesAsync(pageQuery.Page, pageQuery.PageSize);
            return Ok(venues);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<VenueDetail>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<VenueDetail>>> GetVenueById(string id)
        {
            if (!long.TryParse(id, out long venueId))
            {
                return BadRequest(ApiResponse<object>.CreateError("Invalid venue ID format"));
            }

            var venue = await _venueService.GetVenueByIdAsync(id);
            if (venue == null)
            {
                return NotFound(ApiResponse<object>.CreateError($"Venue with ID {id} not found"));
            }

            return Ok(ApiResponse<VenueDetail>.CreateSuccess(venue));
        }

        [HttpGet("{id}/business-hours")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<BusinessHours>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<BusinessHours>>> GetVenueBusinessHours(string id)
        {
            if (!long.TryParse(id, out long venueId))
            {
                return BadRequest(ApiResponse<object>.CreateError("Invalid venue ID format"));
            }

            var businessHours = await _venueService.GetVenueBusinessHoursAsync(id);
            if (businessHours == null)
            {
                return NotFound(ApiResponse<object>.CreateError($"Business hours for venue with ID {id} not found"));
            }

            return Ok(ApiResponse<BusinessHours>.CreateSuccess(businessHours));
        }

        [HttpGet("{id}/specials")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<VenueSpecials>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<VenueSpecials>>> GetVenueSpecials(string id)
        {
            if (!long.TryParse(id, out long venueId))
            {
                return BadRequest(ApiResponse<object>.CreateError("Invalid venue ID format"));
            }

            var specials = await _venueService.GetVenueSpecialsAsync(id);
            if (specials == null)
            {
                return NotFound(ApiResponse<object>.CreateError($"Specials for venue with ID {id} not found"));
            }

            return Ok(ApiResponse<VenueSpecials>.CreateSuccess(specials));
        }

        [HttpPost]
        [Authorize(Roles = "System.Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResponse<VenueDetail>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<VenueDetail>>> CreateVenue([FromBody] CreateVenueRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.CreateError("Invalid request data"));
            }

            if (UserId == null)
            {
                return Unauthorized(ApiResponse<object>.CreateError("Unauthorized"));
            }

            var venue = await _venueService.CreateVenueAsync(request, UserId);
            var response = ApiResponse<VenueDetail>.CreateSuccess(venue, "Venue created successfully");
            return CreatedAtAction(nameof(GetVenueById), new { id = venue.Id }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<VenueDetail>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<VenueDetail>>> UpdateVenue(string id, [FromBody] UpdateVenueRequest request)
        {
            if (!long.TryParse(id, out long venueId))
            {
                return BadRequest(ApiResponse<object>.CreateError("Invalid venue ID format"));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.CreateError("Invalid request data"));
            }

            if (UserId == null)
            {
                return Unauthorized(ApiResponse<object>.CreateError("Unauthorized"));
            }

            try
            {
                var venue = await _venueService.UpdateVenueAsync(id, request, UserId);
                return Ok(ApiResponse<VenueDetail>.CreateSuccess(venue, "Venue updated successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.CreateError($"Venue with ID {id} not found"));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteVenue(string id)
        {
            if (!long.TryParse(id, out long venueId))
            {
                return BadRequest(ApiResponse<object>.CreateError("Invalid venue ID format"));
            }

            if (UserId == null)
            {
                return Unauthorized(ApiResponse<object>.CreateError("Unauthorized"));
            }

            bool result = await _venueService.DeleteVenueAsync(id, UserId);
            if (!result)
            {
                return NotFound(ApiResponse<object>.CreateError($"Venue with ID {id} not found"));
            }

            return Ok(ApiResponse<bool>.CreateSuccess(true, "Venue deleted successfully"));
        }
    }
}
