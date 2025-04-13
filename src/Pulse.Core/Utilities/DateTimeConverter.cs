namespace Pulse.Core.Utilities
{
    using NodaTime;

    public static class DateTimeConverter
    {
        public static DateTime? ToDateTime(LocalDate? date)
        {
            if (date == null)
                return null;

            return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day);
        }

        public static DateTime ToDateTime(LocalDate date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        public static LocalDate? ToLocalDate(DateTime? dateTime)
        {
            if (dateTime == null)
                return null;

            return LocalDate.FromDateTime(dateTime.Value);
        }

        public static LocalDate ToLocalDate(DateTime dateTime)
        {
            return LocalDate.FromDateTime(dateTime);
        }

        public static DateTime? ToDateTime(LocalTime? time)
        {
            if (time == null)
                return null;

            var today = DateTime.Today;
            return today.Add(new TimeSpan(time.Value.Hour, time.Value.Minute, time.Value.Second));
        }

        public static DateTime ToDateTime(LocalTime time)
        {
            var today = DateTime.Today;
            return today.Add(new TimeSpan(time.Hour, time.Minute, time.Second));
        }

        public static LocalTime? ToLocalTime(DateTime? dateTime)
        {
            if (dateTime == null)
                return null;

            return LocalTime.FromTicksSinceMidnight(dateTime.Value.TimeOfDay.Ticks);
        }

        public static LocalTime ToLocalTime(DateTime dateTime)
        {
            return LocalTime.FromTicksSinceMidnight(dateTime.TimeOfDay.Ticks);
        }
    }
}
