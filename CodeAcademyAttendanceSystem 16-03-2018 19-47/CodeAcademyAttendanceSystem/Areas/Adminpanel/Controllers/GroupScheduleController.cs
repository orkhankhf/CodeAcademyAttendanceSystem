using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodeAcademyAttendanceSystem.Models;

namespace CodeAcademyAttendanceSystem.Areas.Adminpanel.Controllers
{
    public class GroupScheduleController : Controller
    {
        private CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();

        [HttpGet]
        public ActionResult Index()
        {
            return View(db.Group_Schedule.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "group_schedule_id,group_schedule_name")] Group_Schedule group_Schedule)
        {
            if (ModelState.IsValid)
            {
                db.Group_Schedule.Add(group_Schedule);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            return View(group_Schedule);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group_Schedule group_Schedule = db.Group_Schedule.Find(id);
            if (group_Schedule == null)
            {
                return HttpNotFound();
            }
            return View(group_Schedule);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "group_schedule_id,group_schedule_name")] Group_Schedule group_Schedule)
        {
            if (ModelState.IsValid)
            {
                Group_Schedule update_group_schedule = db.Group_Schedule.Find(group_Schedule.group_schedule_id);
                update_group_schedule.group_schedule_name = group_Schedule.group_schedule_name;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group_Schedule);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Group_Schedule group_Schedule = db.Group_Schedule.Find(id);
                db.Group_Schedule.Remove(group_Schedule);
                db.SaveChanges();
                return Json(new { result = "group_schedule_deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
