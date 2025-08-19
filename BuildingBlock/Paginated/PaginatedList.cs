using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Paginated
{
    public class PaginatedList<T>
    {
        public int PageIndex { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public List<T> Items { get; set; }

        public PaginatedList(List<T> items, int totalItems, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalItems = totalItems;
            Items = items;
            PageSize = pageSize;
        }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(TotalItems / (double)PageSize);
            }
        }
    }
}
