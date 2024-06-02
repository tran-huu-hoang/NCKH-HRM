using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Models;
using Newtonsoft.Json;

namespace NCKH_HRM.Controllers
{
    public class ChangePasswordController : BaseController
    {
        private readonly NckhDbContext _context;

        public ChangePasswordController(NckhDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(long? id)
        {
            var userStaff = await _context.UserStaffs
                .Include(u => u.StaffNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            return View(userStaff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IFormCollection form)
        {
            var userStaffSession = HttpContext.Session.GetString("StaffLogin");
            if (string.IsNullOrEmpty(userStaffSession))
            {
                // Handle the case where the session is missing
                return RedirectToAction("Login", "Account");
            }

            var user_staff = JsonConvert.DeserializeObject<UserStaff>(userStaffSession);

            if (form["OldPassword"].ToString() == user_staff.Password)
            {
                UserStaff userStaff = new UserStaff();
                userStaff.Id = long.Parse(form["Id"]);
                userStaff.Staff = long.Parse(form["Staff"]);
                userStaff.Username = form["Username"].ToString();
                userStaff.Password = form["NewPassword"].ToString();
                userStaff.CreateBy = form["CreateBy"].ToString();
                userStaff.UpdateBy = user_staff.Username;
                userStaff.CreateDate = DateTime.Parse(form["CreateDate"]);
                userStaff.UpdateDate = DateTime.Now;
                userStaff.IsActive = bool.Parse(form["IsActive"]);
                userStaff.IsDelete = bool.Parse(form["IsDelete"]);

                _context.Update(userStaff);
                await _context.SaveChangesAsync();
            }
            return View();
        }
    }
}
