namespace MirthSystems.Pulse.Services.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models;
    using MirthSystems.Pulse.Services.API.Controllers.Base;

    [Route("api/operating-schedules")]
    public class OperatingSchedulesController : ApiController
    {
        private readonly IOperatingScheduleService _operatingScheduleService;

        public OperatingSchedulesController(IOperatingScheduleService operatingScheduleService)
        {
            _operatingScheduleService = operatingScheduleService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOperatingScheduleById(string id)
        {
            if (!long.TryParse(id, out long scheduleId))
            {
                return BadRequest("Invalid schedule ID format");
            }

            var schedule = await _operatingScheduleService.GetOperatingScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound($"Operating schedule with ID {id} not found");
            }

            return Ok(schedule);
        }

        [HttpPost]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateOperatingSchedule([FromBody] CreateOperatingScheduleRequest request)
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
                var schedule = await _operatingScheduleService.CreateOperatingScheduleAsync(request, UserId);
                return CreatedAtAction(nameof(GetOperatingScheduleById), new { id = schedule.Id }, schedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOperatingSchedule(string id, [FromBody] UpdateOperatingScheduleRequest request)
        {
            if (!long.TryParse(id, out long scheduleId))
            {
                return BadRequest("Invalid schedule ID format");
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
                var schedule = await _operatingScheduleService.UpdateOperatingScheduleAsync(id, request, UserId);
                return Ok(schedule);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Operating schedule with ID {id} not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOperatingSchedule(string id)
        {
            if (!long.TryParse(id, out long scheduleId))
            {
                return BadRequest("Invalid schedule ID format");
            }

            if (UserId == null)
            {
                return Unauthorized("Unauthorized");
            }

            bool result = await _operatingScheduleService.DeleteOperatingScheduleAsync(id, UserId);
            if (!result)
            {
                return NotFound($"Operating schedule with ID {id} not found");
            }

            return Ok(true);
        }

        [HttpGet("venue/{venueId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVenueOperatingSchedules(string venueId)
        {
            if (!long.TryParse(venueId, out long parsedVenueId))
            {
                return BadRequest("Invalid venue ID format");
            }

            var schedules = await _operatingScheduleService.GetVenueOperatingSchedulesAsync(venueId);
            if (schedules == null || !schedules.Any())
            {
                return NotFound($"Operating schedules for venue with ID {venueId} not found");
            }

            return Ok(schedules);
        }

        [HttpPost("venue/{venueId}")]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateOperatingSchedulesForVenue(string venueId, [FromBody] List<CreateOperatingScheduleRequest> request)
        {
            if (!long.TryParse(venueId, out long parsedVenueId))
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

            bool result = await _operatingScheduleService.CreateOperatingSchedulesForVenueAsync(venueId, request, UserId);
            if (!result)
            {
                return BadRequest("Failed to create operating schedules");
            }
            
            return CreatedAtAction(nameof(GetVenueOperatingSchedules), new { venueId }, true);
        }
    }
}
