namespace Pulse.Server.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using NodaTime;
    using NodaTime.TimeZones;
    using Pulse.Infrastructure;

    [ApiController]
    [Route("specials")]
    public class SpecialsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IClock _clock;
        private readonly ILogger<SpecialsController> _logger;
        private readonly IDateTimeZoneProvider _dateTimeZoneProvider;

        public SpecialsController(ApplicationDbContext db, IClock clock, ILogger<SpecialsController> logger)
        {
            this._db = db;
            this._clock = clock;
            this._logger = logger;
            this._dateTimeZoneProvider = new DateTimeZoneCache(TzdbDateTimeZoneSource.Default);
        }
    }
}
