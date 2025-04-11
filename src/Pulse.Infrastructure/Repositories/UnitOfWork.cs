namespace Pulse.Infrastructure.Repositories
{
    using Microsoft.Extensions.Logging;
    using NodaTime;

    using Pulse.Core.Contracts;

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

        public async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> operation)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var result = await operation();
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task ExecuteInTransactionAsync(Func<Task> operation)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await operation();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
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
