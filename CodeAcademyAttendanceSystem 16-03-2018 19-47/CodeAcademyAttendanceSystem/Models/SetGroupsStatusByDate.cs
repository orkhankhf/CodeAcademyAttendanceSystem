using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeAcademyAttendanceSystem.Models;

namespace CodeAcademyAttendanceSystem.Models
{
    //Bu class'da bu gündən köhnə olan grupların statusunu false edir. Yəni ki, bu günün tarixinin 25-03-2018 olduğunu varsayarsaq,
    //Groups Table'da group_end_date column'u 25-03-2018 (bu gün) tarixindən kiçik olan bütün grupların group_status'nu false edir.
    //Bu classın constructor'u login olunan zaman run olunur ki, Admin/Müəllim vaxtı bitən grupları görməsin və ya grupun vaxtının bitdiyini görsün.
    public class SetGroupsStatusByDate
    {
        CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();
        public void Run()
        {
            DateTime Today = DateTime.Now;
            List<Groups> ExpiredGroups = db.Groups.Where(g => g.group_end_date < Today && g.group_status == true).ToList();
            foreach (var group in ExpiredGroups)
            {
                group.group_status = false;
            }
            db.SaveChanges();
        }
    }
}