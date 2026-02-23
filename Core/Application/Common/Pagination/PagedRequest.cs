using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Pagination
{
    public class PagedRequest
    {
        private const int MaxPageSize = 50;

        public int Page { get; set; } = 1;

        private int _pageSize = 5;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
        public int Skip => (Page - 1) * PageSize;
    }
}

