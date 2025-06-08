using Microsoft.EntityFrameworkCore;

namespace EMS.INFRASTRUCTURE.Extensions
{
    public class PaginatedList<T>
    {
        public int TotalItems { get; private set; }
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public List<T> Items { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            TotalItems = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageIndex = pageIndex;
            Items = items;
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            if (pageIndex < 1)
                pageIndex = 1;

            var count = await source.CountAsync();

            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            if (totalPages > 0 && pageIndex > totalPages)
            {
                pageIndex = totalPages;
            }

            if (totalPages == 0)
            {
                pageIndex = 1;
            }

            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}