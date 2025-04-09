namespace Pulse.Core.Models.Responses
{
    public class TagListResponse
    {
        public List<TagListItem> Tags { get; set; } = new List<TagListItem>();
        public int TotalCount { get; set; }
    }
}
