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
                                                             join a in db.Students_Attendance on s.student_id equals a.students_attendance_student_id
                                                             where q.qr_codes_group_id == group_id && q.qr_codes_date == today && a.students_attendance_date == today
                                                             select new GroupAttendanceList
                                                             {
                                                                 StudentId = s.student_id,
                                                                 StudentName = s.student_name,
                                                                 StudentSurname = s.student_surname,
                                                                 StudentAttendanceStatus = (bool)a.students_attendance_status,
                                                                 StudentAttendanceSenderIp = a.students_attendance_sender_ip
                                                             }
                                                             ).ToList();

            return Json(GroupAttendanceList, JsonRequestBehavior.AllowGet);
        }
    }
}