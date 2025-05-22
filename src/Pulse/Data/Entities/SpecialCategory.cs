namespace Pulse.Data.Entities
{
    using System.Collections.Generic;
    using NodaTime;

    public class SpecialCategory
    {
        #region Identity and primary fields
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        #endregion

        #region Metadata properties
        public string? Icon { get; set; }
        public int BitMask { get; set; }
        public int SortOrder { get; set; }
        #endregion

        #region Navigation properties
        public IList<Special> Specials { get; set; } = new List<Special>();
        #endregion
    }
}
