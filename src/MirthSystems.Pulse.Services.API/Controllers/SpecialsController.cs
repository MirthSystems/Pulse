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

        public SpecialsController(ISpecialService specialService)
        {
            _specialService = specialService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedApiResponse<SpecialListItem>))]
        public async Task<ActionResult<PagedApiResponse<SpecialListItem>>> GetSpecials([FromQuery] GetSpecialsRequest request)
        {
            var specials = await _specialService.GetSpecialsAsync(request);
            return Ok(specials);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<SpecialDetail>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<SpecialDetail>>> GetSpecialById(string id)
        {
            if (!long.TryParse(id, out long specialId))
            {
                return BadRequest(ApiResponse<object>.CreateError("Invalid special ID format"));
            }

            var special = await _specialService.GetSpecialByIdAsync(id);
            if (special == null)
            {
                return NotFound(ApiResponse<object>.CreateError($"Special with ID {id} not found"));
            }

            return Ok(ApiResponse<SpecialDetail>.CreateSuccess(special));
        }

        [HttpPost]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResponse<SpecialDetail>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
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

        [HttpPut("{id}")]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<SpecialDetail>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<SpecialDetail>>> UpdateSpecial(string id, [FromBody] UpdateSpecialRequest request)
        {
            if (!long.TryParse(id, out long specialId))
            {
                return BadRequest(ApiResponse<object>.CreateError("Invalid special ID format"));
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
                var special = await _specialService.UpdateSpecialAsync(id, request, UserId);
                return Ok(ApiResponse<SpecialDetail>.CreateSuccess(special, "Special updated successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ApiResponse<object>.CreateError($"Special with ID {id} not found"));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse<object>))]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteSpecial(string id)
        {
            if (!long.TryParse(id, out long specialId))
            {
                return BadRequest(ApiResponse<object>.CreateError("Invalid special ID format"));
            }

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
