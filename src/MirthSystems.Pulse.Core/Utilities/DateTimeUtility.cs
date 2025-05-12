namespace MirthSystems.Pulse.Core.Utilities
{
    using NodaTime;
    using System;

    /// <summary>
    /// Provides utility methods for common date and time operations.
    /// </summary>
    /// <remarks>
    /// <para>This static utility class centralizes common date and time conversion operations used throughout the system.</para>
    /// <para>It provides standardized methods for converting between .NET DateTime types and NodaTime types.</para>
    /// <para>Using this utility ensures consistency in how date and time values are handled across the application.</para>
    /// </remarks>
    public static class DateTimeUtility
    {
        /// <summary>
        /// Gets the current instant in time.
        /// </summary>
        /// <returns>The current instant from the system clock.</returns>
        public static Instant GetCurrentInstant()
        {
            return SystemClock.Instance.GetCurrentInstant();
        }

        /// <summary>
        /// Gets the current local date (today).
        /// </summary>
        /// <returns>The current date without time component.</returns>
        public static LocalDate GetCurrentLocalDate()
        {
            return LocalDate.FromDateTime(DateTime.Today);
        }

        /// <summary>
        /// Gets the current local time.
        /// </summary>
        /// <returns>The current time without date component.</returns>
        public static LocalTime GetCurrentLocalTime()
        {
            var now = DateTime.Now;
            return new LocalTime(now.Hour, now.Minute, now.Second);
        }

        /// <summary>
        /// Converts an Instant to LocalDate.
        /// </summary>
        /// <param name="instant">The instant to convert.</param>
        /// <returns>The local date component of the instant in UTC.</returns>
        public static LocalDate InstantToLocalDate(Instant instant)
        {
            return LocalDate.FromDateTime(instant.ToDateTimeUtc().Date);
        }

        /// <summary>
        /// Converts an Instant to LocalTime.
        /// </summary>
        /// <param name="instant">The instant to convert.</param>
        /// <returns>The local time component of the instant in UTC.</returns>
        public static LocalTime InstantToLocalTime(Instant instant)
        {
            var dateTime = instant.ToDateTimeUtc();
            return new LocalTime(dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        /// <summary>
        /// Parses a time string to LocalTime.
        /// </summary>
        /// <param name="timeString">The time string in format HH:mm or HH:mm:ss.</param>
        /// <returns>The parsed LocalTime, or null if parsing fails.</returns>
        public static LocalTime? ParseLocalTime(string timeString)
        {
            if (TimeOnly.TryParse(timeString, out TimeOnly time))
            {
                return new LocalTime(time.Hour, time.Minute, time.Second);
            }
            return null;
        }

        /// <summary>
        /// Parses a DateTimeOffset string to Instant.
        /// </summary>
        /// <param name="dateTimeString">The DateTimeOffset string.</param>
        /// <returns>The parsed Instant, or null if parsing fails.</returns>
        public static Instant? ParseInstant(string dateTimeString)
        {
            if (DateTimeOffset.TryParse(dateTimeString, out var parsedDateTime))
            {
                return Instant.FromDateTimeOffset(parsedDateTime);
            }
            return null;
        }

        /// <summary>
        /// Converts a TimeOnly to LocalTime.
        /// </summary>
        /// <param name="time">The TimeOnly value to convert.</param>
        /// <returns>A NodaTime LocalTime equivalent.</returns>
        public static LocalTime FromTimeOnly(TimeOnly time)
        {
            return new LocalTime(time.Hour, time.Minute, time.Second);
        }

        /// <summary>
        /// Converts a DateOnly to LocalDate.
        /// </summary>
        /// <param name="date">The DateOnly value to convert.</param>
        /// <returns>A NodaTime LocalDate equivalent.</returns>
        public static LocalDate FromDateOnly(DateOnly date)
        {
            return new LocalDate(date.Year, date.Month, date.Day);
        }
    }
}