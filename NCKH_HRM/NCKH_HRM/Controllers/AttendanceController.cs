using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NCKH_HRM.Models;
using NCKH_HRM.ViewModels;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace NCKH_HRM.Controllers
{
    public class AttendanceController : BaseController
    {
        private readonly NckhDbContext _context;

        public AttendanceController(NckhDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user_staff = JsonConvert.DeserializeObject<UserStaff>(HttpContext.Session.GetString("StaffLogin"));

            var data = await (from userstaff in _context.UserStaffs
                              join staff in _context.Staff on userstaff.Staff equals staff.Id
                              join teachingassignment in _context.TeachingAssignments on staff.Id equals teachingassignment.Staff
                              join detailterm in _context.DetailTerms on teachingassignment.DetailTerm equals detailterm.Id
                              join registstudents in _context.RegistStudents on detailterm.Id equals registstudents.DetailTerm
                              join term in _context.Terms on detailterm.Term equals term.Id
                              join datelearn in _context.DateLearns on detailterm.Id equals datelearn.DetailTerm
                              join timeline in _context.Timelines on datelearn.Timeline equals timeline.Id
                              join year in _context.Years on timeline.Year equals year.Id
                              join staffsubject in _context.StaffSubjects on staff.Id equals staffsubject.Staff
                              join subject in _context.Subjects on staffsubject.Subject equals subject.Id
                              where userstaff.Id == user_staff.Id && year.Name == DateTime.Now.Year
                              group new { term, staff, subject, detailterm, registstudents } by new
                              {
                                  term.Id,
                                  term.Name,
                                  term.Code,
                                  term.CollegeCredit
                              } into g
                              select new StaffIndex
                              {
                                  TermId = g.Key.Id,
                                  TermName = g.Key.Name,
                                  TermCode = g.Key.Code,
                                  StudentNumber = g.Select(x => x.registstudents.Student).Distinct().Count(),
                                  TermClassNumber = g.Select(x => x.detailterm.TermClass).Distinct().Count(),
                                  CollegeCredit = g.Key.CollegeCredit,
                              }).ToListAsync();

            var staffinfodata = await (from userstaff in _context.UserStaffs
                              join staff in _context.Staff on userstaff.Staff equals staff.Id
                              join staffsubject in _context.StaffSubjects on staff.Id equals staffsubject.Staff
                              join subject in _context.Subjects on staffsubject.Subject equals subject.Id
                              where userstaff.Id == user_staff.Id
                              select new StaffIndex
                              {
                                  StaffCode = staff.Code,
                                  StaffName = staff.Name,
                                  SubjectName = subject.Name,
                              }).FirstOrDefaultAsync();
            ViewBag.StaffInfo = staffinfodata;

            return View(data);
        }

        public async Task<IActionResult> AttendanceSheet(long? id, long? idselect)
        {
            var dataclassterm = await (from term in _context.Terms
                                       join detailterm in _context.DetailTerms on term.Id equals detailterm.Term
                                       join teachingassignments in _context.TeachingAssignments on detailterm.Id equals teachingassignments.DetailTerm
                                       where detailterm.Term == id
                                       group new { detailterm } by new
                                       {
                                           detailterm.Id,
                                           detailterm.TermClass
                                       } into g
                                       select new DetailTerm
                                       {
                                           Id = g.Key.Id,
                                           TermClass = g.Key.TermClass
                                       }).ToListAsync();

            long? iddetailterm = null;
            if (idselect == null)
            {
                iddetailterm = dataclassterm.FirstOrDefault()?.Id;
            }
            else
            {
                iddetailterm = idselect;
            }

            var data = await (from term in _context.Terms
                              join detailterm in _context.DetailTerms on term.Id equals detailterm.Term
                              join registstudent in _context.RegistStudents on detailterm.Id equals registstudent.DetailTerm
                              join attendance in _context.Attendances on registstudent.Id equals attendance.RegistStudent
                              join detailattendance in _context.DetailAttendances on attendance.Id equals detailattendance.IdAttendance
                              join student in _context.Students on registstudent.Student equals student.Id
                              join classes in _context.Classes on student.Classes equals classes.Id
                              where detailterm.Id == iddetailterm
                              group new { student, detailattendance, detailterm, classes } by new
                              {
                                  student.Code,
                                  student.Name,
                                  detailterm.Id,
                                  student.BirthDate
                              } into g
                              select new AttendanceSheet
                              {
                                  DetailTermId = g.Key.Id,
                                  StudentCode = g.Key.Code,
                                  StudentName = g.Key.Name,
                                  BirthDay = g.Key.BirthDate,
                                  ListBeginClass = g.Select(x => x.detailattendance.BeginClass ?? -1).ToList(),
                                  ListEndClass = g.Select(x => x.detailattendance.EndClass ?? -1).ToList(),
                                  NumberOfBeginClassesAttended = g.Count(x => x.detailattendance.BeginClass == 1 ||
                                  !x.detailattendance.BeginClass.HasValue),
                                  NumberOfEndClassesAttended = g.Count(x => x.detailattendance.EndClass == 1 ||
                                  !x.detailattendance.EndClass.HasValue),
                                  NumberOfBeginLate = g.Count(x => x.detailattendance.BeginClass == 4),
                                  NumberOfEndLate = g.Count(x => x.detailattendance.EndClass == 4),
                                  CountDateLearn = g.Count(x => x.detailattendance.BeginClass.HasValue || !x.detailattendance.BeginClass.HasValue) * 2
                              }).ToListAsync();

            var dateLearn = await (
                              from detailterm in _context.DetailTerms
                              join registstudent in _context.RegistStudents on detailterm.Id equals registstudent.DetailTerm
                              join attendance in _context.Attendances on registstudent.Id equals attendance.RegistStudent
                              join detailattendance in _context.DetailAttendances on attendance.Id equals detailattendance.IdAttendance
                              join datelearn in _context.DateLearns on detailattendance.DateLearn equals datelearn.Id
                              join timeline in _context.Timelines on datelearn.Timeline equals timeline.Id
                              where detailterm.Id == iddetailterm
                              group new { timeline, registstudent, datelearn, detailterm, attendance, detailattendance } by new
                              {
                                  timeline.DateLearn,
                              } into g
                              select new Timeline
                              {
                                  DateLearn = g.Key.DateLearn,

                              }).ToListAsync();


            var termName = (from term in _context.Terms
                            join detailterm in _context.DetailTerms on term.Id equals detailterm.Term
                            where detailterm.Term == id
                            select new NameTermWithIdDT
                            {
                                Id = detailterm.Id,
                                Name = term.Name
                            }).FirstOrDefault();
            ViewBag.TermName = termName.Name;
            ViewBag.detailTerm = iddetailterm;
            ViewBag.dateLearn = dateLearn;
            ViewBag.countDateLearn = dateLearn.Count;
            ViewData["dataclassterm"] = new SelectList(dataclassterm, "Id", "TermClass");
            if (idselect != null)
            {
                return PartialView("PartialBodyAttendanceSheet", data);
            }
            else
            {
                return View(data);
            }
        }


        public async Task<IActionResult> StudentInTerm(long? id)
        {
            var data = await (from term in _context.Terms
                              join detailterm in _context.DetailTerms on term.Id equals detailterm.Term
                              join registstudent in _context.RegistStudents on detailterm.Id equals registstudent.DetailTerm
                              join attendance in _context.Attendances on registstudent.Id equals attendance.RegistStudent
                              join detailattendance in _context.DetailAttendances on attendance.Id equals detailattendance.IdAttendance
                              join datelearn in _context.DateLearns on detailattendance.DateLearn equals datelearn.Id
                              join timeline in _context.Timelines on datelearn.Timeline equals timeline.Id
                              join student in _context.Students on registstudent.Student equals student.Id
                              where detailterm.Id == id && timeline.DateLearn.Value.Date == DateTime.Now.Date
                              group new { student, timeline, registstudent, datelearn, detailterm, attendance, detailattendance } by new
                              {
                                  student.Code,
                                  student.Name,
                                  student.BirthDate,
                                  timeline.DateLearn,
                                  student.Id,
                                  attendanceId = attendance.Id,
                                  detailtermId = detailterm.Id,
                                  datelearnId = datelearn.Id,
                                  detailattendance.BeginClass,
                                  detailattendance.EndClass,
                                  detailattendance.Description,
                                  detailattendanceId = detailattendance.Id
                              } into g
                              select new StudentInTerm
                              {
                                  Id = g.Key.detailattendanceId,
                                  StudentCode = g.Key.Code,
                                  StudentName = g.Key.Name,
                                  BirthDate = g.Key.BirthDate,
                                  DateLearn = g.Key.DateLearn,
                                  StudentId = g.Key.Id,
                                  AttendanceId = g.Key.attendanceId,
                                  DetailTermId = g.Key.detailtermId,
                                  DateLearnId = g.Key.datelearnId,
                                  BeginClass = g.Key.BeginClass,
                                  EndClass = g.Key.EndClass,
                                  Description = g.Key.Description,
                              }).ToListAsync();
            var termName = (from term in _context.Terms
                            join detailterm in _context.DetailTerms on term.Id equals detailterm.Term
                            where detailterm.Id == id
                            select new NameTermWithIdDT
                            {
                                Id = detailterm.Id,
                                Name = term.Name,
                                TermClassName = detailterm.TermClass
                            }).FirstOrDefault();
            ViewBag.TermName = termName.Name;
            ViewBag.TermClassName = termName.TermClassName;
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentInTerm(IFormCollection form)
        {
            int itemCount = form["AttendanceId"].Count;
            for (int i = 0; i < itemCount; i++)
            {
                DetailAttendance attendancedetail = new DetailAttendance();
                attendancedetail.Id = long.Parse(form["Id"][i]);
                attendancedetail.IdAttendance = long.Parse(form["AttendanceId"][i]);
                attendancedetail.DetailTerm = long.Parse(form["DetailTermId"][i]);
                attendancedetail.DateLearn = long.Parse(form["DateLearnId"][i]);
                attendancedetail.BeginClass = int.Parse(form["begin-"+(i+1)].ToString());
                attendancedetail.EndClass = int.Parse(form["end-"+(i+1)].ToString());
                attendancedetail.Description = form["Description"][i].ToString().ToString();

                _context.Update(attendancedetail);

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file, IFormCollection form)
        {
            if (file == null || file.Length == 0)
            {
                /*TempData["ErrorMessage"] = "Bạn chưa chọn file";*/
                return BadRequest("Bạn chưa chọn file");
            }

            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                /*TempData["ErrorMessage"] = "File phải có định dạng Excel (.xlsx)";*/
                return BadRequest("File phải có định dạng Excel (.xlsx)");
            }

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 0; row < rowCount; row++)
                    {
                        if (row >= form["Id"].Count || row >= form["AttendanceId"].Count || row >= form["DetailTermId"].Count || row >= form["DateLearnId"].Count)
                        {
                            Console.WriteLine($"Index out of range: row = {row}");
                            continue;
                        }

                        DetailAttendance detailAttendance = new DetailAttendance();
                        detailAttendance.Id = long.Parse(form["Id"][row]);
                        detailAttendance.IdAttendance = long.Parse(form["AttendanceId"][row]);
                        detailAttendance.DetailTerm = long.Parse(form["DetailTermId"][row]);
                        detailAttendance.DateLearn = long.Parse(form["DateLearnId"][row]);

                        // Kiểm tra và xử lý giá trị của ô [row + 1, 3]
                        object cellBeginClassValue = worksheet.Cells[row + 2, 3].Value;
                        if (cellBeginClassValue != null)
                        {
                            string beginClassValue = cellBeginClassValue.ToString().Trim();
                            if (beginClassValue == "P")
                            {
                                detailAttendance.BeginClass = 1;
                            }
                            else if (beginClassValue == "A")
                            {
                                detailAttendance.BeginClass = 2;
                            }
                            else if (beginClassValue == "PA")
                            {
                                detailAttendance.BeginClass = 3;
                            }
                            else if (beginClassValue == "P-")
                            {
                                detailAttendance.BeginClass = 4;
                            }
                        }
                        else
                        {
                            detailAttendance.BeginClass = null; // Hoặc đặt giá trị mặc định
                        }

                        // Kiểm tra và xử lý giá trị của ô [row + 1, 4]
                        object cellEndClassValue = worksheet.Cells[row + 2, 4].Value;
                        if (cellEndClassValue != null)
                        {
                            string endClassValue = cellEndClassValue.ToString().Trim();
                            if (endClassValue == "P")
                            {
                                detailAttendance.EndClass = 1;
                            }
                            else if (endClassValue == "A")
                            {
                                detailAttendance.EndClass = 2;
                            }
                            else if (endClassValue == "PA")
                            {
                                detailAttendance.EndClass = 3;
                            }
                            else if (endClassValue == "P-")
                            {
                                detailAttendance.EndClass = 4;
                            }
                        }
                        else
                        {
                            detailAttendance.EndClass = null; // Hoặc đặt giá trị mặc định
                        }

                        _context.Update(detailAttendance);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Attendance");
        }

        [HttpPost]
        public async Task<IActionResult> Export(long? id, long? idselect)
        {
            var dataclassterm = await (from term in _context.Terms
                                       join detailterm in _context.DetailTerms on term.Id equals detailterm.Term
                                       join teachingassignments in _context.TeachingAssignments on detailterm.Id equals teachingassignments.DetailTerm
                                       where detailterm.Term == id
                                       group new { detailterm } by new
                                       {
                                           detailterm.Id,
                                           detailterm.TermClass
                                       } into g
                                       select new DetailTerm
                                       {
                                           Id = g.Key.Id,
                                           TermClass = g.Key.TermClass
                                       }).ToListAsync();

            long? iddetailterm = null;
            if (idselect == null)
            {
                iddetailterm = dataclassterm.FirstOrDefault()?.Id;
            }
            else
            {
                iddetailterm = idselect;
            }

            var data = await (from term in _context.Terms
                              join detailterm in _context.DetailTerms on term.Id equals detailterm.Term
                              join registstudent in _context.RegistStudents on detailterm.Id equals registstudent.DetailTerm
                              join attendance in _context.Attendances on registstudent.Id equals attendance.RegistStudent
                              join detailattendance in _context.DetailAttendances on attendance.Id equals detailattendance.IdAttendance
                              join student in _context.Students on registstudent.Student equals student.Id
                              join classes in _context.Classes on student.Classes equals classes.Id
                              where detailterm.Id == iddetailterm
                              group new { student, detailattendance, detailterm, classes, term } by new
                              {
                                  student.Code,
                                  student.Name,
                                  detailterm.Id,
                                  student.BirthDate,
                                  termCode = term.Code,
                              } into g
                              select new AttendanceSheet
                              {
                                  DetailTermId = g.Key.Id,
                                  StudentCode = g.Key.Code,
                                  StudentName = g.Key.Name,
                                  BirthDay = g.Key.BirthDate,
                                  TermCode = g.Key.termCode,
                                  ListBeginClass = g.Select(x => x.detailattendance.BeginClass ?? -1).ToList(),
                                  ListEndClass = g.Select(x => x.detailattendance.EndClass ?? -1).ToList(),
                                  NumberOfBeginClassesAttended = g.Count(x => x.detailattendance.BeginClass == 1 ||
                                  !x.detailattendance.BeginClass.HasValue),
                                  NumberOfEndClassesAttended = g.Count(x => x.detailattendance.EndClass == 1 ||
                                  !x.detailattendance.EndClass.HasValue),
                                  NumberOfBeginLate = g.Count(x => x.detailattendance.BeginClass == 4),
                                  NumberOfEndLate = g.Count(x => x.detailattendance.EndClass == 4),
                                  CountDateLearn = g.Count(x => x.detailattendance.BeginClass.HasValue || !x.detailattendance.BeginClass.HasValue) * 2
                              }).ToListAsync();

            var dateLearn = await (
                              from detailterm in _context.DetailTerms
                              join registstudent in _context.RegistStudents on detailterm.Id equals registstudent.DetailTerm
                              join attendance in _context.Attendances on registstudent.Id equals attendance.RegistStudent
                              join detailattendance in _context.DetailAttendances on attendance.Id equals detailattendance.IdAttendance
                              join datelearn in _context.DateLearns on detailattendance.DateLearn equals datelearn.Id
                              join timeline in _context.Timelines on datelearn.Timeline equals timeline.Id
                              where detailterm.Id == iddetailterm
                              group new { timeline, registstudent, datelearn, detailterm, attendance, detailattendance } by new
                              {
                                  timeline.DateLearn,
                              } into g
                              select new Timeline
                              {
                                  DateLearn = g.Key.DateLearn,

                              }).ToListAsync();


            var termName = (from term in _context.Terms
                            join detailterm in _context.DetailTerms on term.Id equals detailterm.Term
                            where detailterm.Term == id
                            select new NameTermWithIdDT
                            {
                                Id = detailterm.Id,
                                Name = term.Name
                            }).FirstOrDefault();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Attendances");

                worksheet.Column(1).Width = 8.67;
                worksheet.Column(2).Width = 15.22;
                worksheet.Column(3).Width = 20.56;
                worksheet.Column(4).Width = 8.22;
                worksheet.Column(5).Width = 14.33;
                var dateLearnCount = dateLearn.Count();
                for (int i = 0; i < dateLearnCount * 2; i++)
                {
                    worksheet.Column(i + 6).Width = 5.67;
                }
                worksheet.Column(6 + dateLearnCount * 2).Width = 11.11;

                worksheet.Cells["A1:D1"].Merge = true;
                worksheet.Cells["A1:D1"].Value = "TRƯỜNG ĐẠI HỌC NGUYỄN TRÃI";
                worksheet.Cells["A1:D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet.Cells["A2:D2"].Merge = true;
                worksheet.Cells["A2:D2"].Value = "KHOA CÔNG NGHỆ THÔNG TIN";
                worksheet.Cells["A2:D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A2:D2"].Style.Font.Bold = true;

                worksheet.Cells["A4:AA4"].Merge = true;
                worksheet.Cells["A4:AA4"].Value = "SỔ ĐIỂM DANH THEO DÕI CHUYÊN CẦN SINH VIÊN";
                worksheet.Cells["A4:AA4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A4:AA4"].Style.Font.Bold = true;

                var stream = new MemoryStream();
                package.SaveAs(stream);

                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DiemDanh" + (data.FirstOrDefault().TermCode) + ".xlsx");
            }
        }
    }
}