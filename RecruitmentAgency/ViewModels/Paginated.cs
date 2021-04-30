using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RecruitmentAgency.ViewModels
{
    public class Paginated<T>: PaginatedBase
    {
        public IEnumerable<T> Items { get; set; }

        public static async Task<Paginated<T>> PaginateAsync(IQueryable<T> items, int pageIndex, int pageSize)
        {
            var count = await items.CountAsync();
            var countOfPages = count % pageSize == 0 ? count / pageSize : count / pageSize + 1;
            var actualPageIndex = Math.Min(countOfPages, pageIndex);
            var paginated = countOfPages > 0
                ? await items
                    .Skip((actualPageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync()
                : Enumerable.Empty<T>();


            return new Paginated<T>
            {
                Items = paginated,
                PageIndex = actualPageIndex,
                PageSize = pageSize,
                CountOfPages = countOfPages,
                TotalItems = count
            };
        }
    }
}
