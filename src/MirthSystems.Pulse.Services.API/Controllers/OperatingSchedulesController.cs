namespace MirthSystems.Pulse.Services.API.Controllers
{
    using Auth0.ManagementApi.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models.Responses;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<OperatingScheduleDetail>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<OperatingScheduleDetail>>> GetOperatingScheduleById(string id)
        {
            if (!long.TryParse(id, out long scheduleId))
            {
                return BadRequest(ApiResponse<object>.CreateError("Invalid schedule ID format"));
            }

            var schedule = await _operatingScheduleService.GetOperatingScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound(ApiResponse<object>.CreateError($"Operating schedule with ID {id} not found"));
            }

            return Ok(ApiResponse<OperatingScheduleDetail>.CreateSuccess(schedule));
        }

        [HttpPost]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResponse<OperatingScheduleDetail>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<OperatingScheduleDetail>>> CreateOperatingSchedule([FromBody] CreateOperatingScheduleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.CreateError("Invalid request data"));
            }

            if (UserId == null)
            {
                return Unauthorized(ApiResponse<object>.CreateError("Unauthorized"));
            }

            var schedule = await _operatingScheduleService.CreateOperatingScheduleAsync(request, UserId);
            var response = ApiResponse<OperatingScheduleDetail>.CreateSuccess(schedule, "Operating schedule created successfully");
            return CreatedAtAction(nameof(GetOperatingScheduleById), new { id = schedule.Id }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<OperatingScheduleDetail>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<OperatingScheduleDetail>>> UpdateOperatingSchedule(string id, [FromBody] UpdateOperatingScheduleRequest request)
        {
            if (!long.TryParse(id, out long scheduleId))
            {
                return BadRequest(ApiResponse<object>.CreateError("Invalid schedule ID format"));
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
                var schedule = await _operatingScheduleService.UpdateOperatingScheduleAsync(id, request, UserId);
                return Ok(ApiResponse<OperatingScheduleDetail>.CreateSuccess(schedule, "Operating schedule updated successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.CreateError($"Operating schedule with ID {id} not found"));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteOperatingSchedule(string id)
        {
            if (!long.TryParse(id, out long scheduleId))
            {
                return BadRequest(ApiResponse<object>.CreateError("Invalid schedule ID format"));
            }

            if (UserId == null)
            {
                return Unauthorized(ApiResponse<object>.CreateError("Unauthorized"));
            }

            bool result = await _operatingScheduleService.DeleteOperatingScheduleAsync(id, UserId);
            if (!result)
            {
                return NotFound(ApiResponse<object>.CreateError($"Operating schedule with ID {id} not found"));
            }

            return Ok(ApiResponse<bool>.CreateSuccess(true, "Operating schedule deleted successfully"));
        }

        [HttpGet("venue/{venueId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<OperatingScheduleDetail>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<List<OperatingScheduleDetail>>>> GetVenueOperatingSchedules(string venueId)
        {
            if (!long.TryParse(venueId, out long parsedVenueId))
            {
                return BadRequest(ApiResponse<object>.CreateError("Invalid venue ID format"));
            }

            var schedules = await _operatingScheduleService.GetVenueOperatingSchedulesAsync(venueId);
            if (schedules == null || !schedules.Any())
            {
                return NotFound(ApiResponse<object>.CreateError($"Operating schedules for venue with ID {venueId} not found"));
            }

            return Ok(ApiResponse<List<OperatingScheduleDetail>>.CreateSuccess(schedules));
        }

        [HttpPost("venue/{venueId}")]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<bool>>> CreateOperatingSchedulesForVenue(string venueId, [FromBody] List<CreateOperatingScheduleRequest> request)
        {
            if (!long.TryParse(venueId, out long parsedVenueId))
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

            bool result = await _operatingScheduleService.CreateOperatingSchedulesForVenueAsync(venueId, request, UserId);
            var response = ApiResponse<bool>.CreateSuccess(result, "Operating schedules created successfully");
            return CreatedAtAction(nameof(GetVenueOperatingSchedules), new { venueId }, response);
        }
    }
}
