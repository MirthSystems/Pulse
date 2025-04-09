namespace Pulse.Api.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Pulse.Core.Contracts;

    using Pulse.Core.Models.Requests;

    using Pulse.Core.Models.Responses;

    [ApiController]
    [Route("api/venues")]
    public class VenuesController : ControllerBase
    {
        private readonly IVenueService _venueService;
        private readonly ILogger<VenuesController> _logger;

        public VenuesController(IVenueService venueService, ILogger<VenuesController> logger)
        {
            _venueService = venueService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<VenueListResponse>> GetVenues([FromQuery] VenueQueryRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var response = await _venueService.GetVenuesAsync(request, userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving venues");
                return StatusCode(500, "An error occurred while retrieving venues.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VenueDetailResponse>> GetVenueById(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var response = await _venueService.GetVenueByIdAsync(id, userId);

                if (response == null)
                {
                    return NotFound($"Venue with ID {id} not found.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving venue with ID {VenueId}", id);
                return StatusCode(500, "An error occurred while retrieving the venue.");
            }
        }

        [HttpPost]
        [Authorize(Policy = "VenueManagement")]
        public async Task<ActionResult<NewVenueResponse>> CreateVenue([FromBody] NewVenueRequest newVenueRequest)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID not found in claims.");
                }
                var response = await _venueService.CreateVenueAsync(newVenueRequest, userId);
                return CreatedAtAction(nameof(GetVenueById), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating venue");
                return StatusCode(500, "An error occurred while creating the venue.");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "VenueManagement")]
        public async Task<ActionResult<UpdateVenueResponse>> UpdateVenue(int id, [FromBody] UpdateVenueRequest updateVenueRequest)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID not found in claims.");
                }
                if (!await _venueService.UserCanManageVenueAsync(id, userId))
                {
                    return Forbid("You do not have permission to update this venue.");
                }

                var response = await _venueService.UpdateVenueAsync(id, updateVenueRequest, userId);

                if (response == null)
                {
                    return NotFound($"Venue with ID {id} not found.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating venue with ID {VenueId}", id);
                return StatusCode(500, "An error occurred while updating the venue.");
            }
        }

        [HttpGet("types")]
        public async Task<ActionResult<VenueTypeListResponse>> GetVenueTypes()
        {
            try
            {
                var response = await _venueService.GetVenueTypesAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving venue types");
                return StatusCode(500, "An error occurred while retrieving venue types.");
            }
        }

        [HttpGet("managed")]
        [Authorize]
        public async Task<ActionResult<VenueListResponse>> GetManagedVenues()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID not found in claims.");
                }
                var response = await _venueService.GetManagedVenuesAsync(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving managed venues");
                return StatusCode(500, "An error occurred while retrieving managed venues.");
            }
        }
    }
}
