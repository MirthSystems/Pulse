namespace MirthSystems.Pulse.Core.Interfaces
{
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models.Responses;
    using MirthSystems.Pulse.Core.Models;

    public interface IVenueService
    {
        /// <summary>
        /// Gets all venues with pagination
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Items per page</param>
        /// <returns>A paged list of venues</returns>
        Task<PagedApiResponse<VenueListItem>> GetVenuesAsync(int page = 1, int pageSize = 20);

        /// <summary>
        /// Gets a venue by ID with its address and business hours
        /// </summary>
        /// <param name="id">The venue ID</param>
        /// <returns>The venue with details</returns>
        Task<VenueDetail?> GetVenueByIdAsync(long id);

        /// <summary>
        /// Gets the business hours for a venue
        /// </summary>
        /// <param name="id">The venue ID</param>
        /// <returns>The venue's business hours</returns>
        Task<BusinessHours?> GetVenueBusinessHoursAsync(long id);

        /// <summary>
        /// Gets the specials for a venue
        /// </summary>
        /// <param name="id">The venue ID</param>
        /// <param name="includeCurrentStatus">Whether to calculate if specials are currently running</param>
        /// <returns>The venue's specials</returns>
        Task<VenueSpecials?> GetVenueSpecialsAsync(long id, bool includeCurrentStatus = true);

        /// <summary>
        /// Creates a new venue
        /// </summary>
        /// <param name="request">The venue creation request</param>
        /// <param name="userId">The ID of the user creating the venue</param>
        /// <returns>The created venue</returns>
        Task<VenueDetail> CreateVenueAsync(CreateVenueRequest request, string userId);

        /// <summary>
        /// Updates an existing venue
        /// </summary>
        /// <param name="id">The venue ID</param>
        /// <param name="request">The venue update request</param>
        /// <param name="userId">The ID of the user updating the venue</param>
        /// <returns>The updated venue</returns>
        Task<VenueDetail> UpdateVenueAsync(long id, UpdateVenueRequest request, string userId);

        /// <summary>
        /// Deletes a venue
        /// </summary>
        /// <param name="id">The venue ID</param>
        /// <param name="userId">The ID of the user deleting the venue</param>
        /// <returns>Success indicator</returns>
        Task<bool> DeleteVenueAsync(long id, string userId);
    }
}
