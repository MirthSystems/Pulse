namespace MirthSystems.Pulse.Core.Interfaces
{
    using MirthSystems.Pulse.Core.Models;
    using MirthSystems.Pulse.Core.Models.Requests;

    /// <summary>
    /// Service interface for venue operations, providing venue management functionality.
    /// </summary>
    /// <remarks>
    /// <para>This interface defines business logic operations related to venues, including:</para>
    /// <para>- Retrieving venues (individual or paginated listings)</para>
    /// <para>- Creating new venues with address information</para>
    /// <para>- Updating existing venue details</para>
    /// <para>- Deleting venues (soft delete)</para>
    /// <para>- Accessing venue-related information like business hours and specials</para>
    /// <para>Implementations handle data validation, database operations, and external service integration.</para>
    /// </remarks>
    public interface IVenueService
    {
        /// <summary>
        /// Retrieves a paginated list of venues with optional filtering.
        /// </summary>
        /// <param name="request">The request containing pagination and filter criteria.</param>
        /// <returns>A paginated list of venue list items.</returns>
        /// <remarks>
        /// <para>This method supports sophisticated filtering including:</para>
        /// <para>- Text search on name and description</para>
        /// <para>- Geographic proximity to a location</para>
        /// <para>- Filtering by operating schedule (open on specific days/times)</para>
        /// <para>- Filtering by special availability</para>
        /// <para>When no filters are specified, returns a basic paginated list of venues.</para>
        /// <para>Use GetVenueByIdAsync for detailed information about a specific venue.</para>
        /// </remarks>
        Task<PagedResult<VenueItem>> GetVenuesAsync(GetVenuesRequest request);

        /// <summary>
        /// Retrieves a venue by its ID.
        /// </summary>
        /// <param name="id">The venue ID.</param>
        /// <returns>The venue details if found, otherwise null.</returns>
        /// <remarks>
        /// <para>This method returns comprehensive information about a venue.</para>
        /// <para>Includes address, business hours, and other venue properties.</para>
        /// </remarks>
        Task<VenueItemExtended?> GetVenueByIdAsync(string id);

        /// <summary>
        /// Retrieves business hours for a venue by its ID.
        /// </summary>
        /// <param name="id">The venue ID.</param>
        /// <returns>The business hours information if found, otherwise null.</returns>
        /// <remarks>
        /// <para>This method returns the operating schedule for a venue.</para>
        /// <para>Includes hours for each day of the week.</para>
        /// </remarks>
        Task<List<OperatingScheduleItem>?> GetVenueBusinessHoursAsync(string id);

        /// <summary>
        /// Retrieves specials for a venue by its ID.
        /// </summary>
        /// <param name="id">The venue ID.</param>
        /// <param name="includeCurrentStatus">Whether to include the current active status of each special.</param>
        /// <returns>The list of specials for the venue if found, otherwise null.</returns>
        /// <remarks>
        /// <para>This method returns all specials associated with the venue.</para>
        /// <para>When includeCurrentStatus is true, each special includes whether it's currently active.</para>
        /// </remarks>
        Task<List<SpecialItem>?> GetVenueSpecialsAsync(string id, bool includeCurrentStatus = true);

        /// <summary>
        /// Creates a new venue.
        /// </summary>
        /// <param name="request">The venue creation request.</param>
        /// <param name="userId">The ID of the user creating the venue.</param>
        /// <returns>The newly created venue details.</returns>
        /// <remarks>
        /// <para>This method validates the request and persists a new venue.</para>
        /// <para>Includes geocoding the provided address.</para>
        /// </remarks>
        Task<VenueItemExtended> CreateVenueAsync(CreateVenueRequest request, string userId);

        /// <summary>
        /// Updates an existing venue.
        /// </summary>
        /// <param name="id">The venue ID.</param>
        /// <param name="request">The venue update request.</param>
        /// <param name="userId">The ID of the user updating the venue.</param>
        /// <returns>The updated venue details.</returns>
        /// <remarks>
        /// <para>This method validates the request and updates an existing venue.</para>
        /// <para>Includes re-geocoding the address if changed.</para>
        /// <para>Throws KeyNotFoundException if the venue doesn't exist.</para>
        /// </remarks>
        Task<VenueItemExtended> UpdateVenueAsync(string id, UpdateVenueRequest request, string userId);

        /// <summary>
        /// Deletes a venue (soft delete).
        /// </summary>
        /// <param name="id">The venue ID.</param>
        /// <param name="userId">The ID of the user deleting the venue.</param>
        /// <returns>True if the venue was successfully deleted; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method performs a soft delete, marking the venue as deleted but retaining the record.</para>
        /// <para>Returns false if the venue doesn't exist.</para>
        /// </remarks>
        Task<bool> DeleteVenueAsync(string id, string userId);
    }
}
