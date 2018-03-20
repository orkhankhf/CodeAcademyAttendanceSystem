using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeAcademyAttendanceSystem.Filters;

namespace CodeAcademyAttendanceSystem.Areas.Adminpanel.Controllers
{
    [AdminpanelLoginFilter]
    public class DashboardController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}