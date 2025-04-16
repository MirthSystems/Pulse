namespace Pulse.Api.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using System.Net;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Pulse.Core.Contracts;

    using Pulse.Core.Models.Entities;

    using Pulse.Core.Models;

    [Authorize]
    [ApiController]
    [Route("api/admin")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVenueLocationService _venueLocationService;
        private readonly IMapper _mapper;

        public AdminController(
            IUnitOfWork unitOfWork,
            IVenueLocationService venueLocationService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _venueLocationService = venueLocationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the ID of the current authenticated user
        /// </summary>
        private string GetUserId()
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User ID not found in claims.");
            }
            return userId;
        }

        #region Venue Operations

        /// <summary>
        /// Gets all venues
        /// </summary>
        [HttpGet("venues")]
        [ProducesResponseType(typeof(IEnumerable<VenueItem>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetVenues()
        {
            var venues = await _unitOfWork.Venues.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<VenueItem>>(venues));
        }

        /// <summary>
        /// Gets a venue by ID
        /// </summary>
        [HttpGet("venues/{id}")]
        [ProducesResponseType(typeof(VenueWithDetails), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetVenue(long id)
        {
            var venue = await _unitOfWork.Venues.GetWithAllDataAsync(id);
            if (venue == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VenueWithDetails>(venue));
        }

        /// <summary>
        /// Creates a new venue
        /// </summary>
        [HttpPost("venues")]
        [ProducesResponseType(typeof(VenueItem), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateVenue(NewVenueRequest request)
        {
            var venue = _mapper.Map<Venue>(request);

            var userId = GetUserId();
            await _venueLocationService.GeocodeVenueAsync(venue);
            var createdVenue = await _unitOfWork.Venues.AddAsync(venue, userId);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVenue), new { id = createdVenue.Id }, _mapper.Map<VenueItem>(createdVenue));
        }

        /// <summary>
        /// Updates an existing venue
        /// </summary>
        [HttpPut("venues/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateVenue(long id, UpdateVenueRequest request)
        {
            var venue = await _unitOfWork.Venues.GetByIdAsync(id);
            if (venue == null)
            {
                return NotFound();
            }

            _mapper.Map(request, venue);

            var userId = GetUserId();
            await _venueLocationService.GeocodeVenueAsync(venue);
            await _unitOfWork.Venues.UpdateAsync(venue, userId);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes a venue
        /// </summary>
        [HttpDelete("venues/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteVenue(long id)
        {
            var venue = await _unitOfWork.Venues.GetByIdAsync(id);
            if (venue == null)
            {
                return NotFound();
            }

            await _unitOfWork.Venues.DeleteAsync(venue);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region VenueType Operations

        /// <summary>
        /// Gets all venue types
        /// </summary>
        [HttpGet("venue-types")]
        [ProducesResponseType(typeof(IEnumerable<VenueTypeItem>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetVenueTypes()
        {
            var venueTypes = await _unitOfWork.VenueTypes.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<VenueTypeItem>>(venueTypes));
        }

        /// <summary>
        /// Gets a venue type by ID
        /// </summary>
        [HttpGet("venue-types/{id}")]
        [ProducesResponseType(typeof(VenueTypeItem), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetVenueType(int id)
        {
            var venueType = await _unitOfWork.VenueTypes.GetByIdAsync(id);
            if (venueType == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VenueTypeItem>(venueType));
        }

        /// <summary>
        /// Creates a new venue type
        /// </summary>
        [HttpPost("venue-types")]
        [ProducesResponseType(typeof(VenueTypeItem), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateVenueType(NewVenueTypeRequest request)
        {
            var venueType = _mapper.Map<VenueType>(request);

            var userId = GetUserId();
            var createdVenueType = await _unitOfWork.VenueTypes.AddAsync(venueType, userId);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVenueType), new { id = createdVenueType.Id }, _mapper.Map<VenueTypeItem>(createdVenueType));
        }

        /// <summary>
        /// Updates an existing venue type
        /// </summary>
        [HttpPut("venue-types/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateVenueType(int id, UpdateVenueTypeRequest request)
        {
            var venueType = await _unitOfWork.VenueTypes.GetByIdAsync(id);
            if (venueType == null)
            {
                return NotFound();
            }

            _mapper.Map(request, venueType);

            var userId = GetUserId();
            await _unitOfWork.VenueTypes.UpdateAsync(venueType, userId);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes a venue type
        /// </summary>
        [HttpDelete("venue-types/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteVenueType(int id)
        {
            var venueType = await _unitOfWork.VenueTypes.GetByIdAsync(id);
            if (venueType == null)
            {
                return NotFound();
            }

            await _unitOfWork.VenueTypes.DeleteAsync(venueType);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region Special Operations

        /// <summary>
        /// Gets all specials
        /// </summary>
        [HttpGet("specials")]
        [ProducesResponseType(typeof(IEnumerable<SpecialItem>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSpecials()
        {
            var specials = await _unitOfWork.Specials.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<SpecialItem>>(specials));
        }

        /// <summary>
        /// Gets a special by ID
        /// </summary>
        [HttpGet("specials/{id}")]
        [ProducesResponseType(typeof(SpecialWithDetails), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetSpecial(long id)
        {
            var special = await _unitOfWork.Specials.GetWithAllDataAsync(id);
            if (special == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<SpecialWithDetails>(special));
        }

        /// <summary>
        /// Creates a new special
        /// </summary>
        [HttpPost("specials")]
        [ProducesResponseType(typeof(SpecialItem), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateSpecial(NewSpecialRequest request)
        {
            var special = _mapper.Map<Special>(request);

            var userId = GetUserId();
            var createdSpecial = await _unitOfWork.Specials.AddAsync(special, userId);
            await _unitOfWork.SaveChangesAsync();

            // Add tags if specified
            if (request.TagIds != null && request.TagIds.Any())
            {
                foreach (var tagId in request.TagIds)
                {
                    await _unitOfWork.TagToSpecialLinks.AddLinkAsync(tagId, createdSpecial.Id, userId);
                }
                await _unitOfWork.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetSpecial), new { id = createdSpecial.Id }, _mapper.Map<SpecialItem>(createdSpecial));
        }

        /// <summary>
        /// Updates an existing special
        /// </summary>
        [HttpPut("specials/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateSpecial(long id, UpdateSpecialRequest request)
        {
            var special = await _unitOfWork.Specials.GetByIdAsync(id);
            if (special == null)
            {
                return NotFound();
            }

            _mapper.Map(request, special);

            var userId = GetUserId();
            await _unitOfWork.Specials.UpdateAsync(special, userId);
            await _unitOfWork.SaveChangesAsync();

            if (request.TagIds != null)
            {
                var currentLinks = await _unitOfWork.TagToSpecialLinks.GetBySpecialIdAsync(id);
                var currentTagIds = currentLinks.Select(l => l.TagId).ToList();

                var tagsToRemove = currentTagIds.Except(request.TagIds);
                foreach (var tagId in tagsToRemove)
                {
                    await _unitOfWork.TagToSpecialLinks.RemoveLinkAsync(tagId, id);
                }

                var tagsToAdd = request.TagIds.Except(currentTagIds);
                foreach (var tagId in tagsToAdd)
                {
                    await _unitOfWork.TagToSpecialLinks.AddLinkAsync(tagId, id, userId);
                }

                await _unitOfWork.SaveChangesAsync();
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a special
        /// </summary>
        [HttpDelete("specials/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteSpecial(long id)
        {
            var special = await _unitOfWork.Specials.GetByIdAsync(id);
            if (special == null)
            {
                return NotFound();
            }

            await _unitOfWork.Specials.DeleteAsync(special);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region Operating Schedule Operations

        /// <summary>
        /// Gets all operating schedules for a venue
        /// </summary>
        [HttpGet("venues/{venueId}/schedules")]
        [ProducesResponseType(typeof(IEnumerable<OperatingScheduleItem>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetVenueSchedules(long venueId)
        {
            var venue = await _unitOfWork.Venues.GetByIdAsync(venueId);
            if (venue == null)
            {
                return NotFound();
            }

            var schedules = await _unitOfWork.BusinessHours.GetByVenueIdAsync(venueId);
            return Ok(_mapper.Map<IEnumerable<OperatingScheduleItem>>(schedules));
        }

        /// <summary>
        /// Gets a specific operating schedule by ID
        /// </summary>
        [HttpGet("schedules/{id}")]
        [ProducesResponseType(typeof(OperatingScheduleItem), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetSchedule(long id)
        {
            var schedule = await _unitOfWork.BusinessHours.GetByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<OperatingScheduleItem>(schedule));
        }

        /// <summary>
        /// Creates a new operating schedule for a venue
        /// </summary>
        [HttpPost("venues/{venueId}/schedules")]
        [ProducesResponseType(typeof(OperatingScheduleItem), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CreateSchedule(long venueId, NewOperatingScheduleRequest request)
        {
            var venue = await _unitOfWork.Venues.GetByIdAsync(venueId);
            if (venue == null)
            {
                return NotFound();
            }

            var schedule = _mapper.Map<OperatingSchedule>(request);
            schedule.VenueId = venueId;

            var userId = GetUserId();
            var createdSchedule = await _unitOfWork.BusinessHours.AddAsync(schedule, userId);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSchedule), new { id = createdSchedule.Id }, _mapper.Map<OperatingScheduleItem>(createdSchedule));
        }

        /// <summary>
        /// Updates an existing operating schedule
        /// </summary>
        [HttpPut("schedules/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateSchedule(long id, UpdateOperatingScheduleRequest request)
        {
            var schedule = await _unitOfWork.BusinessHours.GetByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            _mapper.Map(request, schedule);

            var userId = GetUserId();
            await _unitOfWork.BusinessHours.UpdateAsync(schedule, userId);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Updates all operating schedules for a venue
        /// </summary>
        [HttpPut("venues/{venueId}/schedules")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateVenueSchedules(long venueId, IEnumerable<UpdateOperatingScheduleRequest> requests)
        {
            var venue = await _unitOfWork.Venues.GetByIdAsync(venueId);
            if (venue == null)
            {
                return NotFound();
            }

            var userId = GetUserId();
            var schedules = _mapper.Map<IEnumerable<OperatingSchedule>>(requests);

            foreach (var schedule in schedules)
            {
                schedule.VenueId = venueId;
            }

            await _unitOfWork.BusinessHours.UpdateVenueSchedulesAsync(venueId, schedules, userId);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes an operating schedule
        /// </summary>
        [HttpDelete("schedules/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteSchedule(long id)
        {
            var schedule = await _unitOfWork.BusinessHours.GetByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            await _unitOfWork.BusinessHours.DeleteAsync(schedule);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region Tag Operations

        /// <summary>
        /// Gets all tags
        /// </summary>
        [HttpGet("tags")]
        [ProducesResponseType(typeof(IEnumerable<TagItem>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTags()
        {
            var tags = await _unitOfWork.Tags.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<TagItem>>(tags));
        }

        /// <summary>
        /// Gets a tag by ID
        /// </summary>
        [HttpGet("tags/{id}")]
        [ProducesResponseType(typeof(TagItem), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTag(long id)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TagItem>(tag));
        }

        /// <summary>
        /// Creates a new tag
        /// </summary>
        [HttpPost("tags")]
        [ProducesResponseType(typeof(TagItem), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateTag(NewTagRequest request)
        {
            var tag = _mapper.Map<Tag>(request);

            var userId = GetUserId();
            var createdTag = await _unitOfWork.Tags.AddAsync(tag, userId);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTag), new { id = createdTag.Id }, _mapper.Map<TagItem>(createdTag));
        }

        /// <summary>
        /// Updates an existing tag
        /// </summary>
        [HttpPut("tags/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateTag(long id, UpdateTagRequest request)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            _mapper.Map(request, tag);

            var userId = GetUserId();
            await _unitOfWork.Tags.UpdateAsync(tag, userId);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes a tag
        /// </summary>
        [HttpDelete("tags/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteTag(long id)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            await _unitOfWork.Tags.DeleteAsync(tag);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        #endregion
    }
}
