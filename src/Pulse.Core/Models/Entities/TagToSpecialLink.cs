namespace Pulse.Core.Models.Entities
{
    /// <summary>
    /// Join entity representing the many-to-many relationship between Tags and Specials
    /// </summary>
    public class TagToSpecialLink : EntityBase
    {
        public long TagId { get; set; }
        public virtual Tag Tag { get; set; } = null!;

        public long SpecialId { get; set; }
        public virtual Special Special { get; set; } = null!;
    }
}
