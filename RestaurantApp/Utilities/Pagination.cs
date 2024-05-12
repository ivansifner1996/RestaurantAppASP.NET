using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantApp.Utilities
{
    public class Pagination<T>: List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public List<T> Items { get; private set; }

        private const int pageSize = 5;

        public Pagination(IQueryable<T> source, int pageIndex)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(source.Count() / (double)pageSize);

            Items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            this.AddRange(Items);
        }
    }
}