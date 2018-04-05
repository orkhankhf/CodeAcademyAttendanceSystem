using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeAcademyAttendanceSystem.Models;

namespace CodeAcademyAttendanceSystem.Areas.Teacher.Controllers
{
    public class PartialsController : Controller
    {
        CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();

        // GET: Teacher/Partials
        public PartialViewResult TeacherGroups()
        {
            int teacher_id = Convert.ToInt32(System.Web.HttpContext.Current.Session["LoggedTeacherId"]);
            Teachers teacher = db.Teachers.Where(t => t.teacher_id == teacher_id).FirstOrDefault();
            return PartialView(teacher);
        }
    }
}