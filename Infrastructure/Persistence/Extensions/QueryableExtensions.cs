using Application.Common.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResponse<T>> ToPagedResponseAsync<T>(
            this IQueryable<T> query, PagedRequest request)
        {
            var totalCount = await query.CountAsync();
            var data = await query.Skip(request.Skip).Take(request.PageSize).ToListAsync();

            return new PagedResponse<T>
            {
                Data = data,
                TotalCount = totalCount,
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
            };
        }
    }
}
