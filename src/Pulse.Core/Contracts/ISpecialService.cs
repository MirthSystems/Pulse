namespace Pulse.Core.Contracts
{
    using Pulse.Core.Models.Requests;
    using Pulse.Core.Models.Responses;

    public interface ISpecialService
    {
        Task<SpecialListResponse> GetSpecialsAsync(SpecialQueryRequest request);
        Task<SpecialDetailResponse?> GetSpecialByIdAsync(int id);
        Task<NewSpecialResponse> CreateSpecialAsync(NewSpecialRequest request, string userId);
        Task<UpdateSpecialResponse?> UpdateSpecialAsync(int id, UpdateSpecialRequest request, string userId);
        Task<bool> DeleteSpecialAsync(int id);
        Task<SpecialTypeListResponse> GetSpecialTypesAsync();
        Task<TagListResponse> GetTagsAsync(string? searchTerm);
    }
}
