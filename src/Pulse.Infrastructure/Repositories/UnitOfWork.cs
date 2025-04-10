namespace Pulse.Infrastructure.Repositories
{
    using Microsoft.Extensions.Logging;
    using NodaTime;

    using Pulse.Core.Contracts;

    /// <summary>
    /// Implementation of the Unit of Work pattern to coordinate operations across repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IClock _clock;
        private readonly ILogger<UnitOfWork> _logger;
        private bool _disposed = false;

        public IVenueRepository Venues { get; }
        public IVenueTypeRepository VenueTypes { get; }
        public ISpecialRepository Specials { get; }
        public IOperatingScheduleRepository BusinessHours { get; }
        public ITagRepository Tags { get; }
        public ITagToSpecialLinkRepository TagToSpecialLinks { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IClock clock,
            IVenueRepository venues,
            IVenueTypeRepository venueTypes,
            ISpecialRepository specials,
            IOperatingScheduleRepository businessHours,
            ITagRepository tags,
            ITagToSpecialLinkRepository tagToSpecialLinks,
            ILogger<UnitOfWork> logger)
        {
            _context = context;
            _clock = clock;
            _logger = logger;
            Venues = venues;
            VenueTypes = venueTypes;
            Specials = specials;
            BusinessHours = businessHours;
            Tags = tags;
            TagToSpecialLinks = tagToSpecialLinks;
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving changes to database");
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
