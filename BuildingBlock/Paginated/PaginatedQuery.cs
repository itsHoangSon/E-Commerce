using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Paginated
{
    public interface IPaginatedQuery
    {
        int PageIndex { get; set; }
        int PageSize { get; set; }
        string? SortBy { get; set; }
        SortType? SortType { get; set; }
    }

    public class PaginatedQuery : IPaginatedQuery
    {
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public SortType? SortType { get; set; }

        public Dictionary<string, string> GetRouteDatas()
        {
            PropertyInfo[] infos = this.GetType().GetProperties();

            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (PropertyInfo info in infos)
            {
                var val = info.GetValue(this, null);
                if (val != null)
                    result.Add(info.Name, val.ToString());
            }
            return result;
        }
    }

    public enum SortType : int
    {
        Asc = 0,
        Desc
    }
}
