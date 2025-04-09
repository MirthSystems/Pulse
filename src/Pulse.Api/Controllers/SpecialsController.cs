namespace Pulse.Api.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Pulse.Core.Contracts;
    using Pulse.Core.Models.Requests;

    using Pulse.Core.Models.Responses;

    [ApiController]
    [Route("api/specials")]
    public class SpecialsController : ControllerBase
    {
        private readonly ISpecialService _specialService;
        private readonly IVenueService _venueService;
        private readonly ILogger<SpecialsController> _logger;

        public SpecialsController(
            ISpecialService specialService,
            IVenueService venueService,
            ILogger<SpecialsController> logger)
        {
            _specialService = specialService;
            _venueService = venueService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<SpecialListResponse>> GetSpecials([FromQuery] SpecialQueryRequest request)
        {
            try
            {
                var response = await _specialService.GetSpecialsAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving specials");
                return StatusCode(500, "An error occurred while retrieving specials.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SpecialDetailResponse>> GetSpecialById(int id)
        {
            try
            {
                var response = await _specialService.GetSpecialByIdAsync(id);

                if (response == null)
                {
                    return NotFound($"Special with ID {id} not found.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving special with ID {SpecialId}", id);
                return StatusCode(500, "An error occurred while retrieving the special.");
            }
        }

        [HttpPost]
        [Authorize(Policy = "SpecialManagement")]
        public async Task<ActionResult<NewSpecialResponse>> CreateSpecial([FromBody] NewSpecialRequest newSpecialRequest)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID not found in claims.");
                }
                // Check if user can manage this venue's specials
                if (!await _venueService.UserCanManageVenueSpecialsAsync(newSpecialRequest.VenueId, userId))
                {
                    return Forbid("You do not have permission to create specials for this venue.");
                }

                var response = await _specialService.CreateSpecialAsync(newSpecialRequest, userId);
                return CreatedAtAction(nameof(GetSpecialById), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating special");
                return StatusCode(500, "An error occurred while creating the special.");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "SpecialManagement")]
        public async Task<ActionResult<UpdateSpecialResponse>> UpdateSpecial(int id, [FromBody] UpdateSpecialRequest updateSpecialRequest)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID not found in claims.");
                }

                // Get the special's venue ID to check permissions
                var special = await _specialService.GetSpecialByIdAsync(id);
                if (special == null)
                {
                    return NotFound($"Special with ID {id} not found.");
                }

                // Check if user can manage this venue's specials
                if (!await _venueService.UserCanManageVenueSpecialsAsync(special.VenueId, userId))
                {
                    return Forbid("You do not have permission to update specials for this venue.");
                }

                var response = await _specialService.UpdateSpecialAsync(id, updateSpecialRequest, userId);

                if (response == null)
                {
                    return NotFound($"Special with ID {id} not found.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating special with ID {SpecialId}", id);
                return StatusCode(500, "An error occurred while updating the special.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "SpecialManagement")]
        public async Task<ActionResult> DeleteSpecial(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID not found in claims.");
                }

                // Get the special's venue ID to check permissions
                var special = await _specialService.GetSpecialByIdAsync(id);
                if (special == null)
                {
                    return NotFound($"Special with ID {id} not found.");
                }

                // Check if user can manage this venue's specials
                if (!await _venueService.UserCanManageVenueSpecialsAsync(special.VenueId, userId))
                {
                    return Forbid("You do not have permission to delete specials for this venue.");
                }

                var success = await _specialService.DeleteSpecialAsync(id);

                if (!success)
                {
                    return NotFound($"Special with ID {id} not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting special with ID {SpecialId}", id);
                return StatusCode(500, "An error occurred while deleting the special.");
            }
        }

        [HttpGet("types")]
        public async Task<ActionResult<SpecialTypeListResponse>> GetSpecialTypes()
        {
            try
            {
                var response = await _specialService.GetSpecialTypesAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving special types");
                return StatusCode(500, "An error occurred while retrieving special types.");
            }
        }

        [HttpGet("tags")]
        public async Task<ActionResult<TagListResponse>> GetTags([FromQuery] string? searchTerm = null)
        {
            try
            {
                var response = await _specialService.GetTagsAsync(searchTerm);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tags");
                return StatusCode(500, "An error occurred while retrieving tags.");
            }
        }
    }
}
