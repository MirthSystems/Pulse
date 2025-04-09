namespace Pulse.Core.Contracts
{
    using Pulse.Core.Models.Requests;
    using Pulse.Core.Models.Responses;

    public interface IVenueService
    {
        Task<VenueListResponse> GetVenuesAsync(VenueQueryRequest request, string? userId);
        Task<VenueDetailResponse?> GetVenueByIdAsync(int id, string? userId);
        Task<NewVenueResponse> CreateVenueAsync(NewVenueRequest request, string userId);
        Task<UpdateVenueResponse?> UpdateVenueAsync(int id, UpdateVenueRequest request, string userId);
        Task<VenueTypeListResponse> GetVenueTypesAsync();
        Task<VenueListResponse> GetManagedVenuesAsync(string userId);
        Task<bool> UserCanManageVenueAsync(int venueId, string userId);
        Task<bool> UserCanManageVenueSpecialsAsync(int venueId, string userId);
    }
}
