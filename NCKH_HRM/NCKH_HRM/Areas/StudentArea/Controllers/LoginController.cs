using Microsoft.AspNetCore.Mvc;
using NCKH_HRM.Areas.StudentArea.Models;
using NCKH_HRM.Models;
using System.Text;
using NuGet.Protocol;
using System.Security.Cryptography;

namespace NCKH_HRM.Areas.StudentArea.Controllers
{
    [Area("StudentArea")]
    public class LoginController : Controller
    {
        private readonly NckhDbContext _context;
        public LoginController(NckhDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Login model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); //trả về trạng thái lỗi
            }

            //xử lý logic đăng nhập tại đây
            var pass = model.Password;
            var dataLogin = _context.UserStudents.Where(x => x.Username.Equals(model.UserName) && x.Password.Equals(pass)).FirstOrDefault();
            var data = dataLogin.ToJson();
            if (dataLogin != null)
            {
                //lưu session khi đăng nhập thành công
                HttpContext.Session.SetString("StudentLogin", data);
                TempData["SuccessMessage"] = "Đăng nhập thành công";

                return RedirectToAction("Index", "Dashboard");
            }
            TempData["ErrorMessage"] = "Sai tên đăng nhập hoặc mật khẩu";
            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("StudentLogin");
            return RedirectToAction("Index");
        }

        static string GetSHA26Hash(string input)
        {
            string hash = "";
            using (var sha256 = new SHA256Managed())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
            return hash;
        }
    }
}
