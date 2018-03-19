using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeAcademyAttendanceSystem.Areas.Adminpanel.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Adminpanel/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}