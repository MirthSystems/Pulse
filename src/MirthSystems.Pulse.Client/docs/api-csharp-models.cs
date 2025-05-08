using System.Reflection;

using Microsoft.OpenApi.Models;

using MirthSystems.Pulse.Infrastructure.Extensions;

using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.AddSerilog(
                logger: Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(builder.Configuration.GetSection("Logging:Serilog"))
                            .CreateBootstrapLogger(),
                dispose: true
            );

        builder.Services.AddServiceDefaults();
        builder.Services.AddApplicationDbContext(builder.Configuration.GetConnectionString("PostgresDbConnection"));
        builder.Services.AddAzureMaps(builder.Configuration.GetSection("AzureMaps")["SubscriptionKey"]);
        builder.Services.AddAuthentication().AddJwtBearer();
        builder.Services.AddAuthorization();

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Pulse API",
                Description = "Learn how to protect your .NET applications with Auth0",
                Contact = new OpenApiContact
                {
                    Name = ".NET Identity with Auth0",
                    Url = new Uri("https://a0.to/dotnet-templates/webapi")
                },
                Version = "v1.0.0"
            });

            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "Using the Authorization header with the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            options.AddSecurityDefinition("Bearer", securitySchema);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securitySchema, new[] { "Bearer" } }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseStaticFiles();
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapControllers();
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}
namespace MirthSystems.Pulse.Services.API.Controllers.Base
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Produces("application/json")]
    public abstract class ApiController : ControllerBase
    {
        /// <summary>
        /// Gets the current user's ID from claims
        /// </summary>
        protected string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
namespace MirthSystems.Pulse.Services.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models;
    using MirthSystems.Pulse.Services.API.Controllers.Base;
    using NSwag.Annotations;

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
namespace MirthSystems.Pulse.Infrastructure.Services
{
    using Microsoft.Extensions.Logging;
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models;
    using NodaTime;
    using NetTopologySuite.Geometries;
    using Azure.Maps.Search.Models;
    using MirthSystems.Pulse.Core.Extensions;

    public class VenueService : IVenueService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAzureMapsApiService _mapsApiService;
        private readonly ILogger<VenueService> _logger;

        public VenueService(
            IUnitOfWork unitOfWork,
            IAzureMapsApiService mapsApiService,
            ILogger<VenueService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapsApiService = mapsApiService;
            _logger = logger;
        }

        public async Task<PagedResult<VenueListItem>> GetVenuesAsync(int page = 1, int pageSize = 20)
        {
            try
            {
                var venues = await _unitOfWork.Venues.GetPagedVenuesAsync(page, pageSize);
                return new PagedResult<VenueListItem>
                {
                    Items = venues.Select(v => v.MapToVenueListItem()).ToList(),
                    PagingInfo = PagingInfo.Create(
                        venues.CurrentPage,
                        venues.PageSize,
                        venues.TotalCount)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving venues");
                throw;
            }
        }

        public async Task<VenueDetail?> GetVenueByIdAsync(string id)
        {
            try
            {
                if (!long.TryParse(id, out long venueId))
                {
                    _logger.LogWarning("Invalid venue ID format: {Id}", id);
                    return null;
                }
                var venue = await _unitOfWork.Venues.GetVenueWithDetailsAsync(venueId);
                if (venue == null)
                {
                    return null;
                }
                return venue.MapToVenueDetail();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving venue with ID {VenueId}", id);
                return null;
            }
        }

        public async Task<BusinessHours?> GetVenueBusinessHoursAsync(string id)
        {
            try
            {
                if (!long.TryParse(id, out long venueId))
                {
                    _logger.LogWarning("Invalid venue ID format: {Id}", id);
                    return null;
                }
                var venue = await _unitOfWork.Venues.GetByIdAsync(venueId);
                if (venue == null)
                {
                    return null;
                }
                var schedules = await _unitOfWork.OperatingSchedules.GetSchedulesByVenueIdAsync(venueId);
                if (schedules == null || !schedules.Any())
                {
                    return null;
                }
                return new BusinessHours
                {
                    VenueId = venue.Id.ToString(),
                    VenueName = venue.Name ?? string.Empty,
                    ScheduleItems = schedules.Select(s => s.MapToOperatingScheduleListItem()).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving business hours for venue with ID {VenueId}", id);
                return null;
            }
        }

        public async Task<VenueSpecials?> GetVenueSpecialsAsync(string id, bool includeCurrentStatus = true)
        {
            try
            {
                if (!long.TryParse(id, out long venueId))
                {
                    _logger.LogWarning("Invalid venue ID format: {Id}", id);
                    return null;
                }
                var venue = await _unitOfWork.Venues.GetByIdAsync(venueId);
                if (venue == null)
                {
                    return null;
                }
                var specials = await _unitOfWork.Specials.GetSpecialsByVenueIdAsync(venueId);
                var specialListItems = new List<SpecialListItem>();
                if (includeCurrentStatus)
                {
                    var now = SystemClock.Instance.GetCurrentInstant();
                    specialListItems = await Task.WhenAll(specials.Select(async s =>
                    {
                        var isCurrentlyRunning = await _unitOfWork.Specials.IsSpecialCurrentlyActiveAsync(s.Id, now);
                        return s.MapToSpecialListItem(isCurrentlyRunning);
                    })).ContinueWith(t => t.Result.ToList());
                }
                else
                {
                    specialListItems = specials.Select(s => s.MapToSpecialListItem(false)).ToList();
                }
                return new VenueSpecials
                {
                    VenueId = venue.Id.ToString(),
                    VenueName = venue.Name ?? string.Empty,
                    Specials = specialListItems
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving specials for venue with ID {VenueId}", id);
                return null;
            }
        }

        public async Task<VenueDetail> CreateVenueAsync(CreateVenueRequest request, string userId)
        {
            try
            {
                var fullAddress = $"{request.Address.StreetAddress}, {request.Address.Locality}, {request.Address.Region} {request.Address.Postcode}, {request.Address.Country}";
                var geocodeResponse = await _mapsApiService.SearchClient.GetGeocodingAsync(fullAddress);
                Point geocodedPoint;
                if (geocodeResponse.Value.Features.Count > 0)
                {
                    var result = geocodeResponse.Value.Features[0];
                    geocodedPoint = new Point(result.Geometry.Coordinates.Longitude, result.Geometry.Coordinates.Latitude) { SRID = 4326 };
                }
                else
                {
                    throw new InvalidOperationException("Unable to geocode address");
                }

                var venue = request.MapToNewVenue(userId, geocodedPoint);

                await _unitOfWork.Venues.AddAsync(venue);
                await _unitOfWork.SaveChangesAsync();

                var createdVenue = await _unitOfWork.Venues.GetVenueWithDetailsAsync(venue.Id);
                if (createdVenue == null)
                {
                    throw new InvalidOperationException("Failed to retrieve created venue");
                }
                return createdVenue.MapToVenueDetail();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating venue");
                throw;
            }
        }

        public async Task<VenueDetail> UpdateVenueAsync(string id, UpdateVenueRequest request, string userId)
        {
            try
            {
                if (!long.TryParse(id, out long venueId))
                {
                    throw new ArgumentException("Invalid venue ID format");
                }
                var venue = await _unitOfWork.Venues.GetVenueWithDetailsAsync(venueId);
                if (venue == null)
                {
                    throw new KeyNotFoundException($"Venue with ID {id} not found");
                }

                var fullAddress = $"{request.Address.StreetAddress}, {request.Address.Locality}, {request.Address.Region} {request.Address.Postcode}, {request.Address.Country}";
                var geocodeResponse = await _mapsApiService.SearchClient.GetGeocodingAsync(fullAddress);
                Point geocodedPoint;
                if (geocodeResponse.Value.Features.Count > 0)
                {
                    var result = geocodeResponse.Value.Features[0];
                    geocodedPoint = new Point(result.Geometry.Coordinates.Longitude, result.Geometry.Coordinates.Latitude) { SRID = 4326 };
                }
                else
                {
                    throw new InvalidOperationException("Unable to geocode address");
                }

                request.MapAndUpdateExistingVenue(venue, userId, geocodedPoint);

                _unitOfWork.Venues.Update(venue);
                await _unitOfWork.SaveChangesAsync();

                var updatedVenue = await _unitOfWork.Venues.GetVenueWithDetailsAsync(venueId);
                if (updatedVenue == null)
                {
                    throw new InvalidOperationException("Failed to retrieve updated venue");
                }
                return updatedVenue.MapToVenueDetail();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating venue with ID {VenueId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteVenueAsync(string id, string userId)
        {
            try
            {
                if (!long.TryParse(id, out long venueId))
                {
                    _logger.LogWarning("Invalid venue ID format: {Id}", id);
                    return false;
                }
                var venue = await _unitOfWork.Venues.GetByIdAsync(venueId);
                if (venue == null)
                {
                    return false;
                }

                venue.IsDeleted = true;
                venue.DeletedAt = SystemClock.Instance.GetCurrentInstant();
                venue.DeletedByUserId = userId;

                _unitOfWork.Venues.Update(venue);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting venue with ID {VenueId}", id);
                return false;
            }
        }
    }
}
namespace MirthSystems.Pulse.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Cronos;
    using Microsoft.Extensions.Logging;
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models;
    using NodaTime;
    using NetTopologySuite.Geometries;
    using Azure.Maps.Search.Models;
    using Microsoft.IdentityModel.Tokens;
    using MirthSystems.Pulse.Core.Extensions;

    public class SpecialService : ISpecialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAzureMapsApiService _mapsApiService;
        private readonly ILogger<SpecialService> _logger;

        public SpecialService(
            IUnitOfWork unitOfWork,
            IAzureMapsApiService mapsApiService,
            ILogger<SpecialService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapsApiService = mapsApiService;
            _logger = logger;
        }

        public async Task<PagedResult<SpecialListItem>> GetSpecialsAsync(GetSpecialsRequest request)
        {
            try
            {
                Point? searchLocation = null;
                double? radiusInMeters = null;

                if (!string.IsNullOrEmpty(request.Address))
                {
                    var geocodeResponse = await _mapsApiService.SearchClient.GetGeocodingAsync(request.Address);
                    if (geocodeResponse.Value.Features.Count > 0)
                    {
                        var result = geocodeResponse.Value.Features[0];
                        searchLocation = new Point(result.Geometry.Coordinates.Longitude, result.Geometry.Coordinates.Latitude) { SRID = 4326 };
                        radiusInMeters = request.Radius * 1609.34;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Could not geocode address: {request.Address}");
                    }
                }

                Instant searchDateTimeInstant;
                if (!string.IsNullOrEmpty(request.SearchDateTime))
                {
                    if (DateTimeOffset.TryParse(request.SearchDateTime, out var parsedDateTime))
                    {
                        searchDateTimeInstant = Instant.FromDateTimeOffset(parsedDateTime);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Invalid search date time format: {request.SearchDateTime}");
                    }
                }
                else
                {
                    searchDateTimeInstant = SystemClock.Instance.GetCurrentInstant();
                }

                var specials = await _unitOfWork.Specials.GetPagedSpecialsAsync(
                    request.Page,
                    request.PageSize,
                    searchLocation,
                    radiusInMeters,
                    request.SearchTerm,
                    request.SpecialTypeId.HasValue ? (Core.Enums.SpecialTypes)request.SpecialTypeId.Value : null,
                    !request.IsCurrentlyRunning.HasValue || !request.IsCurrentlyRunning.Value);

                var specialListItems = await Task.WhenAll(specials.Select(async s =>
                {
                    var isCurrentlyRunning = await _unitOfWork.Specials.IsSpecialCurrentlyActiveAsync(s.Id, searchDateTimeInstant);
                    return s.MapToSpecialListItem(isCurrentlyRunning);
                }));

                var filteredItems = specialListItems.ToList();

                // Additional filtering for runtime conditions not handled at database level
                if (request.IsCurrentlyRunning.HasValue && request.IsCurrentlyRunning.Value)
                {
                    filteredItems = filteredItems.Where(s => s.IsCurrentlyRunning).ToList();
                }

                if (!string.IsNullOrEmpty(request.VenueId) && long.TryParse(request.VenueId, out long venueId))
                {
                    filteredItems = filteredItems.Where(s => s.VenueId == venueId.ToString()).ToList();
                }
                
                int totalCount = 0;
                // If we filtered items in memory, we need to adjust the total count accordingly
                if (filteredItems.Count != specialListItems.Length)
                {
                    // Rough approximation of total count based on the filter ratio
                    double filterRatio = (double)filteredItems.Count / specialListItems.Length;
                    totalCount = (int)(specials.TotalCount * filterRatio);
                }

                return new PagedResult<SpecialListItem>
                {
                    Items = filteredItems,
                    PagingInfo = PagingInfo.Create(
                        request.Page,
                        request.PageSize,
                        totalCount)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving specials");
                throw;
            }
        }

        public async Task<SpecialDetail?> GetSpecialByIdAsync(string id)
        {
            try
            {
                if (!long.TryParse(id, out long specialId))
                {
                    _logger.LogWarning("Invalid special ID format: {Id}", id);
                    return null;
                }
                var special = await _unitOfWork.Specials.GetSpecialWithVenueAsync(specialId);
                if (special == null)
                {
                    return null;
                }
                var isCurrentlyRunning = await _unitOfWork.Specials.IsSpecialCurrentlyActiveAsync(specialId, SystemClock.Instance.GetCurrentInstant());
                return special.MapToSpecialDetail(isCurrentlyRunning);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving special with ID {SpecialId}", id);
                return null;
            }
        }

        public async Task<SpecialDetail> CreateSpecialAsync(CreateSpecialRequest request, string userId)
        {
            try
            {
                if (!long.TryParse(request.VenueId, out long venueId))
                {
                    throw new ArgumentException("Invalid venue ID format");
                }
                var venue = await _unitOfWork.Venues.GetByIdAsync(venueId);
                if (venue == null)
                {
                    throw new KeyNotFoundException($"Venue with ID {request.VenueId} not found");
                }

                var special = request.MapToNewSpecial(userId);

                await _unitOfWork.Specials.AddAsync(special);
                await _unitOfWork.SaveChangesAsync();

                var createdSpecial = await _unitOfWork.Specials.GetSpecialWithVenueAsync(special.Id);
                if (createdSpecial == null)
                {
                    throw new InvalidOperationException("Failed to retrieve created special");
                }
                var isCurrentlyRunning = await _unitOfWork.Specials.IsSpecialCurrentlyActiveAsync(special.Id, SystemClock.Instance.GetCurrentInstant());
                return createdSpecial.MapToSpecialDetail(isCurrentlyRunning);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating special");
                throw;
            }
        }

        public async Task<SpecialDetail> UpdateSpecialAsync(string id, UpdateSpecialRequest request, string userId)
        {
            try
            {
                if (!long.TryParse(id, out long specialId))
                {
                    throw new ArgumentException("Invalid special ID format");
                }
                var special = await _unitOfWork.Specials.GetSpecialWithVenueAsync(specialId);
                if (special == null)
                {
                    throw new KeyNotFoundException($"Special with ID {id} not found");
                }

                request.MapAndUpdateExistingSpecial(special);
                special.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
                special.UpdatedByUserId = userId;

                _unitOfWork.Specials.Update(special);
                await _unitOfWork.SaveChangesAsync();

                var updatedSpecial = await _unitOfWork.Specials.GetSpecialWithVenueAsync(specialId);
                if (updatedSpecial == null)
                {
                    throw new InvalidOperationException("Failed to retrieve updated special");
                }
                var isCurrentlyRunning = await _unitOfWork.Specials.IsSpecialCurrentlyActiveAsync(specialId, SystemClock.Instance.GetCurrentInstant());
                return updatedSpecial.MapToSpecialDetail(isCurrentlyRunning);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating special with ID {SpecialId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteSpecialAsync(string id, string userId)
        {
            try
            {
                if (!long.TryParse(id, out long specialId))
                {
                    _logger.LogWarning("Invalid special ID format: {Id}", id);
                    return false;
                }
                var special = await _unitOfWork.Specials.GetByIdAsync(specialId);
                if (special == null)
                {
                    return false;
                }

                special.IsDeleted = true;
                special.DeletedAt = SystemClock.Instance.GetCurrentInstant();
                special.DeletedByUserId = userId;

                _unitOfWork.Specials.Update(special);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting special with ID {SpecialId}", id);
                return false;
            }
        }

        public async Task<bool> IsSpecialCurrentlyRunningAsync(string specialId, DateTimeOffset? referenceTime = null)
        {
            try
            {
                if (!long.TryParse(specialId, out long parsedSpecialId))
                {
                    _logger.LogWarning("Invalid special ID format: {Id}", specialId);
                    return false;
                }
                Instant instant = referenceTime.HasValue
                    ? Instant.FromDateTimeOffset(referenceTime.Value)
                    : SystemClock.Instance.GetCurrentInstant();
                return await _unitOfWork.Specials.IsSpecialCurrentlyActiveAsync(parsedSpecialId, instant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if special with ID {SpecialId} is currently running", specialId);
                return false;
            }
        }
    }
}
namespace MirthSystems.Pulse.Infrastructure.Services
{
    using Microsoft.Extensions.Logging;
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models;
    using NodaTime;
    using MirthSystems.Pulse.Core.Extensions;

    public class OperatingScheduleService : IOperatingScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OperatingScheduleService> _logger;

        public OperatingScheduleService(
            IUnitOfWork unitOfWork,
            ILogger<OperatingScheduleService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OperatingScheduleDetail?> GetOperatingScheduleByIdAsync(string id)
        {
            try
            {
                if (!long.TryParse(id, out long scheduleId))
                {
                    _logger.LogWarning("Invalid schedule ID format: {Id}", id);
                    return null;
                }
                var schedule = await _unitOfWork.OperatingSchedules.GetByIdAsync(scheduleId);
                if (schedule == null)
                {
                    return null;
                }
                var venue = await _unitOfWork.Venues.GetByIdAsync(schedule.VenueId);
                return schedule.MapToOperatingScheduleDetail(venue?.Name ?? "Unknown Venue");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving operating schedule with ID {ScheduleId}", id);
                return null;
            }
        }

        public async Task<OperatingScheduleDetail> CreateOperatingScheduleAsync(CreateOperatingScheduleRequest request, string userId)
        {
            try
            {
                if (!long.TryParse(request.VenueId, out long venueId))
                {
                    throw new ArgumentException("Invalid venue ID format");
                }
                var venue = await _unitOfWork.Venues.GetByIdAsync(venueId);
                if (venue == null)
                {
                    throw new KeyNotFoundException($"Venue with ID {request.VenueId} not found");
                }

                var schedule = request.MapToNewOperatingSchedule();

                await _unitOfWork.OperatingSchedules.AddAsync(schedule);
                await _unitOfWork.SaveChangesAsync();

                return schedule.MapToOperatingScheduleDetail(venue.Name ?? "Unknown Venue");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating operating schedule");
                throw;
            }
        }

        public async Task<OperatingScheduleDetail> UpdateOperatingScheduleAsync(string id, UpdateOperatingScheduleRequest request, string userId)
        {
            try
            {
                if (!long.TryParse(id, out long scheduleId))
                {
                    throw new ArgumentException("Invalid schedule ID format");
                }
                var schedule = await _unitOfWork.OperatingSchedules.GetByIdAsync(scheduleId);
                if (schedule == null)
                {
                    throw new KeyNotFoundException($"Operating schedule with ID {id} not found");
                }

                request.MapAndUpdateExistingOperatingSchedule(schedule);

                _unitOfWork.OperatingSchedules.Update(schedule);
                await _unitOfWork.SaveChangesAsync();

                var venue = await _unitOfWork.Venues.GetByIdAsync(schedule.VenueId);
                return schedule.MapToOperatingScheduleDetail(venue?.Name ?? "Unknown Venue");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating operating schedule with ID {ScheduleId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteOperatingScheduleAsync(string id, string userId)
        {
            try
            {
                if (!long.TryParse(id, out long scheduleId))
                {
                    _logger.LogWarning("Invalid schedule ID format: {Id}", id);
                    return false;
                }
                return await _unitOfWork.OperatingSchedules.DeleteAsync(scheduleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting operating schedule with ID {ScheduleId}", id);
                return false;
            }
        }

        public async Task<List<OperatingScheduleDetail>> GetVenueOperatingSchedulesAsync(string venueId)
        {
            try
            {
                if (!long.TryParse(venueId, out long parsedVenueId))
                {
                    _logger.LogWarning("Invalid venue ID format: {Id}", venueId);
                    return new List<OperatingScheduleDetail>();
                }
                var venue = await _unitOfWork.Venues.GetByIdAsync(parsedVenueId);
                if (venue == null)
                {
                    return new List<OperatingScheduleDetail>();
                }

                var schedules = await _unitOfWork.OperatingSchedules.GetSchedulesByVenueIdAsync(parsedVenueId);
                return schedules.Select(s => s.MapToOperatingScheduleDetail(venue.Name ?? "Unknown Venue")).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving operating schedules for venue with ID {VenueId}", venueId);
                return new List<OperatingScheduleDetail>();
            }
        }

        public async Task<bool> CreateOperatingSchedulesForVenueAsync(string venueId, List<CreateOperatingScheduleRequest> requests, string userId)
        {
            try
            {
                if (!long.TryParse(venueId, out long parsedVenueId))
                {
                    throw new ArgumentException("Invalid venue ID format");
                }
                var venue = await _unitOfWork.Venues.GetByIdAsync(parsedVenueId);
                if (venue == null)
                {
                    throw new KeyNotFoundException($"Venue with ID {venueId} not found");
                }

                var existingSchedules = await _unitOfWork.OperatingSchedules.GetSchedulesByVenueIdAsync(parsedVenueId);
                foreach (var schedule in existingSchedules)
                {
                    await _unitOfWork.OperatingSchedules.DeleteAsync(schedule.Id);
                }

                foreach (var request in requests)
                {
                    if (!long.TryParse(request.VenueId, out long requestVenueId) || requestVenueId != parsedVenueId)
                    {
                        throw new ArgumentException("Invalid or mismatched venue ID in request");
                    }

                    var schedule = request.MapToNewOperatingSchedule();

                    await _unitOfWork.OperatingSchedules.AddAsync(schedule);
                }

                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating operating schedules for venue with ID {VenueId}", venueId);
                return false;
            }
        }
    }
}
namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;
    
    /// <summary>
    /// Represents all special promotions associated with a venue.
    /// </summary>
    /// <remarks>
    /// <para>This model aggregates all special promotions for a venue to provide a complete view of what the venue is offering.</para>
    /// <para>It includes the venue's identity information and a collection of special promotions.</para>
    /// <para>Used primarily for display purposes in the user interface and API responses.</para>
    /// </remarks>
    public class VenueSpecials
    {
        /// <summary>
        /// Gets or sets the unique identifier of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "The Rusty Anchor Pub"</para>
        /// <para>- "Downtown Music Hall"</para>
        /// <para>- "Cafe Milano"</para>
        /// </remarks>
        [Required]
        [StringLength(255)]
        public string VenueName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of special promotions for the venue.
        /// </summary>
        /// <remarks>
        /// <para>This collection contains all current and upcoming specials for the venue.</para>
        /// <para>Each item describes a specific promotional offer or event, including its timing and status.</para>
        /// <para>The collection may include specials that are currently active, scheduled for the future, or recurring.</para>
        /// </remarks>
        [Required]
        public ICollection<SpecialListItem> Specials { get; set; } = new List<SpecialListItem>();
    }
}
namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a summary view of a venue for display in lists and search results.
    /// </summary>
    /// <remarks>
    /// <para>This model provides essential information about a venue for preview purposes.</para>
    /// <para>It includes only the most important fields needed for venue listings.</para>
    /// <para>Used for venue listings, search results, and map markers.</para>
    /// </remarks>
    public class VenueListItem
    {
        /// <summary>
        /// Gets or sets the unique identifier of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "The Rusty Anchor Pub"</para>
        /// <para>- "Downtown Music Hall"</para>
        /// <para>- "Cafe Milano"</para>
        /// </remarks>
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a brief description of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "A cozy pub with live music and craft beers."</para>
        /// <para>- "A classic diner serving daily specials and homestyle meals."</para>
        /// <para>- "Upscale cocktail bar with rotating seasonal menu."</para>
        /// </remarks>
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the city or locality where the venue is located.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Chicago"</para>
        /// <para>- "San Francisco"</para>
        /// <para>- "Brooklyn"</para>
        /// </remarks>
        [Required]
        [StringLength(100)]
        public string Locality { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the state, province, or region where the venue is located.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Illinois"</para>
        /// <para>- "California"</para>
        /// <para>- "New York"</para>
        /// </remarks>
        [Required]
        [StringLength(100)]
        public string Region { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL to the venue's profile image.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "https://cdn.pulse.com/venues/123456/profile.jpg"</para>
        /// <para>- "https://storage.googleapis.com/pulse-media/venues/rusty-anchor/logo.png"</para>
        /// </remarks>
        [StringLength(255)]
        public string ProfileImage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the latitude coordinate of the venue's location.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- 41.8781 (Chicago)</para>
        /// <para>- 37.7749 (San Francisco)</para>
        /// <para>- 40.7128 (New York)</para>
        /// </remarks>
        public double? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude coordinate of the venue's location.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- -87.6298 (Chicago)</para>
        /// <para>- -122.4194 (San Francisco)</para>
        /// <para>- -74.0060 (New York)</para>
        /// </remarks>
        public double? Longitude { get; set; }
    }
}
namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents detailed information about a venue.
    /// </summary>
    /// <remarks>
    /// <para>This model provides comprehensive information about a venue for detailed display.</para>
    /// <para>It includes all fields needed for a venue's detail page, including location and contact information.</para>
    /// <para>Used for venue detail pages and venue management interfaces.</para>
    /// </remarks>
    public class VenueDetail
    {
        /// <summary>
        /// Gets or sets the unique identifier of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "The Rusty Anchor Pub"</para>
        /// <para>- "Downtown Music Hall"</para>
        /// <para>- "Cafe Milano"</para>
        /// </remarks>
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a detailed description of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "A cozy pub with live music and craft beers featuring local breweries. Our happy hour runs daily from 4-6pm."</para>
        /// <para>- "A classic diner serving daily specials and homestyle meals since 1985. Famous for our all-day breakfast."</para>
        /// </remarks>
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the venue's phone number.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "+1 (555) 123-4567" (USA)</para>
        /// <para>- "+44 20 7946 0958" (UK)</para>
        /// </remarks>
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the venue's website URL.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "https://www.rustyanchorpub.com"</para>
        /// <para>- "https://downtownmusichall.com"</para>
        /// </remarks>
        [Url]
        [StringLength(255)]
        public string Website { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the venue's email address.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "info@rustyanchorpub.com"</para>
        /// <para>- "contact@downtownmusichall.com"</para>
        /// </remarks>
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL to the venue's profile image.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "https://cdn.pulse.com/venues/123456/profile.jpg"</para>
        /// <para>- "https://storage.googleapis.com/pulse-media/venues/rusty-anchor/logo.png"</para>
        /// </remarks>
        [Url]
        [StringLength(255)]
        public string ProfileImage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the street address of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "123 Main St"</para>
        /// <para>- "456 Broadway"</para>
        /// </remarks>
        [Required]
        [StringLength(255)]
        public string StreetAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets any secondary address information.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Suite 200"</para>
        /// <para>- "Floor 2"</para>
        /// </remarks>
        [StringLength(255)]
        public string SecondaryAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the city or locality where the venue is located.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Chicago"</para>
        /// <para>- "San Francisco"</para>
        /// <para>- "Brooklyn"</para>
        /// </remarks>
        [Required]
        [StringLength(100)]
        public string Locality { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the state, province, or region where the venue is located.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Illinois"</para>
        /// <para>- "California"</para>
        /// <para>- "New York"</para>
        /// </remarks>
        [Required]
        [StringLength(100)]
        public string Region { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the postal or ZIP code of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "60601" (USA)</para>
        /// <para>- "M5V 2T6" (Canada)</para>
        /// <para>- "SW1A 1AA" (UK)</para>
        /// </remarks>
        [Required]
        [StringLength(20)]
        public string Postcode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the country where the venue is located.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "United States"</para>
        /// <para>- "Canada"</para>
        /// <para>- "United Kingdom"</para>
        /// </remarks>
        [Required]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the latitude coordinate of the venue's location.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- 41.8781 (Chicago)</para>
        /// <para>- 37.7749 (San Francisco)</para>
        /// <para>- 40.7128 (New York)</para>
        /// </remarks>
        public double? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude coordinate of the venue's location.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- -87.6298 (Chicago)</para>
        /// <para>- -122.4194 (San Francisco)</para>
        /// <para>- -74.0060 (New York)</para>
        /// </remarks>
        public double? Longitude { get; set; }

        /// <summary>
        /// Gets or sets the collection of operating schedule items for the venue.
        /// </summary>
        /// <remarks>
        /// <para>This collection typically contains seven items, one for each day of the week.</para>
        /// <para>Each item describes the opening and closing times for a specific day or indicates if the venue is closed on that day.</para>
        /// <para>The collection is ordered by day of week, starting from Sunday (0) through Saturday (6).</para>
        /// </remarks>
        [Required]
        public ICollection<OperatingScheduleListItem> BusinessHours { get; set; } = new List<OperatingScheduleListItem>();

        /// <summary>
        /// Gets or sets the timestamp when the venue was created.
        /// </summary>
        /// <remarks>
        /// <para>Example: "2023-01-01T08:00:00Z" for a venue created on January 1, 2023.</para>
        /// </remarks>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the venue was last updated, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>Example: "2023-02-15T10:00:00Z" for a venue updated on February 15, 2023.</para>
        /// </remarks>
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    using MirthSystems.Pulse.Core.Enums;

    /// <summary>
    /// Represents a summary view of a special promotion for display in lists and search results.
    /// </summary>
    /// <remarks>
    /// <para>This model provides essential information about a special for preview purposes.</para>
    /// <para>It includes only the most important fields needed for special listings.</para>
    /// <para>Used for venue special listings, search results, and promotional displays.</para>
    /// </remarks>
    public class SpecialListItem
    {
        /// <summary>
        /// Gets or sets the unique identifier of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the special's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the venue offering this special.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "VenueId must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the venue offering this special.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "The Rusty Anchor Pub"</para>
        /// <para>- "Downtown Music Hall"</para>
        /// <para>- "Cafe Milano"</para>
        /// </remarks>
        [Required]
        [StringLength(255)]
        public string VenueName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the special.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Half-Price Wings Happy Hour"</para>
        /// <para>- "Live Jazz Night"</para>
        /// <para>- "Buy One Get One Free Draft Beer"</para>
        /// </remarks>
        [Required]
        [StringLength(500)]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category of the special.
        /// </summary>
        /// <remarks>
        /// <para>Categorizes the special as one of:</para>
        /// <para>- Food: Food specials like discount meals or appetizers</para>
        /// <para>- Drink: Drink specials like happy hour or discount cocktails</para>
        /// <para>- Entertainment: Entertainment specials like live music or events</para>
        /// </remarks>
        public SpecialTypes Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the special type.
        /// </summary>
        /// <remarks>
        /// <para>The string representation of the Type enum value.</para>
        /// <para>Examples: "Food", "Drink", "Entertainment"</para>
        /// </remarks>
        [Required]
        public string TypeName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start date of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: YYYY-MM-DD</para>
        /// <para>Examples: "2023-12-15", "2024-01-01"</para>
        /// <para>For one-time specials, this is the event date.</para>
        /// <para>For recurring specials, this is the first occurrence.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in format YYYY-MM-DD")]
        public string StartDate { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start time of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "17:00" (5 PM), "20:30" (8:30 PM)</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string StartTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the end time of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "19:00" (7 PM), "23:00" (11 PM)</para>
        /// <para>May be empty for specials without a specific end time.</para>
        /// </remarks>
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string EndTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the special is currently running.
        /// </summary>
        /// <remarks>
        /// <para>True if the special is active at the current time or the time of the query.</para>
        /// <para>False if the special is not currently active (upcoming or ended).</para>
        /// </remarks>
        public bool IsCurrentlyRunning { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the special recurs over time.
        /// </summary>
        /// <remarks>
        /// <para>True if the special repeats on a schedule (e.g., weekly happy hour).</para>
        /// <para>False if the special is a one-time event (e.g., New Year's Eve party).</para>
        /// </remarks>
        public bool IsRecurring { get; set; }
    }
}
namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    using MirthSystems.Pulse.Core.Enums;

    /// <summary>
    /// Represents detailed information about a special promotion.
    /// </summary>
    /// <remarks>
    /// <para>This model provides comprehensive information about a special for detailed display.</para>
    /// <para>It includes all fields needed for a special's detail page, including timing and recurrence information.</para>
    /// <para>Used for special detail pages and special management interfaces.</para>
    /// </remarks>
    public class SpecialDetail
    {
        /// <summary>
        /// Gets or sets the unique identifier of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the special's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the venue offering this special.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "VenueId must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the venue offering this special.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "The Rusty Anchor Pub"</para>
        /// <para>- "Downtown Music Hall"</para>
        /// <para>- "Cafe Milano"</para>
        /// </remarks>
        [Required]
        [StringLength(255)]
        public string VenueName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the special.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Half-Price Wings Happy Hour"</para>
        /// <para>- "Live Jazz Night"</para>
        /// <para>- "Buy One Get One Free Draft Beer"</para>
        /// </remarks>
        [Required]
        [StringLength(500)]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category of the special.
        /// </summary>
        /// <remarks>
        /// <para>Categorizes the special as one of:</para>
        /// <para>- Food: Food specials like discount meals or appetizers</para>
        /// <para>- Drink: Drink specials like happy hour or discount cocktails</para>
        /// <para>- Entertainment: Entertainment specials like live music or events</para>
        /// </remarks>
        public SpecialTypes Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the special type.
        /// </summary>
        /// <remarks>
        /// <para>The string representation of the Type enum value.</para>
        /// <para>Examples: "Food", "Drink", "Entertainment"</para>
        /// </remarks>
        [Required]
        public string TypeName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start date of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: YYYY-MM-DD</para>
        /// <para>Examples: "2023-12-15", "2024-01-01"</para>
        /// <para>For one-time specials, this is the event date.</para>
        /// <para>For recurring specials, this is the first occurrence.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in format YYYY-MM-DD")]
        public string StartDate { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start time of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "17:00" (5 PM), "20:30" (8:30 PM)</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string StartTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the end time of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "19:00" (7 PM), "23:00" (11 PM)</para>
        /// <para>May be empty for specials without a specific end time.</para>
        /// </remarks>
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string EndTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the expiration date of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: YYYY-MM-DD</para>
        /// <para>Examples: "2023-12-31", "2024-03-15"</para>
        /// <para>For one-time specials, this is typically the same as the start date.</para>
        /// <para>For recurring specials, this is the last date the special will be offered.</para>
        /// <para>May be empty for ongoing specials with no defined end date.</para>
        /// </remarks>
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in format YYYY-MM-DD")]
        public string ExpirationDate { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the special recurs over time.
        /// </summary>
        /// <remarks>
        /// <para>True if the special repeats on a schedule (e.g., weekly happy hour).</para>
        /// <para>False if the special is a one-time event (e.g., New Year's Eve party).</para>
        /// </remarks>
        public bool IsRecurring { get; set; }

        /// <summary>
        /// Gets or sets the CRON expression defining the recurrence pattern of the special.
        /// </summary>
        /// <remarks>
        /// <para>Only applicable when IsRecurring is true.</para>
        /// <para>Examples:</para>
        /// <para>- "0 17 * * 1-5" (weekdays at 5 PM)</para>
        /// <para>- "0 20 * * 3" (Wednesdays at 8 PM)</para>
        /// <para>- "0 16 * * 6,0" (weekends at 4 PM)</para>
        /// </remarks>
        [StringLength(100)]
        public string CronSchedule { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the special is currently running.
        /// </summary>
        /// <remarks>
        /// <para>True if the special is active at the current time or the time of the query.</para>
        /// <para>False if the special is not currently active (upcoming or ended).</para>
        /// </remarks>
        public bool IsCurrentlyRunning { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the special was created.
        /// </summary>
        /// <remarks>
        /// <para>Example: "2023-04-01T09:00:00Z" for a special created on April 1, 2023.</para>
        /// </remarks>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the special was last updated, if applicable.
        /// </summary>
        /// <remarks>
        /// <para>Example: "2023-04-15T14:00:00Z" for a special updated on April 15, 2023.</para>
        /// </remarks>
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
namespace MirthSystems.Pulse.Core.Models
{
    /// <summary>
    /// Represents pagination metadata for API responses.
    /// </summary>
    public class PagingInfo
    {
        /// <summary>
        /// Gets or sets the current page number (1-based).
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the total number of items across all pages.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets a value indicating whether there is a previous page available.
        /// </summary>
        public bool HasPreviousPage => CurrentPage > 1;

        /// <summary>
        /// Gets a value indicating whether there is a next page available.
        /// </summary>
        public bool HasNextPage => CurrentPage < TotalPages;

        /// <summary>
        /// Creates a new paging info object with calculated total pages.
        /// </summary>
        /// <param name="currentPage">The current page number (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="totalCount">The total count of items across all pages.</param>
        /// <returns>A configured paging info object with all properties set.</returns>
        public static PagingInfo Create(int currentPage, int pageSize, int totalCount)
        {
            return new PagingInfo
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)Math.Max(1, pageSize))
            };
        }
    }
}
namespace MirthSystems.Pulse.Core.Models
{
    /// <summary>
    /// Represents pagination metadata for paged API responses.
    /// </summary>
    /// <remarks>
    /// <para>This class provides information about the current page, total pages, and navigation capabilities.</para>
    /// <para>It is used to help clients build UI pagination controls and navigate through paged data.</para>
    /// <para>The class also includes derived properties to simplify page navigation logic.</para>
    /// </remarks>
    public class PaginationData
    {
        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        /// <remarks>
        /// <para>This is a 1-based index (first page is 1, not 0).</para>
        /// <para>Example: 1 means the first page of results.</para>
        /// </remarks>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        /// <remarks>
        /// <para>This defines how many items are included in each page of results.</para>
        /// <para>Example: 20 means each page contains up to 20 items.</para>
        /// <para>The last page may contain fewer items if the total count is not evenly divisible by the page size.</para>
        /// </remarks>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the total number of items across all pages.
        /// </summary>
        /// <remarks>
        /// <para>This represents the total count of all available items, not just those on the current page.</para>
        /// <para>Example: 57 means there are 57 total items available across all pages.</para>
        /// </remarks>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        /// <remarks>
        /// <para>This is calculated as ⌈TotalCount ÷ PageSize⌉ (ceiling division).</para>
        /// <para>Example: With 57 total items and a page size of 20, there would be 3 pages.</para>
        /// </remarks>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets a value indicating whether there is a previous page available.
        /// </summary>
        /// <remarks>
        /// <para>True when the current page is not the first page (Page > 1).</para>
        /// <para>False when the current page is the first page (Page == 1).</para>
        /// <para>This simplifies UI rendering logic for pagination controls.</para>
        /// </remarks>
        public bool HasPreviousPage => Page > 1;

        /// <summary>
        /// Gets a value indicating whether there is a next page available.
        /// </summary>
        /// <remarks>
        /// <para>True when the current page is not the last page (Page &lt; TotalPages).</para>
        /// <para>False when the current page is the last page (Page >= TotalPages).</para>
        /// <para>This simplifies UI rendering logic for pagination controls.</para>
        /// </remarks>
        public bool HasNextPage => Page < TotalPages;

        /// <summary>
        /// Creates a new pagination data object with calculated total pages.
        /// </summary>
        /// <param name="page">The current page number (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="totalCount">The total count of items across all pages.</param>
        /// <returns>A configured pagination data object with all properties set.</returns>
        /// <remarks>
        /// <para>This factory method simplifies the creation of pagination data with proper total pages calculation.</para>
        /// <para>It ensures the ceiling division operation is performed correctly when calculating TotalPages.</para>
        /// <para>Example usage: PaginationData.Create(2, 10, 25) creates pagination data for page 2 of 3 pages.</para>
        /// </remarks>
        public static PaginationData Create(int page, int pageSize, int totalCount)
        {
            return new PaginationData
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }
    }
}
namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PageQueryParams
    {
        /// <summary>
        /// Gets or sets the page number for pagination.
        /// </summary>
        /// <remarks>
        /// <para>This is a 1-based index (first page is 1, not 0).</para>
        /// <para>Default is 1.</para>
        /// </remarks>
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of items per page for pagination.
        /// </summary>
        /// <remarks>
        /// <para>Defines how many items are returned per page.</para>
        /// <para>Default is 20.</para>
        /// <para>Maximum allowed value is 10000.</para>
        /// </remarks>
        [Range(1, 10000)]
        public int PageSize { get; set; } = 100;
    }
}
namespace MirthSystems.Pulse.Core.Models
{
    /// <summary>
    /// Represents a paginated result set for API responses.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Gets or sets the collection of items for the current page.
        /// </summary>
        public ICollection<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Gets or sets the pagination information.
        /// </summary>
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
namespace MirthSystems.Pulse.Core.Models
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    public class PagedList<T> : List<T>
    {
        public int PageSize { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalCount { get; private set; }
        public int PageCount { get; private set; }
        public bool HasPrevious => (CurrentPage > 1);
        public bool HasNext => (CurrentPage < PageCount);

        private PagedList(List<T> items, int pageSize, int currentPage, int totalItemCount)
        {
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalCount = totalItemCount;
            PageCount = (int)Math.Ceiling(TotalCount / (double)PageSize);
            AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> items, int pageIndex, int pageSize)
        {
            var count = await items.CountAsync();
            var pagedItems = await items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(pagedItems, pageSize, pageIndex, count);
        }
    }
}
namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a summary view of a venue's operating schedule for a specific day of the week.
    /// </summary>
    /// <remarks>
    /// <para>This model provides essential information about when a venue is open on a specific day.</para>
    /// <para>It includes opening and closing times or indicates if the venue is closed that day.</para>
    /// <para>Used for displaying business hours in venue listings and detail pages.</para>
    /// </remarks>
    public class OperatingScheduleListItem
    {
        /// <summary>
        /// Gets or sets the unique identifier of the operating schedule.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the schedule's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the day of the week for this schedule entry.
        /// </summary>
        /// <remarks>
        /// <para>This uses the standard .NET System.DayOfWeek enum.</para>
        /// <para>The values are: Sunday (0), Monday (1), Tuesday (2), etc.</para>
        /// </remarks>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the name of the day of the week.
        /// </summary>
        /// <remarks>
        /// <para>The string representation of the DayOfWeek enum value.</para>
        /// <para>Examples: "Sunday", "Monday", "Tuesday", etc.</para>
        /// </remarks>
        [Required]
        public string DayName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the opening time for the venue on this day.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "09:00" (9 AM), "17:30" (5:30 PM)</para>
        /// <para>This property is still populated even when IsClosed is true, but is not displayed in that case.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string OpenTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the closing time for the venue on this day.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "17:00" (5 PM), "02:00" (2 AM the next day)</para>
        /// <para>This property is still populated even when IsClosed is true, but is not displayed in that case.</para>
        /// <para>If this time is earlier than OpenTime, it's interpreted as crossing midnight into the next day.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string CloseTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the venue is closed on this day.
        /// </summary>
        /// <remarks>
        /// <para>When true, the venue is completely closed on this day of the week.</para>
        /// <para>When false, the venue is open according to the OpenTime and CloseTime properties.</para>
        /// </remarks>
        public bool IsClosed { get; set; }
    }
}
namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents detailed information about a venue's operating schedule for a specific day of the week.
    /// </summary>
    /// <remarks>
    /// <para>This model provides comprehensive information about when a venue is open on a specific day.</para>
    /// <para>It includes venue identification, day of week, and operating hours information.</para>
    /// <para>Used for schedule management interfaces and API responses.</para>
    /// </remarks>
    public class OperatingScheduleDetail
    {
        /// <summary>
        /// Gets or sets the unique identifier of the operating schedule.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the schedule's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the venue associated with this schedule.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "VenueId must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the venue associated with this schedule.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "The Rusty Anchor Pub"</para>
        /// <para>- "Downtown Music Hall"</para>
        /// <para>- "Cafe Milano"</para>
        /// </remarks>
        [Required]
        [StringLength(255)]
        public string VenueName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the day of the week for this schedule entry.
        /// </summary>
        /// <remarks>
        /// <para>This uses the standard .NET System.DayOfWeek enum.</para>
        /// <para>The values are: Sunday (0), Monday (1), Tuesday (2), etc.</para>
        /// </remarks>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the name of the day of the week.
        /// </summary>
        /// <remarks>
        /// <para>The string representation of the DayOfWeek enum value.</para>
        /// <para>Examples: "Sunday", "Monday", "Tuesday", etc.</para>
        /// </remarks>
        [Required]
        public string DayName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the opening time for the venue on this day.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "09:00" (9 AM), "17:30" (5:30 PM)</para>
        /// <para>This property is still populated even when IsClosed is true, but is not displayed in that case.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string OpenTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the closing time for the venue on this day.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "17:00" (5 PM), "02:00" (2 AM the next day)</para>
        /// <para>This property is still populated even when IsClosed is true, but is not displayed in that case.</para>
        /// <para>If this time is earlier than OpenTime, it's interpreted as crossing midnight into the next day.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string CloseTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the venue is closed on this day.
        /// </summary>
        /// <remarks>
        /// <para>When true, the venue is completely closed on this day of the week.</para>
        /// <para>When false, the venue is open according to the OpenTime and CloseTime properties.</para>
        /// </remarks>
        public bool IsClosed { get; set; }
    }
}
namespace MirthSystems.Pulse.Core.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents the complete set of operating hours for a venue.
    /// </summary>
    /// <remarks>
    /// <para>This model aggregates all operating schedules for a venue to provide a complete view of when the venue is open.</para>
    /// <para>It includes the venue's identity information and a collection of individual schedule items for each day of the week.</para>
    /// <para>Used primarily for display purposes in the user interface and API responses.</para>
    /// </remarks>
    public class BusinessHours
    {
        /// <summary>
        /// Gets or sets the unique identifier of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "Id must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "The Rusty Anchor Pub"</para>
        /// <para>- "Downtown Music Hall"</para>
        /// <para>- "Cafe Milano"</para>
        /// </remarks>
        [Required]
        [StringLength(255)]
        public string VenueName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of operating schedule items for the venue.
        /// </summary>
        /// <remarks>
        /// <para>This collection typically contains seven items, one for each day of the week.</para>
        /// <para>Each item describes the opening and closing times for a specific day or indicates if the venue is closed on that day.</para>
        /// <para>The collection is ordered by day of week, starting from Sunday (0) through Saturday (6).</para>
        /// </remarks>
        [Required]
        public ICollection<OperatingScheduleListItem> ScheduleItems { get; set; } = new List<OperatingScheduleListItem>();
    }
}
namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for updating an existing venue.
    /// </summary>
    /// <remarks>
    /// <para>This model defines the fields that can be modified when updating a venue entity.</para>
    /// <para>It includes validation attributes to ensure the data meets business requirements.</para>
    /// <para>Unlike CreateVenueRequest, this does not include operating schedules as those are updated separately.</para>
    /// </remarks>
    public class UpdateVenueRequest
    {
        /// <summary>
        /// Gets or sets the name of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "The Rusty Anchor Pub"</para>
        /// <para>- "Downtown Music Hall"</para>
        /// <para>- "Cafe Milano"</para>
        /// </remarks>
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a description of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "A cozy pub with live music and craft beers."</para>
        /// <para>- "A classic diner serving daily specials and homestyle meals."</para>
        /// <para>- "Upscale cocktail bar with rotating seasonal menu."</para>
        /// </remarks>
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the venue's phone number.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "+1 (555) 123-4567" (USA)</para>
        /// <para>- "+44 20 7946 0958" (UK)</para>
        /// </remarks>
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the venue's website URL.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "https://www.rustyanchorpub.com"</para>
        /// <para>- "https://downtownmusichall.com"</para>
        /// </remarks>
        [Url]
        [StringLength(255)]
        public string Website { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the venue's email address.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "info@rustyanchorpub.com"</para>
        /// <para>- "contact@downtownmusichall.com"</para>
        /// </remarks>
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL to the venue's profile image.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "https://cdn.pulse.com/venues/123456/profile.jpg"</para>
        /// <para>- "https://storage.googleapis.com/pulse-media/venues/rusty-anchor/logo.png"</para>
        /// </remarks>
        [Url]
        [StringLength(255)]
        public string ProfileImage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the updated address information for the venue.
        /// </summary>
        /// <remarks>
        /// <para>This is a nested object containing all address components.</para>
        /// <para>The updated address will be geocoded to determine the venue's new geographic coordinates.</para>
        /// <para>If the address changes significantly, this may affect location-based searches and nearby venue results.</para>
        /// </remarks>
        [Required]
        public UpdateAddressRequest Address { get; set; } = new UpdateAddressRequest();
    }
}
namespace MirthSystems.Pulse.Core.Models.Requests
{
    using MirthSystems.Pulse.Core.Enums;

    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for updating an existing special promotion.
    /// </summary>
    /// <remarks>
    /// <para>This model defines the fields that can be modified when updating a special entity.</para>
    /// <para>It includes validation attributes to ensure the data meets business requirements.</para>
    /// <para>The structure mirrors CreateSpecialRequest but omits the VenueId since that can't be changed.</para>
    /// </remarks>
    public class UpdateSpecialRequest
    {
        /// <summary>
        /// Gets or sets the description of the special.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Half-Price Wings Happy Hour"</para>
        /// <para>- "Live Jazz Night"</para>
        /// <para>- "Buy One Get One Free Draft Beer"</para>
        /// </remarks>
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category of the special.
        /// </summary>
        /// <remarks>
        /// <para>Categorizes the special as one of:</para>
        /// <para>- Food: Food specials like discount meals or appetizers</para>
        /// <para>- Drink: Drink specials like happy hour or discount cocktails</para>
        /// <para>- Entertainment: Entertainment specials like live music or events</para>
        /// </remarks>
        [Required]
        [EnumDataType(typeof(SpecialTypes))]
        public SpecialTypes Type { get; set; }

        /// <summary>
        /// Gets or sets the start date of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: YYYY-MM-DD</para>
        /// <para>Examples: "2023-12-15", "2024-01-01"</para>
        /// <para>For one-time specials, this is the event date.</para>
        /// <para>For recurring specials, this is the first occurrence.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in format YYYY-MM-DD")]
        public string StartDate { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start time of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "17:00" (5 PM), "20:30" (8:30 PM)</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string StartTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the end time of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "19:00" (7 PM), "23:00" (11 PM)</para>
        /// <para>May be empty for specials without a specific end time.</para>
        /// <para>If this time is earlier than StartTime, it's interpreted as crossing midnight into the next day.</para>
        /// </remarks>
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string EndTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the expiration date of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: YYYY-MM-DD</para>
        /// <para>Examples: "2023-12-31", "2024-03-15"</para>
        /// <para>For one-time specials, this is typically the same as the start date.</para>
        /// <para>For recurring specials, this is the last date the special will be offered.</para>
        /// <para>May be empty for ongoing specials with no defined end date.</para>
        /// </remarks>
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in format YYYY-MM-DD")]
        public string ExpirationDate { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the special recurs over time.
        /// </summary>
        /// <remarks>
        /// <para>True if the special repeats on a schedule (e.g., weekly happy hour).</para>
        /// <para>False if the special is a one-time event (e.g., New Year's Eve party).</para>
        /// <para>When true, the CronSchedule property should be provided.</para>
        /// </remarks>
        [Required]
        public bool IsRecurring { get; set; }

        /// <summary>
        /// Gets or sets the CRON expression defining the recurrence pattern of the special.
        /// </summary>
        /// <remarks>
        /// <para>Only applicable when IsRecurring is true.</para>
        /// <para>Examples:</para>
        /// <para>- "0 17 * * 1-5" (weekdays at 5 PM)</para>
        /// <para>- "0 20 * * 3" (Wednesdays at 8 PM)</para>
        /// <para>- "0 16 * * 6,0" (weekends at 4 PM)</para>
        /// </remarks>
        [StringLength(100)]
        public string CronSchedule { get; set; } = string.Empty;
    }
}
namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for updating an existing operating schedule entry.
    /// </summary>
    /// <remarks>
    /// <para>This model defines the fields that can be modified when updating an operating schedule entry.</para>
    /// <para>It includes validation attributes to ensure the data meets business requirements.</para>
    /// <para>Unlike CreateOperatingScheduleRequest, this does not include VenueId or DayOfWeek as those are immutable.</para>
    /// </remarks>
    public class UpdateOperatingScheduleRequest
    {
        /// <summary>
        /// Gets or sets the opening time for the venue on this day.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "09:00" (9 AM), "17:30" (5:30 PM)</para>
        /// <para>This property is required even when IsClosed is true, though it will be ignored in that case.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string TimeOfOpen { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the closing time for the venue on this day.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "17:00" (5 PM), "02:00" (2 AM the next day)</para>
        /// <para>This property is required even when IsClosed is true, though it will be ignored in that case.</para>
        /// <para>If this time is earlier than TimeOfOpen, it's interpreted as crossing midnight into the next day.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string TimeOfClose { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the venue is closed on this day.
        /// </summary>
        /// <remarks>
        /// <para>When true, the venue is completely closed on this day of the week.</para>
        /// <para>When false, the venue is open according to the TimeOfOpen and TimeOfClose properties.</para>
        /// </remarks>
        [Required]
        public bool IsClosed { get; set; }
    }
}
namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for updating an existing address.
    /// </summary>
    /// <remarks>
    /// <para>This model defines the fields that can be modified when updating an address entity.</para>
    /// <para>It includes validation attributes to ensure the data meets business requirements.</para>
    /// <para>Used primarily within venue update requests and as a standalone API request.</para>
    /// <para>The structure mirrors CreateAddressRequest for consistency.</para>
    /// </remarks>
    public class UpdateAddressRequest
    {
        /// <summary>
        /// Gets or sets the street address (primary address line).
        /// </summary>
        /// <remarks>
        /// <para>This should contain the street number and name.</para>
        /// <para>Examples include:</para>
        /// <para>- "123 Main St"</para>
        /// <para>- "456 Broadway"</para>
        /// <para>- "789 Park Avenue"</para>
        /// </remarks>
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string StreetAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the secondary address information.
        /// </summary>
        /// <remarks>
        /// <para>This should contain apartment, suite, unit, building, floor, etc.</para>
        /// <para>Examples include:</para>
        /// <para>- "Suite 200"</para>
        /// <para>- "Apt 303"</para>
        /// <para>- "Floor 15"</para>
        /// </remarks>
        [StringLength(50)]
        public string SecondaryAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the city or locality.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Chicago"</para>
        /// <para>- "San Francisco"</para>
        /// <para>- "Miami"</para>
        /// </remarks>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Locality { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the state, province, or region.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Illinois"</para>
        /// <para>- "California"</para>
        /// <para>- "Florida"</para>
        /// <para>You may use either the full name or the standard abbreviation (IL, CA, FL).</para>
        /// </remarks>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Region { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the postal code or ZIP code.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "60601" (USA)</para>
        /// <para>- "M5V 2T6" (Canada)</para>
        /// <para>- "SW1A 1AA" (UK)</para>
        /// </remarks>
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Postcode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "United States"</para>
        /// <para>- "Canada"</para>
        /// <para>- "United Kingdom"</para>
        /// <para>Use the full country name rather than abbreviations or codes.</para>
        /// </remarks>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Country { get; set; } = string.Empty;
    }
}
namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for searching and filtering specials in the system.
    /// </summary>
    /// <remarks>
    /// <para>This model defines the parameters for searching and filtering specials.</para>
    /// <para>It includes options for location-based searches, text search, time-based filtering, and pagination.</para>
    /// <para>Used primarily for API endpoints that retrieve lists of specials.</para>
    /// </remarks>
    public class GetSpecialsRequest : PageQueryParams
    {
        /// <summary>
        /// Gets or sets the address to search near.
        /// </summary>
        /// <remarks>
        /// <para>This address will be geocoded to determine the center point for proximity searches.</para>
        /// <para>Examples include:</para>
        /// <para>- "123 Main St, Chicago, IL"</para>
        /// <para>- "Times Square, New York, NY"</para>
        /// <para>- "90210" (ZIP code only)</para>
        /// </remarks>
        [Required]
        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the search radius in miles.
        /// </summary>
        /// <remarks>
        /// <para>Specifies the radius (in miles) to search around the geocoded address.</para>
        /// <para>Default value is 5 miles.</para>
        /// <para>Examples: 1 (walking distance), 5 (neighborhood), 25 (regional)</para>
        /// </remarks>
        [Range(0.1, 100)]
        public double Radius { get; set; } = 5;

        /// <summary>
        /// Gets or sets the date and time for which to check special availability.
        /// </summary>
        /// <remarks>
        /// <para>Format: ISO 8601 format (YYYY-MM-DDThh:mm:ssZ)</para>
        /// <para>Examples: "2023-12-15T18:30:00Z", "2024-01-01T00:00:00Z"</para>
        /// <para>If empty, the current date and time will be used.</para>
        /// </remarks>
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z$", ErrorMessage = "Invalid date-time format. Use ISO 8601 format (e.g., 2025-05-05T18:30:00Z).")]
        public string SearchDateTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the text to search for in special descriptions and venue names.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "happy hour" (finds all happy hour specials)</para>
        /// <para>- "pizza" (finds specials mentioning pizza)</para>
        /// <para>- "jazz" (finds music venues with jazz performances)</para>
        /// </remarks>
        [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters.")]
        public string SearchTerm { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the venue ID to filter specials by.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// <para>When provided, only returns specials for the specified venue.</para>
        /// </remarks>
        [RegularExpression(@"^\d+$", ErrorMessage = "VenueId must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the special type ID to filter by.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- 0: Food specials</para>
        /// <para>- 1: Drink specials</para>
        /// <para>- 2: Entertainment specials</para>
        /// </remarks>
        [Range(1, int.MaxValue, ErrorMessage = "SpecialTypeId must be a positive number.")]
        public int? SpecialTypeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include only currently running specials.
        /// </summary>
        /// <remarks>
        /// <para>True: Only return specials that are active at SearchDateTime.</para>
        /// <para>False: Include all specials regardless of their active status.</para>
        /// <para>Default is true.</para>
        /// </remarks>
        public bool? IsCurrentlyRunning { get; set; } = true;
    }
}
namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for creating a new venue.
    /// </summary>
    /// <remarks>
    /// <para>This model defines the required and optional fields for creating a new venue entity.</para>
    /// <para>It includes validation attributes to ensure the data meets business requirements.</para>
    /// <para>It requires nested address and operating schedule information to create a complete venue profile.</para>
    /// </remarks>
    public class CreateVenueRequest
    {
        /// <summary>
        /// Gets or sets the name of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "The Rusty Anchor Pub"</para>
        /// <para>- "Downtown Music Hall"</para>
        /// <para>- "Cafe Milano"</para>
        /// </remarks>
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a description of the venue.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "A cozy pub with live music and craft beers."</para>
        /// <para>- "A classic diner serving daily specials and homestyle meals."</para>
        /// <para>- "Upscale cocktail bar with rotating seasonal menu."</para>
        /// </remarks>
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the venue's phone number.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "+1 (555) 123-4567" (USA)</para>
        /// <para>- "+44 20 7946 0958" (UK)</para>
        /// </remarks>
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the venue's website URL.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "https://www.rustyanchorpub.com"</para>
        /// <para>- "https://downtownmusichall.com"</para>
        /// </remarks>
        [Url]
        [StringLength(255)]
        public string Website { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the venue's email address.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "info@rustyanchorpub.com"</para>
        /// <para>- "contact@downtownmusichall.com"</para>
        /// </remarks>
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL to the venue's profile image.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "https://cdn.pulse.com/venues/123456/profile.jpg"</para>
        /// <para>- "https://storage.googleapis.com/pulse-media/venues/rusty-anchor/logo.png"</para>
        /// </remarks>
        [Url]
        [StringLength(255)]
        public string ProfileImage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the address information for the venue.
        /// </summary>
        /// <remarks>
        /// <para>This is a nested object containing all address components.</para>
        /// <para>The address will be geocoded to determine the venue's geographic coordinates.</para>
        /// </remarks>
        [Required]
        public CreateAddressRequest Address { get; set; } = new CreateAddressRequest();

        /// <summary>
        /// Gets or sets the collection of operating schedule entries for the venue.
        /// </summary>
        /// <remarks>
        /// <para>This collection should include operating hours for each day of the week.</para>
        /// <para>Typically, there should be seven entries, one for each day of the week.</para>
        /// <para>Each entry specifies whether the venue is open on that day and, if so, the opening and closing times.</para>
        /// </remarks>
        [Required]
        [MinLength(1, ErrorMessage = "At least one operating schedule must be provided.")]
        public ICollection<CreateOperatingScheduleRequest> BusinessHours { get; set; } = new List<CreateOperatingScheduleRequest>();
    }
}
namespace MirthSystems.Pulse.Core.Models.Requests
{
    using MirthSystems.Pulse.Core.Enums;

    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for creating a new special promotion.
    /// </summary>
    /// <remarks>
    /// <para>This model defines the required and optional fields for creating a new special entity.</para>
    /// <para>It includes validation attributes to ensure the data meets business requirements.</para>
    /// <para>It supports both one-time specials and recurring specials with CRON-based scheduling.</para>
    /// </remarks>
    public class CreateSpecialRequest
    {
        /// <summary>
        /// Gets or sets the ID of the venue offering this special.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// <para>This field associates the special with a specific venue.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "VenueId must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the special.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Half-Price Wings Happy Hour"</para>
        /// <para>- "Live Jazz Night"</para>
        /// <para>- "Buy One Get One Free Draft Beer"</para>
        /// </remarks>
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category of the special.
        /// </summary>
        /// <remarks>
        /// <para>Categorizes the special as one of:</para>
        /// <para>- Food: Food specials like discount meals or appetizers</para>
        /// <para>- Drink: Drink specials like happy hour or discount cocktails</para>
        /// <para>- Entertainment: Entertainment specials like live music or events</para>
        /// </remarks>
        [Required]
        [EnumDataType(typeof(SpecialTypes))]
        public SpecialTypes Type { get; set; }

        /// <summary>
        /// Gets or sets the start date of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: YYYY-MM-DD</para>
        /// <para>Examples: "2023-12-15", "2024-01-01"</para>
        /// <para>For one-time specials, this is the event date.</para>
        /// <para>For recurring specials, this is the first occurrence.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in format YYYY-MM-DD")]
        public string StartDate { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start time of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "17:00" (5 PM), "20:30" (8:30 PM)</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string StartTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the end time of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "19:00" (7 PM), "23:00" (11 PM)</para>
        /// <para>May be empty for specials without a specific end time.</para>
        /// <para>If this time is earlier than StartTime, it's interpreted as crossing midnight into the next day.</para>
        /// </remarks>
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string EndTime { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the expiration date of the special.
        /// </summary>
        /// <remarks>
        /// <para>Format: YYYY-MM-DD</para>
        /// <para>Examples: "2023-12-31", "2024-03-15"</para>
        /// <para>For one-time specials, this is typically the same as the start date.</para>
        /// <para>For recurring specials, this is the last date the special will be offered.</para>
        /// <para>May be empty for ongoing specials with no defined end date.</para>
        /// </remarks>
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in format YYYY-MM-DD")]
        public string ExpirationDate { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the special recurs over time.
        /// </summary>
        /// <remarks>
        /// <para>True if the special repeats on a schedule (e.g., weekly happy hour).</para>
        /// <para>False if the special is a one-time event (e.g., New Year's Eve party).</para>
        /// <para>When true, the CronSchedule property should be provided.</para>
        /// </remarks>
        [Required]
        public bool IsRecurring { get; set; }

        /// <summary>
        /// Gets or sets the CRON expression defining the recurrence pattern of the special.
        /// </summary>
        /// <remarks>
        /// <para>Only applicable when IsRecurring is true.</para>
        /// <para>Examples:</para>
        /// <para>- "0 17 * * 1-5" (weekdays at 5 PM)</para>
        /// <para>- "0 20 * * 3" (Wednesdays at 8 PM)</para>
        /// <para>- "0 16 * * 6,0" (weekends at 4 PM)</para>
        /// </remarks>
        [RegularExpression(@"^(\*|[0-9,-/]+)(\s+(\*|[0-9,-/]+)){4,5}$", ErrorMessage = "Invalid CRON expression.")]
        [StringLength(100)]
        public string CronSchedule { get; set; } = string.Empty;
    }
}
namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for creating a new operating schedule entry for a venue.
    /// </summary>
    /// <remarks>
    /// <para>This model defines when a venue is open on a specific day of the week.</para>
    /// <para>It includes validation attributes to ensure the data meets business requirements.</para>
    /// <para>Used both as a standalone API request and within venue creation requests.</para>
    /// </remarks>
    public class CreateOperatingScheduleRequest
    {
        /// <summary>
        /// Gets or sets the ID of the venue this schedule applies to.
        /// </summary>
        /// <remarks>
        /// <para>Format example: "123456"</para>
        /// <para>This is a string representation of the venue's database ID.</para>
        /// <para>This field is required for standalone operating schedule creation requests.</para>
        /// <para>When used within a CreateVenueRequest, this field is ignored as the venue ID is not yet available.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "VenueId must be a positive integer")]
        public string VenueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the day of the week this schedule applies to.
        /// </summary>
        /// <remarks>
        /// <para>This uses the standard .NET System.DayOfWeek enum.</para>
        /// <para>The values are: Sunday (0), Monday (1), Tuesday (2), etc.</para>
        /// </remarks>
        [Required]
        [Range(0, 6)]
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the opening time for the venue on this day.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "09:00" (9 AM), "17:30" (5:30 PM)</para>
        /// <para>This property is required even when IsClosed is true, though it will be ignored in that case.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string TimeOfOpen { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the closing time for the venue on this day.
        /// </summary>
        /// <remarks>
        /// <para>Format: HH:mm (24-hour)</para>
        /// <para>Examples: "17:00" (5 PM), "02:00" (2 AM the next day)</para>
        /// <para>This property is required even when IsClosed is true, though it will be ignored in that case.</para>
        /// <para>If this time is earlier than TimeOfOpen, it's interpreted as crossing midnight into the next day.</para>
        /// </remarks>
        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Time must be in format HH:mm")]
        public string TimeOfClose { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the venue is closed on this day.
        /// </summary>
        /// <remarks>
        /// <para>When true, the venue is completely closed on this day of the week.</para>
        /// <para>When false, the venue is open according to the TimeOfOpen and TimeOfClose properties.</para>
        /// </remarks>
        [Required]
        public bool IsClosed { get; set; }
    }
}
namespace MirthSystems.Pulse.Core.Models.Requests
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Request model for creating a new address.
    /// </summary>
    /// <remarks>
    /// <para>This model defines the required and optional fields for creating a new address entity.</para>
    /// <para>It includes validation attributes to ensure the data meets business requirements.</para>
    /// <para>Used primarily within venue creation requests and as a standalone API request.</para>
    /// </remarks>
    public class CreateAddressRequest
    {
        /// <summary>
        /// Gets or sets the street address (primary address line).
        /// </summary>
        /// <remarks>
        /// <para>This should contain the street number and name.</para>
        /// <para>Examples include:</para>
        /// <para>- "123 Main St"</para>
        /// <para>- "456 Broadway"</para>
        /// <para>- "789 Park Avenue"</para>
        /// </remarks>
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string StreetAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the secondary address information.
        /// </summary>
        /// <remarks>
        /// <para>This should contain apartment, suite, unit, building, floor, etc.</para>
        /// <para>Examples include:</para>
        /// <para>- "Suite 200"</para>
        /// <para>- "Apt 303"</para>
        /// <para>- "Floor 15"</para>
        /// </remarks>
        [StringLength(50)]
        public string SecondaryAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the city or locality.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Chicago"</para>
        /// <para>- "San Francisco"</para>
        /// <para>- "Miami"</para>
        /// </remarks>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Locality { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the state, province, or region.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "Illinois"</para>
        /// <para>- "California"</para>
        /// <para>- "Florida"</para>
        /// <para>You may use either the full name or the standard abbreviation (IL, CA, FL).</para>
        /// </remarks>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Region { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the postal code or ZIP code.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "60601" (USA)</para>
        /// <para>- "M5V 2T6" (Canada)</para>
        /// <para>- "SW1A 1AA" (UK)</para>
        /// </remarks>
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Postcode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <remarks>
        /// <para>Examples include:</para>
        /// <para>- "United States"</para>
        /// <para>- "Canada"</para>
        /// <para>- "United Kingdom"</para>
        /// <para>Use the full country name rather than abbreviations or codes.</para>
        /// </remarks>
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Country { get; set; } = string.Empty;
    }
}
