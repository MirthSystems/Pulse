namespace MirthSystems.Pulse.Infrastructure.Data.Repositories
{
    using MirthSystems.Pulse.Core.Interfaces;

    /// <summary>
    /// Implementation of the Unit of Work pattern to coordinate operations across multiple repositories.
    /// </summary>
    /// <remarks>
    /// <para>This class acts as a facade for all repositories in the system to ensure transactional integrity.</para>
    /// <para>It provides access to all entity repositories through a single interface.</para>
    /// <para>It manages a single database context across all repositories to ensure transactional consistency.</para>
    /// <para>This pattern ensures that all changes to the database are applied atomically.</para>
    /// </remarks>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The database context shared across all repositories.
        /// </summary>
        /// <remarks>
        /// <para>This is the shared context that ensures all operations are part of the same transaction.</para>
        /// </remarks>
        private readonly ApplicationDbContext _context;
        
        /// <summary>
        /// The repository for venue entities.
        /// </summary>
        private readonly IVenueRepository _venueRepository;
        
        /// <summary>
        /// The repository for special entities.
        /// </summary>
        private readonly ISpecialRepository _specialRepository;
        
        /// <summary>
        /// The repository for operating schedule entities.
        /// </summary>
        private readonly IOperatingScheduleRepository _operatingScheduleRepository;
        
        /// <summary>
        /// The repository for address entities.
        /// </summary>
        private readonly IAddressRepository _addressRepository;
        
        /// <summary>
        /// Flag to track whether this instance has been disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <remarks>
        /// <para>This constructor initializes all repositories with the shared database context.</para>
        /// <para>All repositories created here share the same database context to ensure transactional integrity.</para>
        /// </remarks>
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _venueRepository = new VenueRepository(_context);
            _specialRepository = new SpecialRepository(_context);
            _operatingScheduleRepository = new OperatingScheduleRepository(_context);
            _addressRepository = new AddressRepository(_context);
        }

        /// <summary>
        /// Gets the venue repository.
        /// </summary>
        /// <remarks>
        /// <para>This property provides access to venue-related database operations.</para>
        /// </remarks>
        public virtual IVenueRepository Venues => _venueRepository;
        
        /// <summary>
        /// Gets the special repository.
        /// </summary>
        /// <remarks>
        /// <para>This property provides access to special-related database operations.</para>
        /// </remarks>
        public virtual ISpecialRepository Specials => _specialRepository;
        
        /// <summary>
        /// Gets the operating schedule repository.
        /// </summary>
        /// <remarks>
        /// <para>This property provides access to operating schedule-related database operations.</para>
        /// </remarks>
        public virtual IOperatingScheduleRepository OperatingSchedules => _operatingScheduleRepository;
        
        /// <summary>
        /// Gets the address repository.
        /// </summary>
        /// <remarks>
        /// <para>This property provides access to address-related database operations.</para>
        /// </remarks>
        public virtual IAddressRepository Addresses => _addressRepository;

        /// <summary>
        /// Saves all pending changes to the database.
        /// </summary>
        /// <returns>The number of affected rows.</returns>
        /// <remarks>
        /// <para>This method commits all changes made through all repositories to the database.</para>
        /// <para>All operations are executed in a single transaction to ensure data consistency.</para>
        /// <para>If any operation fails, all changes are rolled back.</para>
        /// </remarks>
        public virtual async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Disposes the database context and resources.
        /// </summary>
        /// <remarks>
        /// <para>This method ensures proper cleanup of resources when the unit of work is no longer needed.</para>
        /// </remarks>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">True if disposing managed resources; otherwise, false.</param>
        /// <remarks>
        /// <para>This method is called by the public Dispose method and the finalizer (if necessary).</para>
        /// <para>It disposes the database context when disposing is true and the instance hasn't been disposed yet.</para>
        /// </remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}
