using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeAcademyAttendanceSystem.Models;
using CodeAcademyAttendanceSystem.Filters;
using CodeAcademyAttendanceSystem.Areas.Teacher.Models;

namespace CodeAcademyAttendanceSystem.Areas.Teacher.Controllers
{
    [TeacherLoginFilter]
    public class DashboardController : Controller
    {
        CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();

        DateTime today = Convert.ToDateTime(DateTime.Now.ToShortDateString());

        // GET: Teacher/Dashboard
        public ActionResult Index()
        {
            return View();
        }

        //Tələbələrin dərsdə olub olmadığını göstərən siyahını return edir
        public ActionResult CheckStudentAttendanceList(int group_id)
        {
            List<GroupAttendanceList> GroupAttendanceList = (from q in db.Qr_Codes
                                                             join g in db.Groups on q.qr_codes_group_id equals g.group_id
                                                             join s in db.Students on g.group_id equals s.student_group_id
                                                             where q.qr_codes_group_id == group_id
                                                             select new GroupAttendanceList
                                                             {
                                                                 StudentId = s.student_id,
                                                                 StudentName = s.student_name,
                                                                 StudentSurname = s.student_surname,
                                                                 StudentAttendanceStatus = false
                                                             }
                                                             ).ToList();
            string bla = "";
            foreach (var item in GroupAttendanceList)
            {
                Students_Attendance group_attendance = db.Students_Attendance.Where(a => a.students_attendance_date == today && a.students_attendance_student_id == item.StudentId).FirstOrDefault();
                bool test = Convert.ToBoolean(group_attendance.students_attendance_status);
                item.StudentAttendanceStatus = test;
                bla += item.StudentId + " " + item.StudentAttendanceStatus + "<br>";
            }
            return Content(bla);
            return Json(new { }, JsonRequestBehavior.AllowGet);

        }
    }
}