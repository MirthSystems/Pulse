namespace MirthSystems.Pulse.Services.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSpecials([FromQuery] GetSpecialsRequest request)
        {
            var specials = await _specialService.GetSpecialsAsync(request);
            if (specials == null || specials.Items.Count == 0)
            {
                return NotFound("No specials found");
            }
            return Ok(specials);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSpecialById(string id)
        {
            if (!long.TryParse(id, out long specialId))
            {
                return BadRequest("Invalid special ID format");
            }

            var special = await _specialService.GetSpecialByIdAsync(id);
            if (special == null)
            {
                return NotFound($"Special with ID {id} not found");
            }

            return Ok(special);
        }

        [HttpPost]
        [Authorize(Roles = "Content.Manager,System.Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateSpecial([FromBody] CreateSpecialRequest request)
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
                var special = await _specialService.CreateSpecialAsync(request, UserId);
                return CreatedAtAction(nameof(GetSpecialById), new { id = special.Id }, special);
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
        public async Task<IActionResult> UpdateSpecial(string id, [FromBody] UpdateSpecialRequest request)
        {
            if (!long.TryParse(id, out long specialId))
            {
                return BadRequest("Invalid special ID format");
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
                var special = await _specialService.UpdateSpecialAsync(id, request, UserId);
                return Ok(special);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Special with ID {id} not found");
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
        public async Task<IActionResult> DeleteSpecial(string id)
        {
            if (!long.TryParse(id, out long specialId))
            {
                return BadRequest("Invalid special ID format");
            }

            if (UserId == null)
            {
                return Unauthorized("Unauthorized");
            }

            bool result = await _specialService.DeleteSpecialAsync(id, UserId);
            if (!result)
            {
                return NotFound($"Special with ID {id} not found");
            }

            return Ok(true);
        }
    }
}
