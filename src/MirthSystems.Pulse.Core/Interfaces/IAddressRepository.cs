namespace MirthSystems.Pulse.Core.Interfaces
{
    using MirthSystems.Pulse.Core.Entities;

    using NetTopologySuite.Geometries;

    public interface IAddressRepository : IRepository<Address>
    {
        /// <summary>
        /// Standardizes an address using PostgreSQL's address_standardizer extension
        /// </summary>
        Task<StandardizedAddress> StandardizeAddressAsync(string addressString);

        /// <summary>
        /// Finds the nearest address to a given point
        /// </summary>
        Task<Address?> FindNearestAddressAsync(Point location, double maxDistanceInMeters);
    }
}
