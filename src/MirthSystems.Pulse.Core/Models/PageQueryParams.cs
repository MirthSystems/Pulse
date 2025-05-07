namespace MirthSystems.Pulse.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PageQueryParams
    {
        /// <summary>
        /// Gets or sets the page number for pagination.
        /// </summary>
        /// <remarks>
        /// <para>This is a 1-based index (first page is 1, not 0).</para>
        /// <para>Default is 1.</para>
        /// </remarks>
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of items per page for pagination.
        /// </summary>
        /// <remarks>
        /// <para>Defines how many items are returned per page.</para>
        /// <para>Default is 20.</para>
        /// <para>Maximum allowed value is 10000.</para>
        /// </remarks>
        [Range(1, 10000)]
        public int PageSize { get; set; } = 100;
    }
}
