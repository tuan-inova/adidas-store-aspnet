using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;

namespace webbanhangadidas_65134070.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class AdminOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            if (session["MaTK"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        controller = "Account_65134070", action = "Login"
                    }));
                return;
            }

            if (!string.Equals(session["VaiTro"] as string, "QuanLy", StringComparison.Ordinal))
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
    }
}
