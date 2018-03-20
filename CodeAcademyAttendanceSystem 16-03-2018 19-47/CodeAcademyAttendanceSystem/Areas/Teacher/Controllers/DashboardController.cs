using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeAcademyAttendanceSystem.Areas.Teacher.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Teacher/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}