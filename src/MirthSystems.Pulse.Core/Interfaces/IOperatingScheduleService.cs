namespace MirthSystems.Pulse.Core.Interfaces
{
    using MirthSystems.Pulse.Core.Models.Requests;

    using MirthSystems.Pulse.Core.Models;
    using MirthSystems.Pulse.Core.Models.Responses;

    public interface IOperatingScheduleService
    {
        Task<OperatingScheduleDetail?> GetOperatingScheduleByIdAsync(string id);
        Task<OperatingScheduleDetail> CreateOperatingScheduleAsync(CreateOperatingScheduleRequest request, string userId);
        Task<OperatingScheduleDetail> UpdateOperatingScheduleAsync(string id, UpdateOperatingScheduleRequest request, string userId);
        Task<bool> DeleteOperatingScheduleAsync(string id, string userId);
        Task<List<OperatingScheduleDetail>> GetVenueOperatingSchedulesAsync(string venueId);
        Task<bool> CreateOperatingSchedulesForVenueAsync(string venueId, List<CreateOperatingScheduleRequest> requests, string userId);
    }
}
