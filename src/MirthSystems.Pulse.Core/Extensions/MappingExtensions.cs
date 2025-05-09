namespace MirthSystems.Pulse.Core.Extensions
{
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models;
    using NodaTime;
    using NetTopologySuite.Geometries;

    public static class MappingExtensions
    {
        public static VenueListItem MapToVenueListItem(this Venue venue)
        {
            return new VenueListItem
            {
                Id = venue.Id.ToString(),
                Name = venue.Name ?? string.Empty,
                Description = venue.Description ?? string.Empty,
                Locality = venue.Address?.Locality ?? string.Empty,
                Region = venue.Address?.Region ?? string.Empty,
                ProfileImage = venue.ProfileImage ?? string.Empty,
                Latitude = venue.Address?.Location?.Y,
                Longitude = venue.Address?.Location?.X
            };
        }

        public static VenueDetail MapToVenueDetail(this Venue venue)
        {
            return new VenueDetail
            {
                Id = venue.Id.ToString(),
                Name = venue.Name ?? string.Empty,
                Description = venue.Description ?? string.Empty,
                PhoneNumber = venue.PhoneNumber ?? string.Empty,
                Website = venue.Website ?? string.Empty,
                Email = venue.Email ?? string.Empty,
                ProfileImage = venue.ProfileImage ?? string.Empty,
                StreetAddress = venue.Address?.StreetAddress ?? string.Empty,
                SecondaryAddress = venue.Address?.SecondaryAddress ?? string.Empty,
                Locality = venue.Address?.Locality ?? string.Empty,
                Region = venue.Address?.Region ?? string.Empty,
                Postcode = venue.Address?.Postcode ?? string.Empty,
                Country = venue.Address?.Country ?? string.Empty,
                Latitude = venue.Address?.Location?.Y,
                Longitude = venue.Address?.Location?.X,
                BusinessHours = venue.BusinessHours?.Select(s => s.MapToOperatingScheduleListItem()).ToList() ?? new List<OperatingScheduleListItem>(),
                CreatedAt = venue.CreatedAt.ToDateTimeOffset(),
                UpdatedAt = venue.UpdatedAt?.ToDateTimeOffset()
            };
        }

        public static Venue MapToNewVenue(this CreateVenueRequest request, string userId, Point geocodedPoint)
        {
            return new Venue
            {
                Name = request.Name,
                Description = request.Description,
                PhoneNumber = request.PhoneNumber,
                Website = request.Website,
                Email = request.Email,
                ProfileImage = request.ProfileImage,
                CreatedAt = SystemClock.Instance.GetCurrentInstant(),
                CreatedByUserId = userId,
                Address = request.Address.MapToNewAddress(geocodedPoint),
            };
        }

        public static Venue MapAndUpdateExistingVenue(this UpdateVenueRequest request, Venue existingVenue, string userId, Point geocodedPoint)
        {
            existingVenue.Name = request.Name;
            existingVenue.Description = request.Description;
            existingVenue.PhoneNumber = request.PhoneNumber;
            existingVenue.Website = request.Website;
            existingVenue.Email = request.Email;
            existingVenue.ProfileImage = request.ProfileImage;
            existingVenue.UpdatedAt = SystemClock.Instance.GetCurrentInstant();
            existingVenue.UpdatedByUserId = userId;
            request.Address.MapAndUpdateExistingAddress(existingVenue.Address, geocodedPoint);
            return existingVenue;
        }

        public static OperatingScheduleListItem MapToOperatingScheduleListItem(this OperatingSchedule schedule)
        {
            return new OperatingScheduleListItem
            {
                Id = schedule.Id.ToString(),
                DayOfWeek = schedule.DayOfWeek,
                DayName = schedule.DayOfWeek.ToString(),
                OpenTime = schedule.TimeOfOpen.ToString("HH:mm", null),
                CloseTime = schedule.TimeOfClose.ToString("HH:mm", null),
                IsClosed = schedule.IsClosed
            };
        }

        public static OperatingScheduleDetail MapToOperatingScheduleDetail(this OperatingSchedule schedule, string venueName)
        {
            return new OperatingScheduleDetail
            {
                Id = schedule.Id.ToString(),
                VenueId = schedule.VenueId.ToString(),
                VenueName = venueName,
                DayOfWeek = schedule.DayOfWeek,
                DayName = schedule.DayOfWeek.ToString(),
                OpenTime = schedule.TimeOfOpen.ToString("HH:mm", null),
                CloseTime = schedule.TimeOfClose.ToString("HH:mm", null),
                IsClosed = schedule.IsClosed
            };
        }

        public static OperatingSchedule MapToNewOperatingSchedule(this OperatingHours request, long venueId)
        {
            return new OperatingSchedule
            {
                VenueId = venueId,
                DayOfWeek = request.DayOfWeek,
                TimeOfOpen = LocalTime.FromTimeOnly(TimeOnly.Parse(request.TimeOfOpen)),
                TimeOfClose = LocalTime.FromTimeOnly(TimeOnly.Parse(request.TimeOfClose)),
                IsClosed = request.IsClosed
            };
        }

        public static OperatingSchedule MapAndUpdateExistingOperatingSchedule(this UpdateOperatingScheduleRequest request, OperatingSchedule existingSchedule)
        {
            existingSchedule.TimeOfOpen = LocalTime.FromTimeOnly(TimeOnly.Parse(request.TimeOfOpen));
            existingSchedule.TimeOfClose = LocalTime.FromTimeOnly(TimeOnly.Parse(request.TimeOfClose));
            existingSchedule.IsClosed = request.IsClosed;
            return existingSchedule;
        }

        public static SpecialListItem MapToSpecialListItem(this Special special, bool isCurrentlyRunning)
        {
            return new SpecialListItem
            {
                Id = special.Id.ToString(),
                VenueId = special.VenueId.ToString(),
                VenueName = special.Venue?.Name ?? string.Empty,
                Content = special.Content ?? string.Empty,
                Type = special.Type,
                TypeName = special.Type.ToString(),
                StartDate = special.StartDate.ToString("yyyy-MM-dd", null),
                StartTime = special.StartTime.ToString("HH:mm", null),
                EndTime = special.EndTime?.ToString("HH:mm", null) ?? string.Empty,
                IsCurrentlyRunning = isCurrentlyRunning,
                IsRecurring = special.IsRecurring
            };
        }

        public static SpecialDetail MapToSpecialDetail(this Special special, bool isCurrentlyRunning)
        {
            return new SpecialDetail
            {
                Id = special.Id.ToString(),
                VenueId = special.VenueId.ToString(),
                VenueName = special.Venue?.Name ?? string.Empty,
                Content = special.Content ?? string.Empty,
                Type = special.Type,
                TypeName = special.Type.ToString(),
                StartDate = special.StartDate.ToString("yyyy-MM-dd", null),
                StartTime = special.StartTime.ToString("HH:mm", null),
                EndTime = special.EndTime?.ToString("HH:mm", null) ?? string.Empty,
                ExpirationDate = special.ExpirationDate?.ToString("yyyy-MM-dd", null) ?? string.Empty,
                IsRecurring = special.IsRecurring,
                CronSchedule = special.CronSchedule ?? string.Empty,
                IsCurrentlyRunning = isCurrentlyRunning,
                CreatedAt = special.CreatedAt.ToDateTimeOffset(),
                UpdatedAt = special.UpdatedAt?.ToDateTimeOffset()
            };
        }

        public static Special MapToNewSpecial(this CreateSpecialRequest request, string userId)
        {
            if (!long.TryParse(request.VenueId, out long venueId))
            {
                throw new ArgumentException("Invalid venue ID format");
            }

            LocalDate? expirationDate = string.IsNullOrEmpty(request.ExpirationDate)
                ? null
                : LocalDate.FromDateOnly(DateOnly.Parse(request.ExpirationDate));

            LocalTime? endTime = string.IsNullOrEmpty(request.EndTime)
                ? null
                : LocalTime.FromTimeOnly(TimeOnly.Parse(request.EndTime));

            return new Special
            {
                VenueId = venueId,
                Content = request.Content,
                Type = request.Type,
                StartDate = LocalDate.FromDateOnly(DateOnly.Parse(request.StartDate)),
                StartTime = LocalTime.FromTimeOnly(TimeOnly.Parse(request.StartTime)),
                EndTime = endTime,
                ExpirationDate = expirationDate,
                IsRecurring = request.IsRecurring,
                CronSchedule = request.CronSchedule,
                CreatedAt = SystemClock.Instance.GetCurrentInstant(),
                CreatedByUserId = userId
            };
        }

        public static Special MapAndUpdateExistingSpecial(this UpdateSpecialRequest request, Special existingSpecial)
        {
            LocalDate? expirationDate = string.IsNullOrEmpty(request.ExpirationDate)
                ? null
                : LocalDate.FromDateOnly(DateOnly.Parse(request.ExpirationDate));

            LocalTime? endTime = string.IsNullOrEmpty(request.EndTime)
                ? null
                : LocalTime.FromTimeOnly(TimeOnly.Parse(request.EndTime));

            existingSpecial.Content = request.Content;
            existingSpecial.Type = request.Type;
            existingSpecial.StartDate = LocalDate.FromDateOnly(DateOnly.Parse(request.StartDate));
            existingSpecial.StartTime = LocalTime.FromTimeOnly(TimeOnly.Parse(request.StartTime));
            existingSpecial.EndTime = endTime;
            existingSpecial.ExpirationDate = expirationDate;
            existingSpecial.IsRecurring = request.IsRecurring;
            existingSpecial.CronSchedule = request.CronSchedule;

            return existingSpecial;
        }

        public static Address MapToNewAddress(this AddressRequest request, Point geocodedPoint)
        {
            return new Address
            {
                StreetAddress = request.StreetAddress,
                SecondaryAddress = request.SecondaryAddress,
                Locality = request.Locality,
                Region = request.Region,
                Postcode = request.Postcode,
                Country = request.Country,
                Location = geocodedPoint
            };
        }

        public static Address MapAndUpdateExistingAddress(this AddressRequest request, Address existingAddress, Point geocodedPoint)
        {
            existingAddress.StreetAddress = request.StreetAddress;
            existingAddress.SecondaryAddress = request.SecondaryAddress;
            existingAddress.Locality = request.Locality;
            existingAddress.Region = request.Region;
            existingAddress.Postcode = request.Postcode;
            existingAddress.Country = request.Country;
            existingAddress.Location = geocodedPoint;
            return existingAddress;
        }
    }
}
