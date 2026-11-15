using System.Web;
using System.Web.Mvc;

namespace webbanhangadidas_65134070
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
