namespace MirthSystems.Pulse.Services.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models;
    using MirthSystems.Pulse.Core.Models.Requests;
    using NSwag.Annotations;
    using System.Security.Claims;

    [Route("api/specials")]
    public class SpecialsController : ControllerBase
    {
        private readonly ISpecialService _specialService;
        private readonly IVenueService _venueService;
        private string? UserId => User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public SpecialsController(ISpecialService specialService, IVenueService venueService)
        {
            _specialService = specialService;
            _venueService = venueService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<SearchSpecialsResult>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [OpenApiOperation("SearchSpecials", "Searches for specials grouped by venue with optional filtering")]
        public async Task<ActionResult<PagedResult<SearchSpecialsResult>>> SearchSpecials([FromQuery] GetSpecialsRequest request)
        {
            try
            {
                var results = await _specialService.SearchSpecialsAsync(request);
                return Ok(results);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves detailed information about a specific special.
        /// </summary>
        /// <param name="id">The special ID.</param>
        /// <returns>The special details.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SpecialItemExtended))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("GetSpecialById", "Retrieves detailed information about a specific special")]
        public async Task<ActionResult<SpecialItemExtended>> GetSpecialById(string id)
        {
            try
            {
                var special = await _specialService.GetSpecialByIdAsync(id);
                if (special == null)
                {
                    return NotFound($"Special with ID {id} not found");
                }
                return Ok(special);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new special promotion for a venue.
        /// </summary>
        /// <param name="request">The data for creating the special.</param>
        /// <returns>The created special with its assigned ID.</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SpecialItemExtended))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [OpenApiOperation("CreateSpecial", "Creates a new special promotion")]
        public async Task<ActionResult<SpecialItemExtended>> CreateSpecial([FromBody] CreateSpecialRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (UserId == null)
                {
                    return Unauthorized("User must be authenticated to create specials");
                }

                var special = await _specialService.CreateSpecialAsync(request, UserId);
                return CreatedAtAction(nameof(GetSpecialById), new { id = special.Id }, special);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing special promotion.
        /// </summary>
        /// <param name="id">The special ID.</param>
        /// <param name="request">The update data.</param>
        /// <returns>The updated special.</returns>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SpecialItemExtended))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("UpdateSpecial", "Updates an existing special promotion")]
        public async Task<ActionResult<SpecialItemExtended>> UpdateSpecial(string id, [FromBody] UpdateSpecialRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (UserId == null)
                {
                    return Unauthorized("User must be authenticated to update specials");
                }

                var updatedSpecial = await _specialService.UpdateSpecialAsync(id, request, UserId);
                return Ok(updatedSpecial);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a special promotion.
        /// </summary>
        /// <param name="id">The special ID.</param>
        /// <returns>True if deletion was successful.</returns>
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
            try
            {
                if (UserId == null)
                {
                    return Unauthorized("User must be authenticated to delete specials");
                }

                var result = await _specialService.DeleteSpecialAsync(id, UserId);
                if (!result)
                {
                    return NotFound($"Special with ID {id} not found");
                }

                return Ok(true);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves specials for a specific venue.
        /// </summary>
        /// <param name="venueId">The venue ID.</param>
        /// <returns>A list of specials for the venue.</returns>
        [HttpGet("venue/{venueId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<SpecialItem>>> GetVenueSpecials(string venueId)
        {
            if (string.IsNullOrEmpty(venueId) || !long.TryParse(venueId, out long venueIdLong))
            {
                return BadRequest("Invalid venue ID format");
            }

            var specials = await _venueService.GetVenueSpecialsAsync(venueId);

            if (specials == null)
            {
                return NotFound();
            }

            return Ok(specials);
        }
    }
}
