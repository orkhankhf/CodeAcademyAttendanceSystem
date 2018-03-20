using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeAcademyAttendanceSystem.Models;

namespace CodeAcademyAttendanceSystem.Filters
{
    public class AdminpanelLoginFilter : ActionFilterAttribute
    {
        CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["LoggedAdminId"] == null || HttpContext.Current.Session["LoggedAdminEmail"] == null || HttpContext.Current.Session["LoggedAdminPassword"] == null)
            {
                RedirectToLoginPage(filterContext);
            }
            else
            {
                try
                {
                    int LoggedAdminId = Convert.ToInt32(HttpContext.Current.Session["LoggedAdminId"]);
                    string LoggedAdminEmail = HttpContext.Current.Session["LoggedAdminEmail"].ToString();
                    string LoggedAdminPassword = HttpContext.Current.Session["LoggedAdminPassword"].ToString();

                    var check_loggined_admins_role = (from t in db.Teachers
                                                      join r in db.Role_Types on t.teacher_role_types_id equals r.role_types_id
                                                      where t.teacher_id == LoggedAdminId && t.teacher_email == LoggedAdminEmail && t.teacher_password == LoggedAdminPassword
                                                      select r.role_types_name).FirstOrDefault();
                    if(check_loggined_admins_role != "Admin")
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