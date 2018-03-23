using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodeAcademyAttendanceSystem.Models;
using CodeAcademyAttendanceSystem.Filters;

namespace CodeAcademyAttendanceSystem.Areas.Adminpanel.Controllers
{
    [AdminpanelLoginFilter]
    public class LessonTimesController : Controller
    {
        private CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();

        [HttpGet]
        public ActionResult Index()
        {
            var lesson_Times = db.Lesson_Times.Include(l => l.Group_Schedule);
            return View(lesson_Times.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.lesson_times_group_schedule_id = new SelectList(db.Group_Schedule, "group_schedule_id", "group_schedule_name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lesson_times_id,lesson_times_name,lesson_times_start_time,lesson_times_end_time,lesson_times_group_schedule_id")] Lesson_Times lesson_Times)
        {
            if (ModelState.IsValid)
            {
                db.Lesson_Times.Add(lesson_Times);
                db.SaveChanges();
                return RedirectToAction("Create", "LessonTimes");
            }

            ViewBag.lesson_times_group_schedule_id = new SelectList(db.Group_Schedule, "group_schedule_id", "group_schedule_name", lesson_Times.lesson_times_group_schedule_id);
            return View(lesson_Times);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson_Times lesson_Times = db.Lesson_Times.Find(id);
            if (lesson_Times == null)
            {
                return HttpNotFound();
            }
            ViewBag.lesson_times_group_schedule_id = new SelectList(db.Group_Schedule, "group_schedule_id", "group_schedule_name", lesson_Times.lesson_times_group_schedule_id);
            return View(lesson_Times);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lesson_times_id,lesson_times_name,lesson_times_start_time,lesson_times_end_time,lesson_times_group_schedule_id")] Lesson_Times lesson_Times)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lesson_Times).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lesson_times_group_schedule_id = new SelectList(db.Group_Schedule, "group_schedule_id", "group_schedule_name", lesson_Times.lesson_times_group_schedule_id);
            return View(lesson_Times);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Lesson_Times lesson_Times = db.Lesson_Times.Find(id);
                db.Lesson_Times.Remove(lesson_Times);
                db.SaveChanges();
                return Json(new { result = "lesson_time_deleted" }, JsonRequestBehavior.AllowGet);
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
