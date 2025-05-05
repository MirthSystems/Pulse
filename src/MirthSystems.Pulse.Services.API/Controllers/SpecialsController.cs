namespace MirthSystems.Pulse.Services.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models.Responses;
    using MirthSystems.Pulse.Core.Models;
    using MirthSystems.Pulse.Services.API.Controllers.Base;

    [Route("api/specials")]
    public class SpecialsController : ApiController
    {
        private readonly ISpecialService _specialService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialsController"/> class.
        /// </summary>
        /// <param name="specialService">The special service</param>
        public SpecialsController(ISpecialService specialService)
        {
            _specialService = specialService;
        }

        /// <summary>
        /// Gets all specials with optional filtering and pagination
        /// </summary>
        /// <remarks>
        /// This endpoint allows filtering specials by location, radius, date/time, and venue.
        /// If no date/time is provided, the current time will be used.
        /// </remarks>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedApiResponse<SpecialListItem>))]
        public async Task<ActionResult<PagedApiResponse<SpecialListItem>>> GetSpecials([FromQuery] GetSpecialsRequest request)
        {
            var specials = await _specialService.GetSpecialsAsync(request);
            return Ok(specials);
        }

        /// <summary>
        /// Gets a special by ID
        /// </summary>
        /// <param name="id">The special ID</param>
        /// <returns>The special details</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<SpecialDetail>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<SpecialDetail>>> GetSpecialById(long id)
        {
            var special = await _specialService.GetSpecialByIdAsync(id);

            if (special == null)
            {
                return NotFound(ApiResponse<object>.CreateError($"Special with ID {id} not found"));
            }

            return Ok(ApiResponse<SpecialDetail>.CreateSuccess(special));
        }

        /// <summary>
        /// Creates a new special
        /// </summary>
        /// <param name="request">The special creation request</param>
        /// <returns>The created special</returns>
        [HttpPost]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResponse<SpecialDetail>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<SpecialDetail>>> CreateSpecial([FromBody] CreateSpecialRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.CreateError("Invalid request data"));
            }

            if (UserId == null)
            {
                return Unauthorized(ApiResponse<object>.CreateError("Unauthorized"));
            }

            var special = await _specialService.CreateSpecialAsync(request, UserId);

            var response = ApiResponse<SpecialDetail>.CreateSuccess(special, "Special created successfully");
            return CreatedAtAction(nameof(GetSpecialById), new { id = special.Id }, response);
        }

        /// <summary>
        /// Updates an existing special
        /// </summary>
        /// <param name="id">The special ID</param>
        /// <param name="request">The special update request</param>
        /// <returns>The updated special</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<SpecialDetail>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<SpecialDetail>>> UpdateSpecial(long id, [FromBody] UpdateSpecialRequest request)
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
                var special = await _specialService.UpdateSpecialAsync(id, request, UserId);
                return Ok(ApiResponse<SpecialDetail>.CreateSuccess(special, "Special updated successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.CreateError($"Special with ID {id} not found"));
            }
        }

        /// <summary>
        /// Deletes a special
        /// </summary>
        /// <param name="id">The special ID</param>
        /// <returns>Success indicator</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteSpecial(long id)
        {
            if (UserId == null)
            {
                return Unauthorized(ApiResponse<object>.CreateError("Unauthorized"));
            }

            bool result = await _specialService.DeleteSpecialAsync(id, UserId);

            if (!result)
            {
                return NotFound(ApiResponse<object>.CreateError($"Special with ID {id} not found"));
            }

            return Ok(ApiResponse<bool>.CreateSuccess(true, "Special deleted successfully"));
        }
    }
}
