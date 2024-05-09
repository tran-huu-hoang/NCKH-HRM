using Microsoft.AspNetCore.Mvc;
using NCKH_HRM.Areas.Admin.Models;
using NCKH_HRM.Models;
using System.Text;
using NuGet.Protocol;
using System.Security.Cryptography;

namespace NCKH_HRM.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            var dataLogin = _context.UserStaffs.Where(x => x.Username.Equals(model.Email) && x.Password.Equals(pass)).FirstOrDefault();
            var data = dataLogin.ToJson();
            if (dataLogin != null)
            {
                //lưu session khi đăng nhập thành công
                HttpContext.Session.SetString("AdminLogin", data);

                return RedirectToAction("Index", "Dashboard");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminLogin");
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
