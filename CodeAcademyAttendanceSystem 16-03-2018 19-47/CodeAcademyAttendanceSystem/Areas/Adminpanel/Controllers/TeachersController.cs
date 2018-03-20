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
using CodeAcademyAttendanceSystem.Models.PasswordSecurity;
using System.IO;

namespace CodeAcademyAttendanceSystem.Areas.Adminpanel.Controllers
{
    [AdminpanelLoginFilter]
    public class TeachersController : Controller
    {
        private CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();
        public string teacher_photo_name = null;

        [HttpGet]
        public ActionResult Index()
        {
            var teachers = db.Teachers.Include(t => t.Genders).Include(t => t.Role_Types);
            return View(teachers.ToList());
        }

        // GET: Adminpanel/Teachers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teachers teachers = db.Teachers.Find(id);
            if (teachers == null)
            {
                return HttpNotFound();
            }
            return View(teachers);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.teacher_gender_id = new SelectList(db.Genders, "gender_id", "gender_name");
            ViewBag.teacher_role_types_id = new SelectList(db.Role_Types, "role_types_id", "role_types_name");
            return View();
        }

        // POST: Adminpanel/Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "teacher_id,teacher_email,teacher_password,teacher_name,teacher_surname,teacher_phone,teacher_gender_id,teacher_role_types_id,teacher_first_login")] Teachers teachers, HttpPostedFileBase teacher_photo)
        {
            if (ModelState.IsValid)
            {
                if (teacher_photo != null)
                {
                    teacher_photo_name = (DateTime.Now.ToString("yyyyMMddHHmmss")) + Path.GetExtension(teacher_photo.FileName);
                    var emp_ID_proof_path = Path.Combine(Server.MapPath("~/Areas/Adminpanel/Assets-Adminpanel/Teacher_Photos"), teacher_photo_name);
                    teacher_photo.SaveAs(emp_ID_proof_path);
                }
                teachers.teacher_photo = teacher_photo_name;
                teachers.teacher_first_login = true;
                teachers.teacher_password = PasswordStorage.CreateHash("Code123456");
                db.Teachers.Add(teachers);
                db.SaveChanges();
                return RedirectToAction("Create", "Teachers");
            }

            ViewBag.teacher_gender_id = new SelectList(db.Genders, "gender_id", "gender_name", teachers.teacher_gender_id);
            ViewBag.teacher_role_types_id = new SelectList(db.Role_Types, "role_types_id", "role_types_name", teachers.teacher_role_types_id);
            return View(teachers);
        }

        // GET: Adminpanel/Teachers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teachers teachers = db.Teachers.Find(id);
            if (teachers == null)
            {
                return HttpNotFound();
            }
            ViewBag.teacher_gender_id = new SelectList(db.Genders, "gender_id", "gender_name", teachers.teacher_gender_id);
            ViewBag.teacher_role_types_id = new SelectList(db.Role_Types, "role_types_id", "role_types_name", teachers.teacher_role_types_id);
            return View(teachers);
        }

        // POST: Adminpanel/Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "teacher_id,teacher_email,teacher_password,teacher_name,teacher_surname,teacher_phone,teacher_photo,teacher_gender_id,teacher_role_types_id,teacher_first_login")] Teachers teachers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teachers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.teacher_gender_id = new SelectList(db.Genders, "gender_id", "gender_name", teachers.teacher_gender_id);
            ViewBag.teacher_role_types_id = new SelectList(db.Role_Types, "role_types_id", "role_types_name", teachers.teacher_role_types_id);
            return View(teachers);
        }

        // GET: Adminpanel/Teachers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teachers teachers = db.Teachers.Find(id);
            if (teachers == null)
            {
                return HttpNotFound();
            }
            return View(teachers);
        }

        // POST: Adminpanel/Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Teachers teachers = db.Teachers.Find(id);
            db.Teachers.Remove(teachers);
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
