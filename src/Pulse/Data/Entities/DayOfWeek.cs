namespace Pulse.Data.Entities
{
    public class DayOfWeek
    {
        #region Identity and primary fields
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ShortName { get; set; }
        #endregion

        #region Metadata fields
        public int IsoNumber { get; set; }
        public byte BitMask { get; set; }
        public bool IsWeekday { get; set; }
        public int SortOrder { get; set; }
        #endregion

        #region Navigation properties
        public IList<BusinessHours> BusinessHours { get; set; } = new List<BusinessHours>();
        #endregion
    }
}
