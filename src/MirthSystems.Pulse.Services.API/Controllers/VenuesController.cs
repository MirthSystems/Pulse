namespace MirthSystems.Pulse.Services.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models;
    using MirthSystems.Pulse.Services.API.Controllers.Base;
    using System.ComponentModel.DataAnnotations;
    using NSwag.Annotations;
    using MirthSystems.Pulse.Infrastructure.Services;

    /// <summary>
    /// API controller for venue-related endpoints.
    /// </summary>
    /// <remarks>
    /// <para>This controller provides endpoints for retrieving and managing venue information, including:</para>
    /// <para>- Listing venues with comprehensive filtering options</para>
    /// <para>- Retrieving detailed venue information</para>
    /// <para>- Managing venue business hours and specials</para>
    /// <para>- Creating, updating, and deleting venues</para>
    /// </remarks>
    [Route("api/venues")]
    public class VenuesController : ApiController
    {
        private readonly IVenueService _venueService;

        public VenuesController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        /// <summary>
        /// Retrieves venues with optional filtering.
        /// </summary>
        /// <param name="request">The request containing pagination and filter criteria.</param>
        /// <returns>A paginated list of venues.</returns>
        /// <remarks>
        /// <para>This endpoint supports sophisticated filtering capabilities:</para>
        /// <para>- Text search: Filter venues by name or description</para>
        /// <para>- Location-based: Find venues near a specific address</para>
        /// <para>- Open hours: Filter venues open on specific days and times</para>
        /// <para>- Special availability: Find venues with active specials</para>
        /// <para>All filters are optional and can be combined for advanced searching.</para>
        /// </remarks>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<VenueListItem>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("GetVenues", "Retrieves a paginated list of venues with optional filtering")]
        public async Task<IActionResult> GetVenues([FromQuery] GetVenuesRequest request)
        {
            // Enforce safety limit on page size
            if (request.PageSize > 10000)
            {
                request.PageSize = 10000;
            }
            
            var venues = await _venueService.GetVenuesAsync(request);
            if (venues == null || venues.Items.Count == 0)
            {
                return NotFound("No venues found matching the criteria");
            }
            return Ok(venues);
        }

        /// <summary>
        /// Retrieves a venue by its ID.
        /// </summary>
        /// <param name="id">The venue ID.</param>
        /// <returns>The venue details.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VenueDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("GetVenueById", "Retrieves detailed information about a specific venue")]
        public async Task<IActionResult> GetVenueById(string id)
        {
            if (!long.TryParse(id, out long venueId))
            {
                return BadRequest("Invalid venue ID format");
            }

            var venue = await _venueService.GetVenueByIdAsync(id);
            if (venue == null)
            {
                return NotFound($"Venue with ID {id} not found");
            }

            return Ok(venue);
        }

        /// <summary>
        /// Retrieves the business hours for a specific venue.
        /// </summary>
        /// <param name="id">The unique identifier of the venue.</param>
        /// <returns>The business hours for the specified venue.</returns>
        /// <response code="200">Returns the venue's business hours information.</response>
        /// <response code="400">If the venue ID format is invalid.</response>
        /// <response code="404">If business hours for the specified venue are not found.</response>
        /// <remarks>
        /// <para>This endpoint provides the complete weekly operating schedule for a venue.</para>
        /// <para>It returns consolidated business hours information organized by day of the week.</para>
        /// <para>Useful for displaying when a venue is open to visitors.</para>
        /// </remarks>
        [HttpGet("{id}/business-hours")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BusinessHours))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("GetVenueBusinessHours", "Retrieves the business hours for a specific venue")]
        public async Task<IActionResult> GetVenueBusinessHours(string id)
        {
            if (!long.TryParse(id, out long venueId))
            {
                return BadRequest("Invalid venue ID format");
            }

            var businessHours = await _venueService.GetVenueBusinessHoursAsync(id);
            if (businessHours == null)
            {
                return NotFound($"Business hours for venue with ID {id} not found");
            }

            return Ok(businessHours);
        }

        /// <summary>
        /// Retrieves all special promotions offered by a specific venue.
        /// </summary>
        /// <param name="id">The unique identifier of the venue.</param>
        /// <returns>The specials offered by the specified venue.</returns>
        /// <response code="200">Returns the venue's special promotions.</response>
        /// <response code="400">If the venue ID format is invalid.</response>
        /// <response code="404">If no specials exist for the specified venue.</response>
        /// <remarks>
        /// <para>This endpoint provides comprehensive information about all special promotions offered by a venue.</para>
        /// <para>It includes details about each special and its current active status.</para>
        /// <para>This data is useful for displaying venue promotions on venue detail pages.</para>
        /// </remarks>
        [HttpGet("{id}/specials")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VenueSpecials))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("GetVenueSpecials", "Retrieves all special promotions offered by a specific venue")]
        public async Task<IActionResult> GetVenueSpecials(string id)
        {
            if (!long.TryParse(id, out long venueId))
            {
                return BadRequest("Invalid venue ID format");
            }

            var specials = await _venueService.GetVenueSpecialsAsync(id);
            if (specials == null)
            {
                return NotFound($"Specials for venue with ID {id} not found");
            }

            return Ok(specials);
        }

        /// <summary>
        /// Creates a new venue.
        /// </summary>
        /// <param name="request">The data for creating the venue.</param>
        /// <returns>The created venue with its assigned ID.</returns>
        /// <response code="201">Returns the newly created venue.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user doesn't have the required administrator role.</response>
        /// <remarks>
        /// <para>This endpoint allows system administrators to create new venues in the system.</para>
        /// <para>It handles address validation and geocoding through integrated mapping services.</para>
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = "System.Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VenueDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [OpenApiOperation("CreateVenue", "Creates a new venue")]
        public async Task<IActionResult> CreateVenue([FromBody] CreateVenueRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request data");
            }

            if (UserId == null)
            {
                return Unauthorized("Unauthorized");
            }

            try
            {
                var venue = await _venueService.CreateVenueAsync(request, UserId);
                return CreatedAtAction(nameof(GetVenueById), new { id = venue.Id }, venue);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing venue's information.
        /// </summary>
        /// <param name="id">The unique identifier of the venue to update.</param>
        /// <param name="request">The update data for the venue.</param>
        /// <returns>The updated venue information.</returns>
        /// <response code="200">Returns the updated venue details.</response>
        /// <response code="400">If the ID format or request data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user doesn't have the required administrator role.</response>
        /// <response code="404">If the venue with the specified ID is not found.</response>
        /// <remarks>
        /// <para>This endpoint allows system administrators to update existing venue information.</para>
        /// <para>It handles address validation and geocoding if the address is being updated.</para>
        /// <para>Only fields included in the request are updated; omitted fields remain unchanged.</para>
        /// </remarks>
        [HttpPut("{id}")]
        [Authorize(Roles = "System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VenueDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("UpdateVenue", "Updates an existing venue's information")]
        public async Task<IActionResult> UpdateVenue([FromQuery] string id, [FromBody] UpdateVenueRequest request)
        {
            if (!long.TryParse(id, out long venueId))
            {
                return BadRequest("Invalid venue ID format");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request data");
            }

            if (UserId == null)
            {
                return Unauthorized("Unauthorized");
            }

            try
            {
                var venue = await _venueService.UpdateVenueAsync(id, request, UserId);
                return Ok(venue);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Venue with ID {id} not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Soft-deletes a venue.
        /// </summary>
        /// <param name="id">The unique identifier of the venue to delete.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        /// <response code="200">Returns true if deletion was successful.</response>
        /// <response code="400">If the ID format is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user doesn't have the required administrator role.</response>
        /// <response code="404">If the venue with the specified ID is not found.</response>
        /// <remarks>
        /// <para>This endpoint soft-deletes a venue, marking it as deleted in the database but preserving the record.</para>
        /// <para>This allows for potential recovery and maintains historical data integrity.</para>
        /// </remarks>
        [HttpDelete("{id}")]
        [Authorize(Roles = "System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("DeleteVenue", "Soft-deletes a venue")]
        public async Task<IActionResult> DeleteVenue(string id)
        {
            if (!long.TryParse(id, out long venueId))
            {
                return BadRequest("Invalid venue ID format");
            }

            if (UserId == null)
            {
                return Unauthorized("Unauthorized");
            }

            bool result = await _venueService.DeleteVenueAsync(id, UserId);
            if (!result)
            {
                return NotFound($"Venue with ID {id} not found");
            }

            return Ok(true);
        }
    }
}
