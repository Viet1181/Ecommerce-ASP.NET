using System.Web;
using System.Web.Mvc;

namespace NguyenQuocViet_2122110285
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
