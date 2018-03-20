using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeAcademyAttendanceSystem.Models
{
    public class ResetSessions
    {
        public static void ResetAllSessions()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}