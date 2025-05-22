namespace Pulse.Data.Entities
{
    public class DayOfWeek
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ShortName { get; set; }
        public int IsoNumber { get; set; }
        public byte ByteMap { get; set; }
        public bool IsWeekday { get; set; }
        public int SortOrder { get; set; }
        public IList<BusinessHours> BusinessHours { get; set; } = new List<BusinessHours>();
    }
}
