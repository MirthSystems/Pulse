namespace MirthSystems.Pulse.Services.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models;
    using NSwag.Annotations;
    using System.Security.Claims;

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
    public class OperatingSchedulesController : ControllerBase
    {
        private readonly IOperatingScheduleService _operatingScheduleService;
        private string? UserId => User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        public OperatingSchedulesController(IOperatingScheduleService operatingScheduleService)
        {
            _operatingScheduleService = operatingScheduleService;
        }
        
        /// <summary>
        /// Gets an operating schedule by ID.
        /// </summary>
        /// <param name="id">The operating schedule ID.</param>
        /// <returns>The operating schedule details.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperatingScheduleItemExtended))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("GetOperatingScheduleById", "Retrieves an operating schedule by its ID")]
        public async Task<ActionResult<OperatingScheduleItemExtended>> GetOperatingScheduleById(string id)
        {
            try
            {
                var operatingSchedule = await _operatingScheduleService.GetOperatingScheduleByIdAsync(id);
                if (operatingSchedule == null)
                {
                    return NotFound($"Operating schedule with ID {id} not found");
                }
                
                return Ok(operatingSchedule);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Creates a new operating schedule.
        /// </summary>
        /// <param name="request">The data for the new operating schedule.</param>
        /// <returns>The created operating schedule.</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OperatingScheduleItemExtended))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [OpenApiOperation("CreateOperatingSchedule", "Creates a new operating schedule")]
        public async Task<ActionResult<OperatingScheduleItemExtended>> CreateOperatingSchedule([FromBody] CreateOperatingScheduleRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                if (UserId == null)
                {
                    return Unauthorized("User must be authenticated to create operating schedules");
                }
                
                var operatingSchedule = await _operatingScheduleService.CreateOperatingScheduleAsync(request, UserId);
                return CreatedAtAction(nameof(GetOperatingScheduleById), new { id = operatingSchedule.Id }, operatingSchedule);
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
        /// Updates an existing operating schedule.
        /// </summary>
        /// <param name="id">The ID of the operating schedule to update.</param>
        /// <param name="request">The update data.</param>
        /// <returns>The updated operating schedule.</returns>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperatingScheduleItemExtended))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("UpdateOperatingSchedule", "Updates an existing operating schedule")]
        public async Task<ActionResult<OperatingScheduleItemExtended>> UpdateOperatingSchedule(string id, [FromBody] UpdateOperatingScheduleRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                if (UserId == null)
                {
                    return Unauthorized("User must be authenticated to update operating schedules");
                }
                
                var operatingSchedule = await _operatingScheduleService.UpdateOperatingScheduleAsync(id, request, UserId);
                return Ok(operatingSchedule);
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
        /// Deletes an operating schedule.
        /// </summary>
        /// <param name="id">The ID of the operating schedule to delete.</param>
        /// <returns>A success indicator.</returns>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OpenApiOperation("DeleteOperatingSchedule", "Deletes an operating schedule")]
        public async Task<ActionResult<bool>> DeleteOperatingSchedule(string id)
        {
            try
            {
                if (UserId == null)
                {
                    return Unauthorized("User must be authenticated to delete operating schedules");
                }
                
                var result = await _operatingScheduleService.DeleteOperatingScheduleAsync(id, UserId);
                if (!result)
                {
                    return NotFound($"Operating schedule with ID {id} not found");
                }
                
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
