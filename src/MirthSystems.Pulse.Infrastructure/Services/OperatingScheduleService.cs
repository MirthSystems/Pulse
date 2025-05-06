namespace MirthSystems.Pulse.Infrastructure.Services
{
    using Microsoft.Extensions.Logging;
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Interfaces;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models;
    using NodaTime;

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

        public async Task<OperatingScheduleDetail?> GetOperatingScheduleByIdAsync(long id)
        {
            try
            {
                var schedule = await _unitOfWork.OperatingSchedules.GetByIdAsync(id);
                if (schedule == null)
                {
                    return null;
                }

                var venue = await _unitOfWork.Venues.GetByIdAsync(schedule.VenueId);
                return MapToOperatingScheduleDetail(schedule, venue?.Name ?? "Unknown Venue");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving operating schedule with ID {ScheduleId}", id);
                return null;
            }
        }

        public async Task<OperatingScheduleDetail> CreateOperatingScheduleAsync(CreateOperatingScheduleRequest request, string userId)
        {
            try
            {
                // Verify venue exists
                var venue = await _unitOfWork.Venues.GetByIdAsync(request.VenueId);
                if (venue == null)
                {
                    throw new KeyNotFoundException($"Venue with ID {request.VenueId} not found");
                }

                // Parse times
                var timeOfOpen = LocalTime.FromTimeOnly(TimeOnly.Parse(request.TimeOfOpen));
                var timeOfClose = LocalTime.FromTimeOnly(TimeOnly.Parse(request.TimeOfClose));

                // Create new operating schedule
                var schedule = new OperatingSchedule
                {
                    VenueId = request.VenueId,
                    DayOfWeek = (DayOfWeek)request.DayOfWeek,
                    TimeOfOpen = timeOfOpen,
                    TimeOfClose = timeOfClose,
                    IsClosed = request.IsClosed,
                };

                await _unitOfWork.OperatingSchedules.AddAsync(schedule);
                await _unitOfWork.SaveChangesAsync();

                return MapToOperatingScheduleDetail(schedule, venue.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating operating schedule");
                throw;
            }
        }

        public async Task<OperatingScheduleDetail> UpdateOperatingScheduleAsync(long id, UpdateOperatingScheduleRequest request, string userId)
        {
            var schedule = await _unitOfWork.OperatingSchedules.GetByIdAsync(id);
            if (schedule == null)
            {
                throw new KeyNotFoundException($"Operating schedule with ID {id} not found");
            }

            try
            {
                // Parse times
                var timeOfOpen = LocalTime.FromTimeOnly(TimeOnly.Parse(request.TimeOfOpen));
                var timeOfClose = LocalTime.FromTimeOnly(TimeOnly.Parse(request.TimeOfClose));
                schedule.TimeOfOpen = timeOfOpen;
                schedule.TimeOfClose = timeOfClose;
                schedule.IsClosed = request.IsClosed;

                _unitOfWork.OperatingSchedules.Update(schedule);
                await _unitOfWork.SaveChangesAsync();

                // Get venue name
                var venue = await _unitOfWork.Venues.GetByIdAsync(schedule.VenueId);
                return MapToOperatingScheduleDetail(schedule, venue?.Name ?? "Unknown Venue");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating operating schedule with ID {ScheduleId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteOperatingScheduleAsync(long id, string userId)
        {
            try
            {
                return await _unitOfWork.OperatingSchedules.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting operating schedule with ID {ScheduleId}", id);
                return false;
            }
        }

        public async Task<List<OperatingScheduleDetail>> GetVenueOperatingSchedulesAsync(long venueId)
        {
            try
            {
                var venue = await _unitOfWork.Venues.GetByIdAsync(venueId);
                if (venue == null)
                {
                    return new List<OperatingScheduleDetail>();
                }

                var schedules = await _unitOfWork.OperatingSchedules.GetSchedulesByVenueIdAsync(venueId);
                return schedules.Select(s => MapToOperatingScheduleDetail(s, venue.Name)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving operating schedules for venue with ID {VenueId}", venueId);
                return new List<OperatingScheduleDetail>();
            }
        }

        public async Task<bool> CreateOperatingSchedulesForVenueAsync(long venueId, List<CreateOperatingScheduleRequest> requests, string userId)
        {
            try
            {
                // Verify venue exists
                var venue = await _unitOfWork.Venues.GetByIdAsync(venueId);
                if (venue == null)
                {
                    throw new KeyNotFoundException($"Venue with ID {venueId} not found");
                }

                // Get existing schedules
                var existingSchedules = await _unitOfWork.OperatingSchedules.GetSchedulesByVenueIdAsync(venueId);

                // Delete existing schedules
                foreach (var schedule in existingSchedules)
                {
                    await _unitOfWork.OperatingSchedules.DeleteAsync(schedule.Id);
                }

                // Create new schedules
                foreach (var request in requests)
                {
                    var timeOfOpen = LocalTime.FromTimeOnly(TimeOnly.Parse(request.TimeOfOpen));
                    var timeOfClose = LocalTime.FromTimeOnly(TimeOnly.Parse(request.TimeOfClose));

                    var schedule = new OperatingSchedule
                    {
                        VenueId = venueId,
                        DayOfWeek = (DayOfWeek)request.DayOfWeek,
                        TimeOfOpen = timeOfOpen,
                        TimeOfClose = timeOfClose,
                        IsClosed = request.IsClosed,
                    };

                    await _unitOfWork.OperatingSchedules.AddAsync(schedule);
                }

                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating operating schedules for venue with ID {VenueId}", venueId);
                return false;
            }
        }

        #region Mapping Methods

        private OperatingScheduleDetail MapToOperatingScheduleDetail(OperatingSchedule schedule, string venueName)
        {
            return new OperatingScheduleDetail
            {
                Id = schedule.Id,
                VenueId = schedule.VenueId,
                VenueName = venueName,
                DayOfWeek = schedule.DayOfWeek,
                DayName = schedule.DayOfWeek.ToString(),
                OpenTime = schedule.TimeOfOpen.ToString("HH:mm", null),
                CloseTime = schedule.TimeOfClose.ToString("HH:mm", null),
                IsClosed = schedule.IsClosed
            };
        }

        #endregion
    }
}
