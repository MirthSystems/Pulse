namespace Pulse.Core.Contracts
{
    using NetTopologySuite.Geometries;

    public interface ILocationService
    {
        Task<Point> GetPointFromAddressAsync(string address);
    }
}
