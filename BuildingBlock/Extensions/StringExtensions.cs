using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BuildingBlock.Extensions
{
    public static class StringExtensions
    {
        public static string Join(this IEnumerable<long> input, char separator = ',')
        {
            return string.Join(separator, input);
        }

        public static string UrlEncode(this string strUrl)
        {
            return HttpUtility.UrlEncode(strUrl);
        }

        public static string UrlDecode(this string strUrl)
        {
            return HttpUtility.UrlDecode(strUrl);
        }
    }
}
