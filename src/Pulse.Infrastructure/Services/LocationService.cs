namespace Pulse.Infrastructure.Services
{
    using NetTopologySuite.Geometries;

    using Pulse.Core.Contracts;

    public class LocationService : ILocationService
    {
        public Task<Point> GetPointFromAddressAsync(string address)
        {
            // For MVP, we'll return a default point (0,0)
            // In a future implementation, this would call an actual geocoding service
            var point = new Point(0, 0) { SRID = 4326 };
            return Task.FromResult(point);
        }
    }
}
