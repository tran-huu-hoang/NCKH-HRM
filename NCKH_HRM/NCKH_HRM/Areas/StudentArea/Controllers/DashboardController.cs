using Microsoft.AspNetCore.Mvc;

namespace NCKH_HRM.Areas.StudentArea.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
