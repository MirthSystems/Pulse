namespace MirthSystems.Pulse.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models.Responses;
    using MirthSystems.Pulse.Core.Models;

    public interface ISpecialService
    {
        /// <summary>
        /// Gets all specials with optional filtering and pagination
        /// </summary>
        /// <param name="request">The query parameters</param>
        /// <returns>A paged list of specials</returns>
        Task<PagedApiResponse<SpecialListItem>> GetSpecialsAsync(GetSpecialsRequest request);

        /// <summary>
        /// Gets a special by ID
        /// </summary>
        /// <param name="id">The special ID</param>
        /// <returns>The special details</returns>
        Task<SpecialDetail?> GetSpecialByIdAsync(long id);

        /// <summary>
        /// Creates a new special
        /// </summary>
        /// <param name="request">The special creation request</param>
        /// <param name="userId">The ID of the user creating the special</param>
        /// <returns>The created special</returns>
        Task<SpecialDetail> CreateSpecialAsync(CreateSpecialRequest request, string userId);

        /// <summary>
        /// Updates an existing special
        /// </summary>
        /// <param name="id">The special ID</param>
        /// <param name="request">The special update request</param>
        /// <param name="userId">The ID of the user updating the special</param>
        /// <returns>The updated special</returns>
        Task<SpecialDetail> UpdateSpecialAsync(long id, UpdateSpecialRequest request, string userId);

        /// <summary>
        /// Deletes a special
        /// </summary>
        /// <param name="id">The special ID</param>
        /// <param name="userId">The ID of the user deleting the special</param>
        /// <returns>Success indicator</returns>
        Task<bool> DeleteSpecialAsync(long id, string userId);

        /// <summary>
        /// Determines if a special is currently running
        /// </summary>
        /// <param name="specialId">The special ID</param>
        /// <param name="referenceTime">Optional reference time (defaults to now)</param>
        /// <returns>True if the special is currently running</returns>
        Task<bool> IsSpecialCurrentlyRunningAsync(long specialId, DateTimeOffset? referenceTime = null);
    }
}
