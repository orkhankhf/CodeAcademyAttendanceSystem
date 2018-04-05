using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeAcademyAttendanceSystem.Models;

namespace CodeAcademyAttendanceSystem.Filters
{
    public class TeacherLoginFilter : ActionFilterAttribute
    {
        CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["LoggedTeacherId"] == null || HttpContext.Current.Session["LoggedTeacherEmail"] == null || HttpContext.Current.Session["LoggedTeacherPassword"] == null)
            {
                RedirectToLoginPage(filterContext);
            }
            else
            {
                try
                {
                    int LoggedTeacherId = Convert.ToInt32(HttpContext.Current.Session["LoggedTeacherId"]);
                    string LoggedTeacherEmail = HttpContext.Current.Session["LoggedTeacherEmail"].ToString();
                    string LoggedTeacherPassword = HttpContext.Current.Session["LoggedTeacherPassword"].ToString();

                    var check_loggined_teachers_role = (from t in db.Teachers
                                                      join r in db.Role_Types on t.teacher_role_types_id equals r.role_types_id
                                                      where t.teacher_id == LoggedTeacherId && t.teacher_email == LoggedTeacherEmail && t.teacher_password == LoggedTeacherPassword
                                                      select r.role_types_name).FirstOrDefault();
                    if (check_loggined_teachers_role != "Müəllim")
                    {
                        RedirectToLoginPage(filterContext);
                    }
                }
                catch
                {
                    RedirectToLoginPage(filterContext);
                }
            }
            base.OnActionExecuting(filterContext);
        }
        private void RedirectToLoginPage(ActionExecutingContext filterContext)
        {
            var Url = new UrlHelper(filterContext.RequestContext);
            var url = Url.Action("Index", "Login", new { Area = "" });
            filterContext.Result = new RedirectResult(url);
            return;
        }
    }
}