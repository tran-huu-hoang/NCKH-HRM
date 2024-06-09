using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NCKH_HRM.Areas.StudentArea.Controllers
{
    [Area("StudentArea")]
    public class BaseController : Controller, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("StudentLogin") == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Login", Action = "Index", Areas = "Student" }));
            }

            base.OnActionExecuting(context);
        }
    }
}
