namespace Pulse.Core.Models.Entities
{
    /// <summary>
    /// Join entity representing the many-to-many relationship between Tags and Specials
    /// </summary>
    public class TagToSpecialLink
    {
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; } = null!;

        public int SpecialId { get; set; }
        public virtual Special Special { get; set; } = null!;
    }
}
