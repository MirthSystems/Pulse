namespace MirthSystems.Pulse.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using MirthSystems.Pulse.Core.Entities;

    public class BusinessHours(Venue venue)
    {
        public OperatingSchedule Sunday = new OperatingSchedule
        {
            DayOfWeek = DayOfWeek.Sunday,
            TimeOfOpen = new NodaTime.LocalTime(),
            TimeOfClose = new NodaTime.LocalTime(),
            IsClosed = true,
            Venue = venue
        };
        public OperatingSchedule Monday = new OperatingSchedule
        {
            DayOfWeek = DayOfWeek.Monday,
            TimeOfOpen = new NodaTime.LocalTime(),
            TimeOfClose = new NodaTime.LocalTime(),
            IsClosed = true,
            Venue = venue
        };
        public OperatingSchedule Tuesday = new OperatingSchedule
        {
            DayOfWeek = DayOfWeek.Tuesday,
            TimeOfOpen = new NodaTime.LocalTime(),
            TimeOfClose = new NodaTime.LocalTime(),
            IsClosed = true,
            Venue = venue
        };
        public OperatingSchedule Wednesday = new OperatingSchedule
        {
            DayOfWeek = DayOfWeek.Wednesday,
            TimeOfOpen = new NodaTime.LocalTime(),
            TimeOfClose = new NodaTime.LocalTime(),
            IsClosed = true,
            Venue = venue
        };
        public OperatingSchedule Thursday = new OperatingSchedule
        {
            DayOfWeek = DayOfWeek.Thursday,
            TimeOfOpen = new NodaTime.LocalTime(),
            TimeOfClose = new NodaTime.LocalTime(),
            IsClosed = true,
            Venue = venue
        };
        public OperatingSchedule Friday = new OperatingSchedule
        {
            DayOfWeek = DayOfWeek.Friday,
            TimeOfOpen = new NodaTime.LocalTime(),
            TimeOfClose = new NodaTime.LocalTime(),
            IsClosed = true,
            Venue = venue
        };
        public OperatingSchedule Saturday = new OperatingSchedule
        {
            DayOfWeek = DayOfWeek.Saturday,
            TimeOfOpen = new NodaTime.LocalTime(),
            TimeOfClose = new NodaTime.LocalTime(),
            IsClosed = true,
            Venue = venue
        };
    }
}
