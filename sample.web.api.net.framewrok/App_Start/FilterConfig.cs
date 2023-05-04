using System.Web;
using System.Web.Mvc;

namespace sample.web.api.net.framewrok
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
