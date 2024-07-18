﻿using Microsoft.AspNetCore.Components.Forms;
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
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                                  termId = term.Id,
                                  student.Code,
                                  student.Name,
                                  detailterm.Id,
                                  student.BirthDate
                              } into g
                              select new AttendanceSheet
                              {
                                  TermId = g.Key.termId,
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

        [HttpGet]
        public async Task<IActionResult> Export(long? id)
        {

            var data = await (from term in _context.Terms
                              join detailterm in _context.DetailTerms on term.Id equals detailterm.Term
                              join semesters in _context.Semesters on detailterm.Semester equals semesters.Id
                              join registstudent in _context.RegistStudents on detailterm.Id equals registstudent.DetailTerm
                              join attendance in _context.Attendances on registstudent.Id equals attendance.RegistStudent
                              join detailattendance in _context.DetailAttendances on attendance.Id equals detailattendance.IdAttendance
                              join student in _context.Students on registstudent.Student equals student.Id
                              join classes in _context.Classes on student.Classes equals classes.Id
                              join teachingassignments in _context.TeachingAssignments on detailterm.Id equals teachingassignments.DetailTerm
                              join staff in _context.Staff on teachingassignments.Staff equals staff.Id
                              join majors in _context.Majors on staff.Major equals majors.Id
                              where detailterm.Id == id
                              group new { student, detailattendance, detailterm, classes, term } by new
                              {
                                  majorName = majors.Name,
                                  semesterName = semesters.Name,
                                  teacherName = staff.Name,
                                  termClass = detailterm.TermClass,
                                  termName = term.Name,
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
                                  TermName = g.Key.termName,
                                  TeacherName = g.Key.teacherName,
                                  Semester = g.Key.semesterName,
                                  MajorName = g.Key.majorName,
                                  TermClass = g.Key.termClass,
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
                                  CountDateLearn = g.Count(x => x.detailattendance.BeginClass.HasValue || !x.detailattendance.BeginClass.HasValue) * 2,
                                  //tính điểm chuyên cần
                                  AttendancePoint = (double)(g.Count(x => x.detailattendance.BeginClass == 1) //đếm số buổi đầu giờ đi học
                                  + g.Count(x => x.detailattendance.EndClass == 1) //đếm số buổi cuối giờ đi học
                                  + (double)(g.Count(x => x.detailattendance.BeginClass == 4) + g.Count(x => x.detailattendance.EndClass == 4)) / 2) //đếm số buổi muộn
                                   / (g.Count(x => x.detailattendance.BeginClass.HasValue) * 2)//đếm số buổi học (đầu giờ + cuối giờ)
                              }).ToListAsync();

            var dateLearn = await (
                              from detailterm in _context.DetailTerms
                              join registstudent in _context.RegistStudents on detailterm.Id equals registstudent.DetailTerm
                              join attendance in _context.Attendances on registstudent.Id equals attendance.RegistStudent
                              join detailattendance in _context.DetailAttendances on attendance.Id equals detailattendance.IdAttendance
                              join datelearn in _context.DateLearns on detailattendance.DateLearn equals datelearn.Id
                              join timeline in _context.Timelines on datelearn.Timeline equals timeline.Id
                              where detailterm.Id == id
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

                worksheet.Cells.Style.Font.Name = "Times New Roman";
                worksheet.Cells.Style.Font.Size = 12;

                worksheet.Column(1).Width = 8.67;
                worksheet.Column(2).Width = 15.22;
                worksheet.Column(3).Width = 28.78;
                worksheet.Column(4).Width = 14.33;
                worksheet.Column(5).Width = 6;
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
                worksheet.Cells["A2:D2"].Value = "KHOA " + data.FirstOrDefault().MajorName.ToUpper();
                worksheet.Cells["A2:D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A2:D2"].Style.Font.Bold = true;

                worksheet.Cells["A4:AA4"].Merge = true;
                worksheet.Cells["A4:AA4"].Value = "SỔ ĐIỂM DANH THEO DÕI CHUYÊN CẦN SINH VIÊN";
                worksheet.Cells["A4:AA4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A4:AA4"].Style.Font.Bold = true;

                worksheet.Cells["A5"].Value = "Học phần: " + data.FirstOrDefault().TermName;
                worksheet.Cells["A5"].Style.Font.Bold = true;

                worksheet.Cells["A6"].Value = "Lớp: " + data.FirstOrDefault().TermClass;
                worksheet.Cells["A6"].Style.Font.Bold = true;

                worksheet.Cells["J5"].Value = "Giảng viên: " + data.FirstOrDefault().TeacherName;
                worksheet.Cells["J5"].Style.Font.Bold = true;

                worksheet.Cells["J6"].Value = data.FirstOrDefault().Semester;
                worksheet.Cells["J6"].Style.Font.Bold = true;

                worksheet.Cells["U5"].Value = "Khoa: " + data.FirstOrDefault().MajorName;
                worksheet.Cells["U5"].Style.Font.Bold = true;

                worksheet.Cells["A8:A10"].Merge = true;
                worksheet.Cells["A8:A10"].Value = "STT";
                worksheet.Cells["A8:A10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A8:A10"].Style.Font.Bold = true;
                worksheet.Cells["A8:A10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                worksheet.Cells["B8:B10"].Merge = true;
                worksheet.Cells["B8:B10"].Value = "Mã sinh viên";
                worksheet.Cells["B8:B10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["B8:B10"].Style.Font.Bold = true;
                worksheet.Cells["B8:B10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                worksheet.Cells["C8:C10"].Merge = true;
                worksheet.Cells["C8:C10"].Value = "Họ và tên";
                worksheet.Cells["C8:C10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["C8:C10"].Style.Font.Bold = true;
                worksheet.Cells["C8:C10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                worksheet.Cells["D8:D10"].Merge = true;
                worksheet.Cells["D8:D10"].Value = "Ngày sinh";
                worksheet.Cells["D8:D10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["D8:D10"].Style.Font.Bold = true;
                worksheet.Cells["D8:D10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                worksheet.Cells["E8:E10"].Merge = true;
                worksheet.Cells["E8:E10"].Value = "CC";
                worksheet.Cells["E8:E10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["E8:E10"].Style.Font.Bold = true;
                worksheet.Cells["E8:E10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                worksheet.Cells[8, 5 + dateLearnCount * 2 +1, 10, 5 + dateLearnCount * 2 + 1].Merge = true;
                worksheet.Cells[8, 5 + dateLearnCount * 2 + 1, 10, 5 + dateLearnCount * 2 + 1].Value = "Ghi chú";
                worksheet.Cells[8, 5 + dateLearnCount * 2 + 1, 10, 5 + dateLearnCount * 2 + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[8, 5 + dateLearnCount * 2 + 1, 10, 5 + dateLearnCount * 2 + 1].Style.Font.Bold = true;
                worksheet.Cells[8, 5 + dateLearnCount * 2 + 1, 10, 5 + dateLearnCount * 2 + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                for (int i = 0; i < dateLearnCount * 2; i+=2)
                {
                    worksheet.Cells[8, i + 6, 8, i + 7].Merge = true;
                    worksheet.Cells[8, i + 6, 8, i + 7].Value = "Buổi " + (i/2 + 1);
                    worksheet.Cells[8, i + 6, 8, i + 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[8, i + 6, 8, i + 7].Style.Font.Bold = true;

                    worksheet.Cells[9, i + 6, 9, i + 7].Merge = true;
                    worksheet.Cells[9, i + 6, 9, i + 7].Value = dateLearn[i/2].DateLearn?.ToString("dd/MM");
                    worksheet.Cells[9, i + 6, 9, i + 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[10, i + 6].Value = "ĐG";
                    worksheet.Cells[10, i + 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[10, i + 6].Style.Font.Bold = true;

                    worksheet.Cells[10, i + 7].Value = "CG";
                    worksheet.Cells[10, i + 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[10, i + 7].Style.Font.Bold = true;
                }

                var dataCount = data.Count;
                for(int i = 0; i < dataCount; i++)
                {
                    worksheet.Cells[i + 11, 1].Value = i + 1;
                    worksheet.Cells[i + 11, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[i + 11, 2].Value = data[i].StudentCode;
                    worksheet.Cells[i + 11, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[i + 11, 3].Value = data[i].StudentName;

                    worksheet.Cells[i + 11, 4].Value = data[i].BirthDay?.ToShortDateString();
                    worksheet.Cells[i + 11, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[i + 11, 5].Value = Math.Round((data[i].AttendancePoint ?? 0) * 100);
                    worksheet.Cells[i + 11, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    for (int j = 0; j < (data[i].ListBeginClass.Count() * 2); j+=2)
                    {
                        if (data[i].ListBeginClass[j / 2] == 1)
                        {
                            worksheet.Cells[i + 11, j + 6].Value = "P";
                            worksheet.Cells[i + 11, j + 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        if (data[i].ListBeginClass[j / 2] == 2)
                        {
                            worksheet.Cells[i + 11, j + 6].Value = "A";
                            worksheet.Cells[i + 11, j + 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            // Đổi màu nền
                            worksheet.Cells[i + 11, j + 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[i + 11, j + 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 145, 56));
                        }
                        if (data[i].ListBeginClass[j / 2] == 3)
                        {
                            worksheet.Cells[i + 11, j + 6].Value = "PA";
                            worksheet.Cells[i + 11, j + 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        if (data[i].ListBeginClass[j / 2] == 4)
                        {
                            worksheet.Cells[i + 11, j + 6].Value = "P-";
                            worksheet.Cells[i + 11, j + 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            // Đổi màu nền
                            worksheet.Cells[i + 11, j + 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[i + 11, j + 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));
                        }

                        if (data[i].ListEndClass[j / 2] == 1)
                        {
                            worksheet.Cells[i + 11, j + 7].Value = "P";
                            worksheet.Cells[i + 11, j + 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        if (data[i].ListEndClass[j / 2] == 2)
                        {
                            worksheet.Cells[i + 11, j + 7].Value = "A";
                            worksheet.Cells[i + 11, j + 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            // Đổi màu nền
                            worksheet.Cells[i + 11, j + 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[i + 11, j + 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(230, 145, 56));
                        }
                        if (data[i].ListEndClass[j / 2] == 3)
                        {
                            worksheet.Cells[i + 11, j + 7].Value = "PA";
                            worksheet.Cells[i + 11, j + 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        if (data[i].ListEndClass[j / 2] == 4)
                        {
                            worksheet.Cells[i + 11, j + 7].Value = "P-";
                            worksheet.Cells[i + 11, j + 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            // Đổi màu nền
                            worksheet.Cells[i + 11, j + 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[i + 11, j + 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));
                        }
                    }
                }

                worksheet.Cells[10 + dataCount + 1, 1, 10 + dataCount + 1, 4].Merge = true;
                worksheet.Row(10 + dataCount + 1).Height = 30;
                worksheet.Cells[10 + dataCount + 1, 1, 10 + dataCount + 1, 4].Value = "Giảng viên ký xác nhận sau mỗi buổi học:";
                worksheet.Cells[10 + dataCount + 1, 1, 10 + dataCount + 1, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                for (int i = 0; i < dateLearnCount * 2; i += 2)
                {
                    worksheet.Cells[10 + dataCount + 1, i + 5, 10 + dataCount + 1, i + 6].Merge = true;
                }

                var enough = 0;
                var notenough = 0;
                for(int i = 0; i < dataCount; i++)
                {
                    if (data[i].AttendancePoint >= 0.8)
                    {
                        enough++;
                    }
                    else
                    {
                        notenough++;
                    }
                }

                worksheet.Cells[10 + dataCount + 4, 1].Value = "Số sinh viên đủ điều kiện thi kết thúc học phần:";
                worksheet.Cells[10 + dataCount + 5, 1].Value = "Số sinh viên không đủ điều kiện thi kết thúc học phần:";
                worksheet.Cells[10 + dataCount + 4, 4].Value = enough;
                worksheet.Cells[10 + dataCount + 5, 4].Value = notenough;

                worksheet.Cells[10 + dataCount + 3, 10].Value = "Chú thích:";
                worksheet.Cells[10 + dataCount + 3, 10].Style.Font.Bold = true;

                worksheet.Cells[10 + dataCount + 4, 10].Value = "P";
                worksheet.Cells[10 + dataCount + 5, 10].Value = "A";
                worksheet.Cells[10 + dataCount + 6, 10].Value = "PA";
                worksheet.Cells[10 + dataCount + 7, 10].Value = "P-";


                worksheet.Cells[10 + dataCount + 4, 11].Value = "Có mặt";
                worksheet.Cells[10 + dataCount + 5, 11].Value = "Vắng";
                worksheet.Cells[10 + dataCount + 6, 11].Value = "Phép";
                worksheet.Cells[10 + dataCount + 7, 11].Value = "Muộn";

                var cellBorder = worksheet.Cells[8, 1, 10 + dataCount + 1, 5 + dateLearnCount * 2 + 1].Style.Border;
                cellBorder.Top.Style = ExcelBorderStyle.Thin;
                cellBorder.Bottom.Style = ExcelBorderStyle.Thin;
                cellBorder.Left.Style = ExcelBorderStyle.Thin;
                cellBorder.Right.Style = ExcelBorderStyle.Thin;

                var stream = new MemoryStream();
                package.SaveAs(stream);

                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DiemDanh_" + (data.FirstOrDefault().TermClass) + ".xlsx");
            }
        }
    }
}