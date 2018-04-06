using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeAcademyAttendanceSystem.Areas.Teacher.Models
{
    public class GroupAttendanceList
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentSurname { get; set; }
        public bool StudentAttendanceStatus { get; set; }
        public string StudentAttendanceSenderIp { get; set; }
    }
}