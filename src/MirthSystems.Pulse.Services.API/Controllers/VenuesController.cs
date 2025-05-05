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

        /// <summary>
        /// Initializes a new instance of the <see cref="VenuesController"/> class.
        /// </summary>
        /// <param name="venueService">The venue service</param>
        public VenuesController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        /// <summary>
        /// Gets all venues with pagination
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 20, max: 100)</param>
        /// <returns>A paged list of venues</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedApiResponse<VenueListItem>))]
        public async Task<ActionResult<PagedApiResponse<VenueListItem>>> GetVenues(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            pageSize = pageSize > 100 ? 100 : pageSize;
            var venues = await _venueService.GetVenuesAsync(page, pageSize);
            return Ok(venues);
        }

        /// <summary>
        /// Gets a venue by ID with its address and business hours
        /// </summary>
        /// <param name="id">The venue ID</param>
        /// <returns>The venue with details</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<VenueDetail>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<VenueDetail>>> GetVenueById(long id)
        {
            var venue = await _venueService.GetVenueByIdAsync(id);

            if (venue == null)
            {
                return NotFound(ApiResponse<object>.CreateError($"Venue with ID {id} not found"));
            }

            return Ok(ApiResponse<VenueDetail>.CreateSuccess(venue));
        }

        /// <summary>
        /// Gets the business hours for a venue
        /// </summary>
        /// <param name="id">The venue ID</param>
        /// <returns>The venue's business hours</returns>
        [HttpGet("{id}/business-hours")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<BusinessHours>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<BusinessHours>>> GetVenueBusinessHours(long id)
        {
            var businessHours = await _venueService.GetVenueBusinessHoursAsync(id);

            if (businessHours == null)
            {
                return NotFound(ApiResponse<object>.CreateError($"Business hours for venue with ID {id} not found"));
            }

            return Ok(ApiResponse<BusinessHours>.CreateSuccess(businessHours));
        }

        /// <summary>
        /// Gets the specials for a venue
        /// </summary>
        /// <param name="id">The venue ID</param>
        /// <returns>The venue's specials</returns>
        [HttpGet("{id}/specials")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<VenueSpecials>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<VenueSpecials>>> GetVenueSpecials(long id)
        {
            var specials = await _venueService.GetVenueSpecialsAsync(id);

            if (specials == null)
            {
                return NotFound(ApiResponse<object>.CreateError($"Specials for venue with ID {id} not found"));
            }

            return Ok(ApiResponse<VenueSpecials>.CreateSuccess(specials));
        }

        /// <summary>
        /// Creates a new venue
        /// </summary>
        /// <param name="request">The venue creation request</param>
        /// <returns>The created venue</returns>
        [HttpPost]
        [Authorize(Roles = "System.Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResponse<VenueDetail>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Updates an existing venue
        /// </summary>
        /// <param name="id">The venue ID</param>
        /// <param name="request">The venue update request</param>
        /// <returns>The updated venue</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<VenueDetail>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<VenueDetail>>> UpdateVenue(long id, [FromBody] UpdateVenueRequest request)
        {
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

        /// <summary>
        /// Deletes a venue
        /// </summary>
        /// <param name="id">The venue ID</param>
        /// <returns>Success indicator</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteVenue(long id)
        {
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
