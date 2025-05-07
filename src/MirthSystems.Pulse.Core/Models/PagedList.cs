namespace MirthSystems.Pulse.Core.Models
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    public class PagedList<T> : List<T>
    {
        public int PageSize { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalCount { get; private set; }
        public int PageCount { get; private set; }
        public bool HasPrevious => (CurrentPage > 1);
        public bool HasNext => (CurrentPage < PageCount);

        private PagedList(List<T> items, int pageSize, int currentPage, int totalItemCount)
        {
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalCount = totalItemCount;
            PageCount = (int)Math.Ceiling(TotalCount / (double)PageSize);
            AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> items, int pageIndex, int pageSize)
        {
            var count = await items.CountAsync();
            var pagedItems = await items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(pagedItems, pageSize, pageIndex, count);
        }
    }
}
