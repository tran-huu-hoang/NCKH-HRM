using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Models;
using Newtonsoft.Json;

namespace NCKH_HRM.Areas.StudentArea.Controllers
{
    [Area("StudentArea")]
    public class ChangePasswordController : BaseController
    {
        private readonly NckhDbContext _context;

        public ChangePasswordController(NckhDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(long? id)
        {
            var userSttudent = await _context.UserStudents
                .Include(u => u.StudentNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            return View(userSttudent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IFormCollection form)
        {
            var userStudentSession = HttpContext.Session.GetString("StudentLogin");
            if (string.IsNullOrEmpty(userStudentSession))
            {
                // Handle the case where the session is missing
                return RedirectToAction("Login", "Account");
            }

            var user_student = JsonConvert.DeserializeObject<UserStudent>(userStudentSession);

            if (form["OldPassword"].ToString() == user_student.Password)
            {
                UserStudent userStudent = new UserStudent();
                userStudent.Id = long.Parse(form["Id"]);
                userStudent.Student = long.Parse(form["Student"]);
                userStudent.Username = form["Username"].ToString();
                userStudent.Password = form["NewPassword"].ToString();
                userStudent.CreateBy = form["CreateBy"].ToString();
                userStudent.UpdateBy = user_student.Username;
                userStudent.CreateDate = DateTime.Parse(form["CreateDate"]);
                userStudent.UpdateDate = DateTime.Now;
                userStudent.IsActive = bool.Parse(form["IsActive"]);
                userStudent.IsDelete = bool.Parse(form["IsDelete"]);

                _context.Update(userStudent);
                await _context.SaveChangesAsync();
            }
            return View();
        }
    }
}
