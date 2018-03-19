using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeAcademyAttendanceSystem.Models;

namespace CodeAcademyAttendanceSystem.Filters
{
    public class SetNewPasswordFilter : ActionFilterAttribute
    {
        CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["FirstLogin_Id"] == null || HttpContext.Current.Session["FirstLogin_Email"] == null)
            {
                var Url = new UrlHelper(filterContext.RequestContext);
                var url = Url.Action("Index", "Login");
                filterContext.Result = new RedirectResult(url);
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}