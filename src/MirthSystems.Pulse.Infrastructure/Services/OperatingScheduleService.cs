namespace MirthSystems.Pulse.Infrastructure.Services
{
    using Microsoft.Extensions.Logging;
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models;
    using NodaTime;
    using MirthSystems.Pulse.Core.Extensions;

    public class OperatingScheduleService : IOperatingScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OperatingScheduleService> _logger;

        public OperatingScheduleService(
            IUnitOfWork unitOfWork,
            ILogger<OperatingScheduleService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OperatingScheduleItemExtended?> GetOperatingScheduleByIdAsync(string id)
        {
            try
            {
                if (!long.TryParse(id, out long scheduleId))
                {
                    _logger.LogWarning("Invalid schedule ID format: {Id}", id);
                    return null;
                }
                var schedule = await _unitOfWork.OperatingSchedules.GetByIdAsync(scheduleId);
                if (schedule == null)
                {
                    return null;
                }
                var venue = await _unitOfWork.Venues.GetByIdAsync(schedule.VenueId);
                return schedule.MapToOperatingScheduleDetail(venue?.Name ?? "Unknown Venue");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving operating schedule with ID {ScheduleId}", id);
                return null;
            }
        }

        public async Task<OperatingScheduleItemExtended> CreateOperatingScheduleAsync(CreateOperatingScheduleRequest request, string userId)
        {
            try
            {
                if (!long.TryParse(request.VenueId, out long venueId))
                {
                    throw new ArgumentException("Invalid venue ID format");
                }
                var venue = await _unitOfWork.Venues.GetByIdAsync(venueId);
                if (venue == null)
                {
                    throw new KeyNotFoundException($"Venue with ID {request.VenueId} not found");
                }

                var schedule = new OperatingSchedule
                {
                    VenueId = venueId,
                    DayOfWeek = request.DayOfWeek,
                    TimeOfOpen = LocalTime.FromTimeOnly(TimeOnly.Parse(request.TimeOfOpen)),
                    TimeOfClose = LocalTime.FromTimeOnly(TimeOnly.Parse(request.TimeOfClose)),
                    IsClosed = request.IsClosed
                };

                await _unitOfWork.OperatingSchedules.AddAsync(schedule);
                await _unitOfWork.SaveChangesAsync();

                return schedule.MapToOperatingScheduleDetail(venue.Name ?? "Unknown Venue");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating operating schedule");
                throw;
            }
        }

        public async Task<OperatingScheduleItemExtended> UpdateOperatingScheduleAsync(string id, UpdateOperatingScheduleRequest request, string userId)
        {
            try
            {
                if (!long.TryParse(id, out long scheduleId))
                {
                    throw new ArgumentException("Invalid schedule ID format");
                }
                var schedule = await _unitOfWork.OperatingSchedules.GetByIdAsync(scheduleId);
                if (schedule == null)
                {
                    throw new KeyNotFoundException($"Operating schedule with ID {id} not found");
                }

                request.MapAndUpdateExistingOperatingSchedule(schedule);

                _unitOfWork.OperatingSchedules.Update(schedule);
                await _unitOfWork.SaveChangesAsync();

                var venue = await _unitOfWork.Venues.GetByIdAsync(schedule.VenueId);
                return schedule.MapToOperatingScheduleDetail(venue?.Name ?? "Unknown Venue");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating operating schedule with ID {ScheduleId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteOperatingScheduleAsync(string id, string userId)
        {
            try
            {
                if (!long.TryParse(id, out long scheduleId))
                {
                    _logger.LogWarning("Invalid schedule ID format: {Id}", id);
                    return false;
                }
                return await _unitOfWork.OperatingSchedules.DeleteAsync(scheduleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting operating schedule with ID {ScheduleId}", id);
                return false;
            }
        }
    }
}
