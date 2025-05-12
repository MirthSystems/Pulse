using System;
using System.Threading.Tasks;
using Cronos;
using MirthSystems.Pulse.Core.Entities;
using NodaTime;

namespace MirthSystems.Pulse.Core.Utilities
{
    /// <summary>
    /// Utility class for determining if a special is currently active.
    /// </summary>
    /// <remarks>
    /// <para>This class provides static methods to check whether special promotions are active at a given time.</para>
    /// <para>It handles complex logic like recurrence patterns and time intervals crossing midnight.</para>
    /// </remarks>
    public static class SpecialActivityUtility
    {
        /// <summary>
        /// Determines if a special is active at the specified reference time.
        /// </summary>
        /// <param name="special">The special to check.</param>
        /// <param name="referenceTime">The reference instant in time (typically now).</param>
        /// <returns>True if the special is currently active; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method performs complex time-based calculations to determine if a special is active.</para>
        /// <para>It handles one-time specials, recurring specials with CRON schedules, and various edge cases.</para>
        /// <para>The logic accounts for:</para>
        /// <para>- Start dates and times</para>
        /// <para>- End times (including those that cross midnight)</para>
        /// <para>- Expiration dates</para>
        /// <para>- CRON-based recurrence patterns</para>
        /// </remarks>
        public static bool IsSpecialActive(Special special, Instant referenceTime)
        {
            if (special == null)
            {
                return false;
            }

            var localDateTime = referenceTime.InUtc().ToDateTimeUtc();
            var localDate = LocalDate.FromDateTime(localDateTime.Date);
            var localTime = LocalTime.FromTicksSinceMidnight(localDateTime.TimeOfDay.Ticks);

            // Check expiration date
            if (special.ExpirationDate.HasValue && special.ExpirationDate.Value < localDate)
            {
                return false;
            }

            // Check start date
            if (special.StartDate > localDate)
            {
                return false;
            }

            // Check non-recurring special for the exact day
            if (!special.IsRecurring && special.StartDate == localDate)
            {
                if (special.EndTime.HasValue)
                {
                    // If start time is before end time (same day)
                    if (special.StartTime <= special.EndTime)
                    {
                        return localTime >= special.StartTime && localTime <= special.EndTime;
                    }
                    // If start time is after end time (spans midnight)
                    else
                    {
                        return localTime >= special.StartTime || localTime <= special.EndTime;
                    }
                }
                else
                {
                    // Without end time, just check if we're past start time
                    return localTime >= special.StartTime;
                }
            }

            // Check recurring special
            if (special.IsRecurring && !string.IsNullOrEmpty(special.CronSchedule))
            {
                try
                {
                    var cronExpression = CronExpression.Parse(special.CronSchedule);

                    // Get previous occurrence that would have started this special (within last 24h)
                    var previous = cronExpression.GetNextOccurrence(
                        referenceTime.Minus(Duration.FromHours(24)).ToDateTimeUtc(),
                        TimeZoneInfo.Utc,
                        inclusive: false);

                    if (previous.HasValue)
                    {
                        // Calculate when this occurrence would end
                        var occurrenceStart = Instant.FromDateTimeUtc(DateTime.SpecifyKind(previous.Value, DateTimeKind.Utc));
                        var startTimeOfDay = new TimeSpan(special.StartTime.Hour, special.StartTime.Minute, special.StartTime.Second);
                        var occurrenceStartWithTime = occurrenceStart.Plus(Duration.FromTimeSpan(startTimeOfDay));

                        // If no end time, special runs until next occurrence or for 24 hours
                        if (!special.EndTime.HasValue)
                        {
                            return referenceTime >= occurrenceStartWithTime &&
                                   referenceTime <= occurrenceStartWithTime.Plus(Duration.FromHours(24));
                        }

                        // Calculate end time for this occurrence
                        var endTimeOfDay = new TimeSpan(special.EndTime.Value.Hour, special.EndTime.Value.Minute, special.EndTime.Value.Second);
                        var occurrenceEndWithTime = occurrenceStart.Plus(Duration.FromTimeSpan(endTimeOfDay));

                        // Handle case where end time is earlier than start time (spans midnight)
                        if (special.EndTime.Value < special.StartTime)
                        {
                            occurrenceEndWithTime = occurrenceEndWithTime.Plus(Duration.FromHours(24));
                        }

                        // Check if current time falls between start and end
                        return referenceTime >= occurrenceStartWithTime && referenceTime <= occurrenceEndWithTime;
                    }
                }
                catch (Exception)
                {
                    // Invalid CRON expression
                    return false;
                }
            }

            return false; // Default case - not active
        }
    }
}