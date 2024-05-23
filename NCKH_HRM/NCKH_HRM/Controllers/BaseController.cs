using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NCKH_HRM.Controllers
{
    public class BaseController : Controller, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("StaffLogin") == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Login", Action = "Index" }));
            }

            base.OnActionExecuting(context);
        }
    }
}
