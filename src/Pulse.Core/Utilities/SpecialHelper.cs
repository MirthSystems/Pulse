namespace Pulse.Core.Utilities
{
    using System.Runtime.CompilerServices;

    using NodaTime;

    using Pulse.Core.Models.Entities;

    /// <summary>
    /// Helper utility for Special entity operations
    /// </summary>
    public static class SpecialHelper
    {
        private static readonly Dictionary<DayOfWeek, int> _dayOfWeekBits = new()
        {
            { DayOfWeek.Sunday, 1 << (int)DayOfWeek.Sunday },
            { DayOfWeek.Monday, 1 << (int)DayOfWeek.Monday },
            { DayOfWeek.Tuesday, 1 << (int)DayOfWeek.Tuesday },
            { DayOfWeek.Wednesday, 1 << (int)DayOfWeek.Wednesday },
            { DayOfWeek.Thursday, 1 << (int)DayOfWeek.Thursday },
            { DayOfWeek.Friday, 1 << (int)DayOfWeek.Friday },
            { DayOfWeek.Saturday, 1 << (int)DayOfWeek.Saturday }
        };

        /// <summary>
        /// Determines if a special is currently active based on its properties and recurrence pattern
        /// </summary>
        /// <param name="special">The special to check</param>
        /// <param name="currentInstant">The current instant to check against</param>
        /// <returns>True if the special is active, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsActive(Special special, Instant currentInstant)
        {
            if (special == null)
            {
                return false;
            }

            var dateTime = currentInstant.ToDateTimeUtc();
            var currentDate = LocalDate.FromDateTime(dateTime);
            var currentTime = LocalTime.FromTimeOnly(new TimeOnly(dateTime.Hour, dateTime.Minute));
            var dayOfWeek = dateTime.DayOfWeek;

            if (special.StartDate > currentDate ||
               (special.ExpirationDate != null && special.ExpirationDate < currentDate))
            {
                return false;
            }

            if (special.StartTime > currentTime ||
               (special.EndTime != null && special.EndTime < currentTime))
            {
                return false;
            }

            if (!special.IsRecurring)
            {
                return true;
            }

            if (special.ActiveDaysOfWeek.HasValue && special.ActiveDaysOfWeek.Value != 0)
            {
                int dayBit = _dayOfWeekBits[dayOfWeek];

                if ((special.ActiveDaysOfWeek.Value & dayBit) == 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if multiple specials are active at the given instant
        /// </summary>
        /// <param name="specials">Collection of specials to check</param>
        /// <param name="currentInstant">The current instant to check against</param>
        /// <returns>Collection of active specials</returns>
        public static IEnumerable<Special> GetActiveSpecials(IEnumerable<Special> specials, Instant currentInstant)
        {
            if (specials == null)
            {
                yield break;
            }

            var dateTime = currentInstant.ToDateTimeUtc();
            var currentDate = LocalDate.FromDateTime(dateTime);
            var currentTime = LocalTime.FromTimeOnly(new TimeOnly(dateTime.Hour, dateTime.Minute));
            var dayOfWeek = dateTime.DayOfWeek;
            int dayBit = _dayOfWeekBits[dayOfWeek];

            foreach (var special in specials)
            {
                if (special == null)
                {
                    continue;
                }

                if (special.StartDate > currentDate ||
                    (special.ExpirationDate != null && special.ExpirationDate < currentDate))
                {
                    continue;
                }

                if (special.StartTime > currentTime ||
                    (special.EndTime != null && special.EndTime < currentTime))
                {
                    continue;
                }

                if (special.IsRecurring &&
                    special.ActiveDaysOfWeek.HasValue &&
                    special.ActiveDaysOfWeek.Value != 0 &&
                    (special.ActiveDaysOfWeek.Value & dayBit) == 0)
                {
                    continue;
                }

                yield return special;
            }
        }

        /// <summary>
        /// Day of week bitmask values for ActiveDaysOfWeek property
        /// </summary>
        public static class DayBits
        {
            public const int Sunday = 1 << (int)DayOfWeek.Sunday;
            public const int Monday = 1 << (int)DayOfWeek.Monday;
            public const int Tuesday = 1 << (int)DayOfWeek.Tuesday;
            public const int Wednesday = 1 << (int)DayOfWeek.Wednesday;
            public const int Thursday = 1 << (int)DayOfWeek.Thursday;
            public const int Friday = 1 << (int)DayOfWeek.Friday;
            public const int Saturday = 1 << (int)DayOfWeek.Saturday;

            public const int Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday;
            public const int Weekend = Sunday | Saturday;
            public const int AllDays = Weekdays | Weekend;
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
