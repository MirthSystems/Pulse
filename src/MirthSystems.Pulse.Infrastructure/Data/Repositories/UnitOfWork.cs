namespace MirthSystems.Pulse.Infrastructure.Data.Repositories
{
    using MirthSystems.Pulse.Core.Interfaces;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IVenueRepository _venueRepository;
        private readonly ISpecialRepository _specialRepository;
        private readonly IOperatingScheduleRepository _operatingScheduleRepository;
        private readonly IAddressRepository _addressRepository;
        private bool _disposed;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _venueRepository = new VenueRepository(_context);
            _specialRepository = new SpecialRepository(_context);
            _operatingScheduleRepository = new OperatingScheduleRepository(_context);
            _addressRepository = new AddressRepository(_context);
        }

        public virtual IVenueRepository Venues => _venueRepository;
        public virtual ISpecialRepository Specials => _specialRepository;
        public virtual IOperatingScheduleRepository OperatingSchedules => _operatingScheduleRepository;
        public virtual IAddressRepository Addresses => _addressRepository;

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
