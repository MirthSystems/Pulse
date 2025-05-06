namespace MirthSystems.Pulse.Core.Interfaces
{
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models.Responses;
    using MirthSystems.Pulse.Core.Models;

    /// <summary>
    /// Service interface for venue-related business operations.
    /// </summary>
    /// <remarks>
    /// <para>This interface defines the contract for venue-related business logic.</para>
    /// <para>It provides methods for retrieving, creating, updating, and deleting venues.</para>
    /// <para>It also includes methods for accessing related data such as business hours and specials.</para>
    /// <para>Implementations handle the coordination between repositories, external services, and mapping to response models.</para>
    /// </remarks>
    public interface IVenueService
    {
        /// <summary>
        /// Gets a paged list of venues.
        /// </summary>
        /// <param name="page">The page number (1-based).</param>
        /// <param name="pageSize">The number of venues per page.</param>
        /// <returns>A paged API response containing venue list items and pagination metadata.</returns>
        /// <remarks>
        /// <para>This method retrieves venues in pages for efficient data transfer.</para>
        /// <para>The response includes venue summary information suitable for listings and search results.</para>
        /// <para>Results are typically ordered by creation date (newest first).</para>
        /// </remarks>
        Task<PagedApiResponse<VenueListItem>> GetVenuesAsync(int page = 1, int pageSize = 20);

        /// <summary>
        /// Gets detailed information about a specific venue by ID.
        /// </summary>
        /// <param name="id">The ID of the venue to retrieve.</param>
        /// <returns>Detailed venue information if found; otherwise, null.</returns>
        /// <remarks>
        /// <para>This method retrieves comprehensive information about a venue including:</para>
        /// <para>- Basic venue information (name, description, contact details)</para>
        /// <para>- Address and location information</para>
        /// <para>- Business hours</para>
        /// <para>The response is suitable for venue detail pages and venue management interfaces.</para>
        /// </remarks>
        Task<VenueDetail?> GetVenueByIdAsync(string id);

        /// <summary>
        /// Gets the business hours for a specific venue.
        /// </summary>
        /// <param name="id">The ID of the venue.</param>
        /// <returns>The venue's business hours if found; otherwise, null.</returns>
        /// <remarks>
        /// <para>This method retrieves the operating schedules for each day of the week for a venue.</para>
        /// <para>The response includes information about opening times, closing times, and closed days.</para>
        /// <para>Results are ordered by day of week, starting from Sunday (0) through Saturday (6).</para>
        /// </remarks>
        Task<BusinessHours?> GetVenueBusinessHoursAsync(string id);

        /// <summary>
        /// Gets the special promotions for a specific venue.
        /// </summary>
        /// <param name="id">The ID of the venue.</param>
        /// <param name="includeCurrentStatus">Whether to include the current running status of each special.</param>
        /// <returns>The venue's special promotions if found; otherwise, null.</returns>
        /// <remarks>
        /// <para>This method retrieves all specials (promotions, events, deals) associated with a venue.</para>
        /// <para>When includeCurrentStatus is true, each special includes a flag indicating if it's currently active.</para>
        /// <para>This status determination accounts for start dates, end dates, and recurrence patterns.</para>
        /// </remarks>
        Task<VenueSpecials?> GetVenueSpecialsAsync(string id, bool includeCurrentStatus = true);

        /// <summary>
        /// Creates a new venue.
        /// </summary>
        /// <param name="request">The venue creation request containing venue details.</param>
        /// <param name="userId">The ID of the user creating the venue.</param>
        /// <returns>The created venue with generated IDs and metadata.</returns>
        /// <remarks>
        /// <para>This method handles the complete venue creation process including:</para>
        /// <para>- Geocoding the venue's address to determine its geographic coordinates</para>
        /// <para>- Creating the venue record with its address</para>
        /// <para>- Creating operating schedule records for each day of the week</para>
        /// <para>- Setting creation metadata (timestamp, user)</para>
        /// </remarks>
        Task<VenueDetail> CreateVenueAsync(CreateVenueRequest request, string userId);

        /// <summary>
        /// Updates an existing venue.
        /// </summary>
        /// <param name="id">The ID of the venue to update.</param>
        /// <param name="request">The venue update request containing the new venue details.</param>
        /// <param name="userId">The ID of the user updating the venue.</param>
        /// <returns>The updated venue details.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when a venue with the specified ID is not found.</exception>
        /// <remarks>
        /// <para>This method handles the complete venue update process including:</para>
        /// <para>- Geocoding the updated address if it has changed</para>
        /// <para>- Updating the venue record and its associated address</para>
        /// <para>- Setting update metadata (timestamp, user)</para>
        /// <para>Note that this method does not update operating schedules, which are updated separately.</para>
        /// </remarks>
        Task<VenueDetail> UpdateVenueAsync(string id, UpdateVenueRequest request, string userId);

        /// <summary>
        /// Soft-deletes a venue.
        /// </summary>
        /// <param name="id">The ID of the venue to delete.</param>
        /// <param name="userId">The ID of the user deleting the venue.</param>
        /// <returns>True if the venue was found and deleted; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method performs a soft delete, which means:</para>
        /// <para>- The venue is marked as deleted but remains in the database</para>
        /// <para>- The venue will be excluded from normal queries</para>
        /// <para>- The deletion is recorded with a timestamp and user ID</para>
        /// </remarks>
        Task<bool> DeleteVenueAsync(string id, string userId);
    }
}
