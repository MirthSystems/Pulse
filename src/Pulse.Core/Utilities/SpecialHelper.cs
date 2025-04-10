namespace Pulse.Core.Utilities
{
    using NodaTime;

    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Helper utility for Special entity operations
    /// </summary>
    public static class SpecialHelper
    {
        /// <summary>
        /// Determines if a special is currently active based on its properties and recurrence pattern
        /// </summary>
        /// <param name="special">The special to check</param>
        /// <param name="currentInstant">The current instant to check against</param>
        /// <returns>True if the special is active, false otherwise</returns>
        public static bool IsActive(Special special, Instant currentInstant)
        {
            // Convert to UTC DateTime for consistent comparison
            var dateTime = currentInstant.ToDateTimeUtc();
            var currentDate = LocalDate.FromDateTime(dateTime);
            var currentTime = LocalTime.FromTimeOnly(new TimeOnly(dateTime.Hour, dateTime.Minute));
            var dayOfWeek = dateTime.DayOfWeek;

            // One-time specials
            if (!special.IsRecurring)
            {
                return special.StartDate <= currentDate &&
                    (special.ExpirationDate == null || special.ExpirationDate >= currentDate) &&
                    special.StartTime <= currentTime &&
                    (special.EndTime == null || special.EndTime >= currentTime);
            }

            // Recurring specials
            if (special.IsRecurring && special.RecurringPeriod != null)
            {
                // Basic check for start/end dates
                if (special.StartDate > currentDate ||
                    (special.ExpirationDate != null && special.ExpirationDate < currentDate))
                {
                    return false;
                }

                // Check active days of week if specified
                if (special.ActiveDaysOfWeek.HasValue && special.ActiveDaysOfWeek.Value != 0)
                {
                    // Calculate bit position for current day (Sunday=0 to Saturday=6 in .NET)
                    int dayBit = 1 << ((int)dayOfWeek);

                    // Check if current day's bit is set in the bitmask
                    if ((special.ActiveDaysOfWeek.Value & dayBit) == 0)
                    {
                        return false;
                    }
                }

                // Check time of day
                return special.StartTime <= currentTime &&
                    (special.EndTime == null || special.EndTime >= currentTime);
            }

            return false;
        }

        /// <summary>
        /// Day of week bitmask values for ActiveDaysOfWeek property
        /// </summary>
        public static class DayBits
        {
            // Pre-calculated bitmasks for each day of week
            public const int Sunday = 1 << (int)DayOfWeek.Sunday;       // 1
            public const int Monday = 1 << (int)DayOfWeek.Monday;       // 2
            public const int Tuesday = 1 << (int)DayOfWeek.Tuesday;     // 4
            public const int Wednesday = 1 << (int)DayOfWeek.Wednesday; // 8
            public const int Thursday = 1 << (int)DayOfWeek.Thursday;   // 16
            public const int Friday = 1 << (int)DayOfWeek.Friday;       // 32
            public const int Saturday = 1 << (int)DayOfWeek.Saturday;   // 64

            // Common combinations
            public const int Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday;   // 62
            public const int Weekend = Sunday | Saturday;                                  // 65
            public const int AllDays = Weekdays | Weekend;                                // 127
        }

        /// <summary>
        /// Common recurrence period patterns
        /// </summary>
        public static class RecurrencePeriods
        {
            public static readonly Period Daily = Period.FromDays(1);
            public static readonly Period Weekly = Period.FromDays(7);
            public static readonly Period BiWeekly = Period.FromDays(14);
            public static readonly Period Monthly = Period.FromMonths(1);
            public static readonly Period Quarterly = Period.FromMonths(3);
            public static readonly Period Yearly = Period.FromYears(1);
        }
    }
}
