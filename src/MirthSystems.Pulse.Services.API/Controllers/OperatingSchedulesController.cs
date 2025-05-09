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
    /// API controller for managing venue operating schedules.
    /// </summary>
    /// <remarks>
    /// <para>This controller provides endpoints for retrieving and managing venue business hours:</para>
    /// <para>- Retrieving individual schedule details and venue weekly schedules</para>
    /// <para>- Creating and updating operating schedules</para>
    /// <para>- Batch management of venue operating hours</para>
    /// <para>- Deleting schedules (for authenticated users with appropriate roles)</para>
    /// </remarks>
    [Route("api/operating-schedules")]
    public class OperatingSchedulesController : ApiController
    {
        private readonly IOperatingScheduleService _operatingScheduleService;

        public OperatingSchedulesController(IOperatingScheduleService operatingScheduleService)
        {
            _operatingScheduleService = operatingScheduleService;
        }

        /// <summary>
        /// Retrieves a specific operating schedule by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the operating schedule.</param>
        /// <returns>Detailed information about the requested operating schedule.</returns>
        /// <response code="200">Returns the operating schedule details.</response>
        /// <response code="400">If the ID format is invalid.</response>
        /// <response code="404">If the operating schedule with the specified ID is not found.</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperatingScheduleDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("GetOperatingScheduleById", "Retrieves detailed information about a specific operating schedule")]
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

        /// <summary>
        /// Creates a new operating schedule for a venue.
        /// </summary>
        /// <param name="request">The data for creating the operating schedule.</param>
        /// <returns>The created operating schedule with its assigned ID.</returns>
        /// <response code="201">Returns the newly created operating schedule.</response>
        /// <response code="400">If the request data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user doesn't have the required role.</response>
        [HttpPost]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OperatingScheduleDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [OpenApiOperation("CreateOperatingSchedule", "Creates a new operating schedule for a venue")]
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

        /// <summary>
        /// Updates an existing operating schedule.
        /// </summary>
        /// <param name="id">The unique identifier of the operating schedule to update.</param>
        /// <param name="request">The update data for the operating schedule.</param>
        /// <returns>The updated operating schedule information.</returns>
        /// <response code="200">Returns the updated operating schedule.</response>
        /// <response code="400">If the ID format or request data is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user doesn't have the required role.</response>
        /// <response code="404">If the operating schedule with the specified ID is not found.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperatingScheduleDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("UpdateOperatingSchedule", "Updates an existing operating schedule")]
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

        /// <summary>
        /// Deletes an operating schedule.
        /// </summary>
        /// <param name="id">The unique identifier of the operating schedule to delete.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        /// <response code="200">Returns true if deletion was successful.</response>
        /// <response code="400">If the ID format is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user doesn't have the required role.</response>
        /// <response code="404">If the operating schedule with the specified ID is not found.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("DeleteOperatingSchedule", "Deletes an operating schedule")]
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
    }
}
