using Microsoft.EntityFrameworkCore;

namespace ProEventos.Persistence.Pages
{
    public class PageList<T>
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<T> Items { get; set; } = new List<T>();

        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items.AddRange(items);
        }

        public static async Task<PageList<T>> CreatePageAsync(
            IQueryable<T> source, int pageNumber, int pageSize
        )
        {
            int count = await source.CountAsync();
            List<T> items = await source.Skip((pageNumber -1) * pageSize).Take(pageSize).ToListAsync();
            return new PageList<T>(items, count, pageNumber, pageSize);
        }
    }
}