namespace MirthSystems.Pulse.Services.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models;
    using MirthSystems.Pulse.Services.API.Controllers.Base;
    using NSwag.Annotations;

    /// <summary>
    /// API controller for managing venue specials and promotions.
    /// </summary>
    /// <remarks>
    /// <para>This controller provides endpoints for retrieving and managing special promotions offered by venues:</para>
    /// <para>- Searching and filtering specials with comprehensive criteria</para>
    /// <para>- Retrieving detailed information about specific specials</para>
    /// <para>- Creating, updating, and deleting specials (for authenticated users with appropriate roles)</para>
    /// </remarks>
    [Route("api/specials")]
    public class SpecialsController : ApiController
    {
        private readonly ISpecialService _specialService;

        public SpecialsController(ISpecialService specialService)
        {
            _specialService = specialService;
        }

        /// <summary>
        /// Retrieves specials with comprehensive filtering options.
        /// </summary>
        /// <param name="request">The request containing filtering criteria and pagination options.</param>
        /// <returns>A paginated list of specials matching the filter criteria.</returns>
        /// <remarks>
        /// <para>This endpoint supports the following filtering capabilities:</para>
        /// <para>- Text search: Filter by description</para>
        /// <para>- Location-based: Find specials near a specific address</para>
        /// <para>- Type-based: Filter by special type (Food, Drink, Entertainment)</para>
        /// <para>- Timing: Filter by currently active specials</para>
        /// <para>- Venue: Filter specials by specific venue</para>
        /// <para>All filters are optional and can be combined for refined searching.</para>
        /// </remarks>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<SpecialListItem>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [OpenApiOperation("GetSpecials", "Retrieves a paginated list of specials with optional filtering")]
        public async Task<ActionResult<PagedResult<SpecialListItem>>> GetSpecials([FromQuery] GetSpecialsRequest request)
        {
            var specials = await _specialService.GetSpecialsAsync(request);
            if (specials == null || specials.Items.Count == 0)
            {
                return NotFound("No specials found");
            }
            return Ok(specials);
        }

        /// <summary>
        /// Retrieves detailed information about a specific special.
        /// </summary>
        /// <param name="id">The unique identifier of the special.</param>
        /// <returns>Detailed information about the requested special.</returns>
        /// <response code="200">Returns the special details.</response>
        /// <response code="400">If the ID format is invalid.</response>
        /// <response code="404">If the special with the specified ID is not found.</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SpecialDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("GetSpecialById", "Retrieves detailed information about a specific special promotion")]
        public async Task<ActionResult<SpecialDetail>> GetSpecialById(string id)
        {
            if (!long.TryParse(id, out long specialId))
            {
                return BadRequest("Invalid special ID format");
            }

            var special = await _specialService.GetSpecialByIdAsync(id);
            if (special == null)
            {
                return NotFound($"Special with ID {id} not found");
            }

            return Ok(special);
        }

        /// <summary>
        /// Creates a new special promotion for a venue.
        /// </summary>
        /// <param name="request">The data for creating the special.</param>
        /// <returns>The created special with its assigned ID.</returns>
        /// <response code="201">Returns the newly created special.</response>
        /// <response code="400">If the request data is invalid or if venue is not found.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user doesn't have the required role.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SpecialDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [OpenApiOperation("CreateSpecial", "Creates a new special promotion")]
        public async Task<ActionResult<SpecialDetail>> CreateSpecial([FromBody] CreateSpecialRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (UserId == null)
            {
                return Unauthorized("User must be authenticated to create specials");
            }

            try
            {
                var special = await _specialService.CreateSpecialAsync(request, UserId);
                return CreatedAtAction(nameof(GetSpecialById), new { id = special.Id }, special);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing special promotion.
        /// </summary>
        /// <param name="id">The unique identifier of the special to update.</param>
        /// <param name="request">The update data for the special.</param>
        /// <returns>The updated special information.</returns>
        /// <response code="200">Returns the updated special.</response>
        /// <response code="400">If the ID format or request data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user doesn't have the required role.</response>
        /// <response code="404">If the special with the specified ID is not found.</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SpecialDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("UpdateSpecial", "Updates an existing special promotion")]
        public async Task<ActionResult<SpecialDetail>> UpdateSpecial(string id, [FromBody] UpdateSpecialRequest request)
        {
            if (!long.TryParse(id, out long specialId))
            {
                return BadRequest("Invalid special ID format");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (UserId == null)
            {
                return Unauthorized("User must be authenticated to update specials");
            }

            try
            {
                var special = await _specialService.UpdateSpecialAsync(id, request, UserId);
                return Ok(special);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Special with ID {id} not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Soft-deletes a special promotion.
        /// </summary>
        /// <param name="id">The unique identifier of the special to delete.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        /// <response code="200">Returns true if deletion was successful.</response>
        /// <response code="400">If the ID format is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user doesn't have the required role.</response>
        /// <response code="404">If the special with the specified ID is not found.</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("DeleteSpecial", "Soft-deletes a special promotion")]
        public async Task<ActionResult<bool>> DeleteSpecial(string id)
        {
            if (!long.TryParse(id, out long specialId))
            {
                return BadRequest("Invalid special ID format");
            }

            if (UserId == null)
            {
                return Unauthorized("User must be authenticated to delete specials");
            }

            bool result = await _specialService.DeleteSpecialAsync(id, UserId);
            if (!result)
            {
                return NotFound($"Special with ID {id} not found");
            }

            return Ok(true);
        }
    }
}
