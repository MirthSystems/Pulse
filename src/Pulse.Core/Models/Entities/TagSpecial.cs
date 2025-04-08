namespace Pulse.Core.Models.Entities
{
    public class TagSpecial
    {
        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;

        public int SpecialId { get; set; }
        public Special Special { get; set; } = null!;
    }
}
