namespace Pulse.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Pulse.Infrastructure;

    [ApiController]
    [Route("venues")]
    public class VenuesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<VenuesController> _logger;

        public VenuesController(ApplicationDbContext db, ILogger<VenuesController> logger)
        {
            this._db = db;
            this._logger = logger;
        }
    }
}
