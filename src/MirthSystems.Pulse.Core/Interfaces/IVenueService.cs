namespace MirthSystems.Pulse.Core.Interfaces
{
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models.Responses;
    using MirthSystems.Pulse.Core.Models;

    public interface IVenueService
    {
        Task<PagedApiResponse<VenueListItem>> GetVenuesAsync(int page = 1, int pageSize = 20);
        Task<VenueDetail?> GetVenueByIdAsync(string id);
        Task<BusinessHours?> GetVenueBusinessHoursAsync(string id);
        Task<VenueSpecials?> GetVenueSpecialsAsync(string id, bool includeCurrentStatus = true);
        Task<VenueDetail> CreateVenueAsync(CreateVenueRequest request, string userId);
        Task<VenueDetail> UpdateVenueAsync(string id, UpdateVenueRequest request, string userId);
        Task<bool> DeleteVenueAsync(string id, string userId);
    }
}
