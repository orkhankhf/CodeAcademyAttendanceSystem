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
    public class LessonTimesController : Controller
    {
        private CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();

        // GET: Adminpanel/LessonTimes
        public ActionResult Index()
        {
            var lesson_Times = db.Lesson_Times.Include(l => l.Group_Schedule);
            return View(lesson_Times.ToList());
        }

        // GET: Adminpanel/LessonTimes/Details/5
        public ActionResult Details(int? id)
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
            return View(lesson_Times);
        }

        // GET: Adminpanel/LessonTimes/Create
        public ActionResult Create()
        {
            ViewBag.lesson_times_group_schedule_id = new SelectList(db.Group_Schedule, "group_schedule_id", "group_schedule_name");
            return View();
        }

        // POST: Adminpanel/LessonTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lesson_times_id,lesson_times_name,lesson_times_start_time,lesson_times_end_time,lesson_times_group_schedule_id")] Lesson_Times lesson_Times)
        {
            if (ModelState.IsValid)
            {
                db.Lesson_Times.Add(lesson_Times);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lesson_times_group_schedule_id = new SelectList(db.Group_Schedule, "group_schedule_id", "group_schedule_name", lesson_Times.lesson_times_group_schedule_id);
            return View(lesson_Times);
        }

        // GET: Adminpanel/LessonTimes/Edit/5
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

        // POST: Adminpanel/LessonTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Adminpanel/LessonTimes/Delete/5
        public ActionResult Delete(int? id)
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
            return View(lesson_Times);
        }

        // POST: Adminpanel/LessonTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lesson_Times lesson_Times = db.Lesson_Times.Find(id);
            db.Lesson_Times.Remove(lesson_Times);
            db.SaveChanges();
            return RedirectToAction("Index");
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
