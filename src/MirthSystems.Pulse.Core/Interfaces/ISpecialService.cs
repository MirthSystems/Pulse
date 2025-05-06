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
        Task<PagedApiResponse<SpecialListItem>> GetSpecialsAsync(GetSpecialsRequest request);
        Task<SpecialDetail?> GetSpecialByIdAsync(string id);
        Task<SpecialDetail> CreateSpecialAsync(CreateSpecialRequest request, string userId);
        Task<SpecialDetail> UpdateSpecialAsync(string id, UpdateSpecialRequest request, string userId);
        Task<bool> DeleteSpecialAsync(string id, string userId);
        Task<bool> IsSpecialCurrentlyRunningAsync(string specialId, DateTimeOffset? referenceTime = null);
    }
}
