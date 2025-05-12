namespace MirthSystems.Pulse.Core.Extensions
{
    using MirthSystems.Pulse.Core.Entities;
    using MirthSystems.Pulse.Core.Models.Requests;
    using MirthSystems.Pulse.Core.Models;
    using NodaTime;
    using NetTopologySuite.Geometries;
    using MirthSystems.Pulse.Core.Utilities;

    public static class MappingExtensions
    {
        public static VenueItem MapToVenueListItem(this Venue venue)
        {
            return new VenueItem
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

        public static VenueItemExtended MapToVenueDetail(this Venue venue)
        {
            var detail = new VenueItemExtended
            {
                Id = venue.Id.ToString(),
                Name = venue.Name ?? string.Empty,
                Description = venue.Description ?? string.Empty,
                Locality = venue.Address?.Locality ?? string.Empty,
                Region = venue.Address?.Region ?? string.Empty,
                ProfileImage = venue.ProfileImage ?? string.Empty,
                Latitude = venue.Address?.Location?.Y,
                Longitude = venue.Address?.Location?.X,
                PhoneNumber = venue.PhoneNumber ?? string.Empty,
                Website = venue.Website ?? string.Empty,
                Email = venue.Email ?? string.Empty,
                StreetAddress = venue.Address?.StreetAddress ?? string.Empty,
                SecondaryAddress = venue.Address?.SecondaryAddress ?? string.Empty,
                Postcode = venue.Address?.Postcode ?? string.Empty,
                Country = venue.Address?.Country ?? string.Empty,
                BusinessHours = venue.BusinessHours?.Select(s => s.MapToOperatingScheduleListItem()).ToList() ?? new List<OperatingScheduleItem>(),
                CreatedAt = venue.CreatedAt.ToDateTimeOffset(),
                UpdatedAt = venue.UpdatedAt?.ToDateTimeOffset()
            };

            return detail;
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
                CreatedAt = DateTimeUtility.GetCurrentInstant(),
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
            existingVenue.UpdatedAt = DateTimeUtility.GetCurrentInstant();
            existingVenue.UpdatedByUserId = userId;
            request.Address.MapAndUpdateExistingAddress(existingVenue.Address, geocodedPoint);
            return existingVenue;
        }

        public static OperatingScheduleItem MapToOperatingScheduleListItem(this OperatingSchedule schedule)
        {
            return new OperatingScheduleItem
            {
                Id = schedule.Id.ToString(),
                DayOfWeek = schedule.DayOfWeek,
                DayName = schedule.DayOfWeek.ToString(),
                OpenTime = schedule.TimeOfOpen.ToString("HH:mm", null),
                CloseTime = schedule.TimeOfClose.ToString("HH:mm", null),
                IsClosed = schedule.IsClosed
            };
        }

        public static OperatingScheduleItemExtended MapToOperatingScheduleDetail(this OperatingSchedule schedule, string venueName)
        {
            var detail = new OperatingScheduleItemExtended
            {
                Id = schedule.Id.ToString(),
                DayOfWeek = schedule.DayOfWeek,
                DayName = schedule.DayOfWeek.ToString(),
                OpenTime = schedule.TimeOfOpen.ToString("HH:mm", null),
                CloseTime = schedule.TimeOfClose.ToString("HH:mm", null),
                IsClosed = schedule.IsClosed,
                VenueId = schedule.VenueId.ToString(),
                VenueName = venueName
            };

            return detail;
        }

        public static OperatingSchedule MapToNewOperatingSchedule(this OperatingScheduleItem item, long venueId)
        {
            return new OperatingSchedule
            {
                VenueId = venueId,
                DayOfWeek = item.DayOfWeek,
                TimeOfOpen = DateTimeUtility.FromTimeOnly(TimeOnly.Parse(item.OpenTime)),
                TimeOfClose = DateTimeUtility.FromTimeOnly(TimeOnly.Parse(item.CloseTime)),
                IsClosed = item.IsClosed
            };
        }

        public static OperatingSchedule MapToNewOperatingSchedule(this OperatingHours hours, long venueId)
        {
            return new OperatingSchedule
            {
                VenueId = venueId,
                DayOfWeek = hours.DayOfWeek,
                TimeOfOpen = DateTimeUtility.FromTimeOnly(TimeOnly.Parse(hours.TimeOfOpen)),
                TimeOfClose = DateTimeUtility.FromTimeOnly(TimeOnly.Parse(hours.TimeOfClose)),
                IsClosed = hours.IsClosed
            };
        }

        public static OperatingSchedule MapAndUpdateExistingOperatingSchedule(this UpdateOperatingScheduleRequest request, OperatingSchedule existingSchedule)
        {
            existingSchedule.TimeOfOpen = DateTimeUtility.FromTimeOnly(TimeOnly.Parse(request.TimeOfOpen));
            existingSchedule.TimeOfClose = DateTimeUtility.FromTimeOnly(TimeOnly.Parse(request.TimeOfClose));
            existingSchedule.IsClosed = request.IsClosed;
            return existingSchedule;
        }

        public static SpecialItem MapToSpecialListItem(this Special special, bool isCurrentlyRunning)
        {
            return new SpecialItem
            {
                Id = special.Id.ToString(),
                VenueId = special.VenueId.ToString(),
                Content = special.Content ?? string.Empty,
                Type = special.Type,
                TypeName = special.Type.ToString(),
                StartDate = special.StartDate.ToString("yyyy-MM-dd", null),
                StartTime = special.StartTime.ToString("HH:mm", null),
                EndTime = special.EndTime?.ToString("HH:mm", null),
                IsCurrentlyRunning = isCurrentlyRunning,
                IsRecurring = special.IsRecurring
            };
        }

        public static SpecialItemExtended MapToSpecialDetail(this Special special, bool isCurrentlyRunning)
        {
            var detail = new SpecialItemExtended
            {
                Id = special.Id.ToString(),
                Venue = special.Venue?.MapToVenueListItem() ?? new VenueItem(),
                Content = special.Content ?? string.Empty,
                Type = special.Type,
                TypeName = special.Type.ToString(),
                StartDate = special.StartDate.ToString("yyyy-MM-dd", null),
                StartTime = special.StartTime.ToString("HH:mm", null),
                EndTime = special.EndTime?.ToString("HH:mm", null),
                IsCurrentlyRunning = isCurrentlyRunning,
                IsRecurring = special.IsRecurring,
                ExpirationDate = special.ExpirationDate?.ToString("yyyy-MM-dd", null),
                CronSchedule = special.CronSchedule,
                CreatedAt = special.CreatedAt.ToDateTimeOffset(),
                UpdatedAt = special.UpdatedAt?.ToDateTimeOffset()
            };

            return detail;
        }

        public static Special MapToNewSpecial(this CreateSpecialRequest request, string userId)
        {
            if (!long.TryParse(request.VenueId, out long venueId))
            {
                throw new ArgumentException("Invalid venue ID format");
            }

            LocalDate? expirationDate = string.IsNullOrEmpty(request.ExpirationDate)
                ? null
                : DateTimeUtility.FromDateOnly(DateOnly.Parse(request.ExpirationDate));
            LocalTime? endTime = string.IsNullOrEmpty(request.EndTime)
                ? null
                : DateTimeUtility.FromTimeOnly(TimeOnly.Parse(request.EndTime));

            return new Special
            {
                VenueId = venueId,
                Content = request.Content,
                Type = request.Type,
                StartDate = DateTimeUtility.FromDateOnly(DateOnly.Parse(request.StartDate)),
                StartTime = DateTimeUtility.FromTimeOnly(TimeOnly.Parse(request.StartTime)),
                EndTime = endTime,
                ExpirationDate = expirationDate,
                IsRecurring = request.IsRecurring,
                CronSchedule = request.CronSchedule,
                CreatedAt = DateTimeUtility.GetCurrentInstant(),
                CreatedByUserId = userId
            };
        }

        public static Special MapAndUpdateExistingSpecial(this UpdateSpecialRequest request, Special existingSpecial)
        {
            LocalDate? expirationDate = string.IsNullOrEmpty(request.ExpirationDate)
                ? null
                : DateTimeUtility.FromDateOnly(DateOnly.Parse(request.ExpirationDate));
            LocalTime? endTime = string.IsNullOrEmpty(request.EndTime)
                ? null
                : DateTimeUtility.FromTimeOnly(TimeOnly.Parse(request.EndTime));

            existingSpecial.Content = request.Content;
            existingSpecial.Type = request.Type;
            existingSpecial.StartDate = DateTimeUtility.FromDateOnly(DateOnly.Parse(request.StartDate));
            existingSpecial.StartTime = DateTimeUtility.FromTimeOnly(TimeOnly.Parse(request.StartTime));
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

        // Extension method to convert NodaTime Instant to DateTimeOffset
        public static DateTimeOffset ToDateTimeOffset(this Instant instant)
        {
            return instant.ToDateTimeOffset();
        }
    }
}
