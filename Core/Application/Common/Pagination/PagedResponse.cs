using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Pagination
{
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        /// <summary>
        /// TotalCount ve PageSize'dan otomatik hesaplanır.
        /// Elle set etmeye gerek yoktur.
        /// </summary>
        public int TotalPages => PageSize > 0
            ? (int)Math.Ceiling(TotalCount / (double)PageSize)
            : 0;
    }
}
