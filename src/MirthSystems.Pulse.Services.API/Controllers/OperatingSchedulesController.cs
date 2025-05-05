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

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatingSchedulesController"/> class.
        /// </summary>
        /// <param name="operatingScheduleService">The operating schedule service</param>
        public OperatingSchedulesController(IOperatingScheduleService operatingScheduleService)
        {
            _operatingScheduleService = operatingScheduleService;
        }

        /// <summary>
        /// Gets an operating schedule by ID
        /// </summary>
        /// <param name="id">The operating schedule ID</param>
        /// <returns>The operating schedule details</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<OperatingScheduleDetail>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<OperatingScheduleDetail>>> GetOperatingScheduleById(long id)
        {
            var schedule = await _operatingScheduleService.GetOperatingScheduleByIdAsync(id);

            if (schedule == null)
            {
                return NotFound(ApiResponse<object>.CreateError($"Operating schedule with ID {id} not found"));
            }

            return Ok(ApiResponse<OperatingScheduleDetail>.CreateSuccess(schedule));
        }

        /// <summary>
        /// Creates a new operating schedule
        /// </summary>
        /// <param name="request">The operating schedule creation request</param>
        /// <returns>The created operating schedule</returns>
        [HttpPost]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResponse<OperatingScheduleDetail>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Updates an existing operating schedule
        /// </summary>
        /// <param name="id">The operating schedule ID</param>
        /// <param name="request">The operating schedule update request</param>
        /// <returns>The updated operating schedule</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<OperatingScheduleDetail>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<OperatingScheduleDetail>>> UpdateOperatingSchedule(long id, [FromBody] UpdateOperatingScheduleRequest request)
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
                var schedule = await _operatingScheduleService.UpdateOperatingScheduleAsync(id, request, UserId);
                return Ok(ApiResponse<OperatingScheduleDetail>.CreateSuccess(schedule, "Operating schedule updated successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.CreateError($"Operating schedule with ID {id} not found"));
            }
        }

        /// <summary>
        /// Deletes an operating schedule
        /// </summary>
        /// <param name="id">The operating schedule ID</param>
        /// <returns>Success indicator</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteOperatingSchedule(long id)
        {
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

        /// <summary>
        /// Gets operating schedules for a venue
        /// </summary>
        /// <param name="venueId">The venue ID</param>
        /// <returns>The venue's operating schedules</returns>
        [HttpGet("venue/{venueId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<OperatingScheduleDetail>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<List<OperatingScheduleDetail>>>> GetVenueOperatingSchedules(long venueId)
        {
            var schedules = await _operatingScheduleService.GetVenueOperatingSchedulesAsync(venueId);

            if (schedules == null || !schedules.Any())
            {
                return NotFound(ApiResponse<object>.CreateError($"Operating schedules for venue with ID {venueId} not found"));
            }

            return Ok(ApiResponse<List<OperatingScheduleDetail>>.CreateSuccess(schedules));
        }

        /// <summary>
        /// Creates multiple operating schedules for a venue
        /// </summary>
        /// <param name="venueId">The venue ID</param>
        /// <param name="request">The operating schedule creation requests</param>
        /// <returns>Success indicator</returns>
        [HttpPost("venue/{venueId}")]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<bool>>> CreateOperatingSchedulesForVenue(long venueId, [FromBody] List<CreateOperatingScheduleRequest> request)
        {
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
