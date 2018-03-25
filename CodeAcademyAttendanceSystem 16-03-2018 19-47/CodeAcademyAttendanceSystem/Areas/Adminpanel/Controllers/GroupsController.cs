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
    public class GroupsController : Controller
    {
        private CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();

        [HttpGet]
        public ActionResult Index()
        {
            var groups = db.Groups.Include(g => g.Group_Types).Include(g => g.Lesson_Times).Include(g => g.Teachers);
            return View(groups.ToList());
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Groups groups = db.Groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.group_group_type_id = new SelectList(db.Group_Types, "group_types_id", "group_types_name");
            ViewBag.group_lesson_times_id = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.group_schedule_id = new SelectList(db.Group_Schedule, "group_schedule_id", "group_schedule_name");
            ViewBag.group_teacher_id = new SelectList((from t in db.Teachers select new { t.teacher_id, teacher_fullname = t.teacher_name + " " + t.teacher_surname }), "teacher_id", "teacher_fullname", null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "group_id,group_name,group_start_date,group_end_date,group_lesson_times_id,group_teacher_id,group_group_type_id,group_status")] Groups groups)
        {
            if (ModelState.IsValid)
            {
                groups.group_status = true;
                db.Groups.Add(groups);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            ViewBag.group_group_type_id = new SelectList(db.Group_Types, "group_types_id", "group_types_name", groups.group_group_type_id);
            ViewBag.group_lesson_times_id = new SelectList(db.Lesson_Times, "lesson_times_id", "lesson_times_name", groups.group_lesson_times_id);
            ViewBag.group_teacher_id = new SelectList(db.Teachers, "teacher_id", "teacher_email", groups.group_teacher_id);
            return View(groups);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Groups groups = db.Groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            ViewBag.group_group_type_id = new SelectList(db.Group_Types, "group_types_id", "group_types_name", groups.group_group_type_id);
            ViewBag.group_lesson_times_id = new SelectList(db.Lesson_Times, "lesson_times_id", "lesson_times_name", groups.group_lesson_times_id);
            ViewBag.group_schedule_id = new SelectList(db.Group_Schedule, "group_schedule_id", "group_schedule_name", groups.Lesson_Times.Group_Schedule.group_schedule_id);
            ViewBag.group_teacher_id = new SelectList((from t in db.Teachers select new { t.teacher_id, teacher_fullname = t.teacher_name + " " + t.teacher_surname }), "teacher_id", "teacher_fullname",groups.group_teacher_id);
            return View(groups);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "group_id,group_name,group_start_date,group_end_date,group_lesson_times_id,group_teacher_id,group_group_type_id,group_status")] Groups groups)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groups).State = EntityState.Modified;
                groups.group_status = true;
                db.SaveChanges();

                //Class'ın hansı məqsədlə istifadə olunduğunu SetGroupsStatusByDate.cs faylında yazmışam.
                SetGroupsStatusByDate test = new SetGroupsStatusByDate();
                test.Run();

                return RedirectToAction("Index");
            }
            ViewBag.group_group_type_id = new SelectList(db.Group_Types, "group_types_id", "group_types_name", groups.group_group_type_id);
            ViewBag.group_lesson_times_id = new SelectList(db.Lesson_Times, "lesson_times_id", "lesson_times_name", groups.group_lesson_times_id);
            ViewBag.group_teacher_id = new SelectList(db.Teachers, "teacher_id", "teacher_email", groups.group_teacher_id);
            return View(groups);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetLessonTimes(int id)
        {
            List<Lesson_Times> lesson_times = db.Lesson_Times.Where(lt => lt.lesson_times_group_schedule_id == id).ToList();
            SelectList lesson_times_selectlist = new SelectList(lesson_times, "lesson_times_id", "lesson_times_name", 0);
            return Json(lesson_times_selectlist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Groups groups = db.Groups.Find(id);
                db.Groups.Remove(groups);
                db.SaveChanges();
                return Json(new { result = "group_deleted" }, JsonRequestBehavior.AllowGet);
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
