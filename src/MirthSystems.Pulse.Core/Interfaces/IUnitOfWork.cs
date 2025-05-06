namespace MirthSystems.Pulse.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IVenueRepository Venues { get; }
        ISpecialRepository Specials { get; }
        IOperatingScheduleRepository OperatingSchedules { get; }
        IAddressRepository Addresses { get; }

        Task<int> SaveChangesAsync();
    }
}
