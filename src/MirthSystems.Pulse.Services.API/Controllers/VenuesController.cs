namespace MirthSystems.Pulse.Services.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetVenues([FromQuery] PageQueryParams pageQuery)
        {
            pageQuery.PageSize = pageQuery.PageSize > 10000 ? 10000 : pageQuery.PageSize;
            var venues = await _venueService.GetVenuesAsync(pageQuery.Page, pageQuery.PageSize);
            if (venues == null || venues.Items.Count == 0)
            {
                return NotFound("No venues found");
            }
            return Ok(venues);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpGet("{id}/business-hours")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpGet("{id}/specials")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpPost]
        [Authorize(Roles = "System.Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        [HttpPut("{id}")]
        [Authorize(Roles = "System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVenue(string id, [FromBody] UpdateVenueRequest request)
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
