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

namespace CodeAcademyAttendanceSystem.Areas.Adminpanel.Controllers
{
    [AdminpanelLoginFilter]
    public class StudentsController : Controller
    {
        private CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();

        [HttpGet]
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Genders).Include(s => s.Groups);
            return View(students.ToList());
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.student_gender_id = new SelectList(db.Genders, "gender_id", "gender_name");
            ViewBag.student_group_id = new SelectList(db.Groups.Where(g=>g.group_status == true), "group_id", "group_name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "student_id,student_email,student_password,student_name,student_surname,student_father_name,student_phone,student_group_id,student_gender_id,student_device_id,student_first_login,student_status")] Students students)
        {
            string check_student_email;
            try
            {
                check_student_email = db.Students.Where(s => s.student_email == students.student_email).FirstOrDefault().student_email;
            }
            catch
            {
                check_student_email = null;
            }
            if (check_student_email != null)
            {
                ViewBag.EmailExist = "Bu email artıq mövcuddur!";
                ViewBag.student_gender_id = new SelectList(db.Genders, "gender_id", "gender_name", students.student_gender_id);
                ViewBag.student_group_id = new SelectList(db.Groups, "group_id", "group_name", students.student_group_id);
                return View(students);
            }
            if (ModelState.IsValid)
            {
                students.student_status = true;
                students.student_first_login = true;
                students.student_password = PasswordStorage.CreateHash("CA123456");
                db.Students.Add(students);
                db.SaveChanges();
                return RedirectToAction("Create", "Students");
            }

            ViewBag.student_gender_id = new SelectList(db.Genders, "gender_id", "gender_name", students.student_gender_id);
            ViewBag.student_group_id = new SelectList(db.Groups, "group_id", "group_name", students.student_group_id);
            return View(students);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            ViewBag.student_gender_id = new SelectList(db.Genders, "gender_id", "gender_name", students.student_gender_id);
            ViewBag.student_group_id = new SelectList(db.Groups.Where(g=>g.group_status == true), "group_id", "group_name", students.student_group_id);
            return View(students);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "student_id,student_email,student_name,student_surname,student_father_name,student_phone,student_group_id,student_gender_id,student_device_id,student_first_login,student_status")] Students students, string reset_password, string close_account)
        {
            if (ModelState.IsValid)
            {
                Students update_student = db.Students.Find(students.student_id);

                update_student.student_name = students.student_name;
                update_student.student_surname = students.student_surname;
                update_student.student_father_name = students.student_father_name;
                update_student.student_email = students.student_email;
                update_student.student_group_id = students.student_group_id;
                update_student.student_phone = students.student_phone;
                update_student.student_gender_id = students.student_gender_id;

                //Əgər update formunda reset password check edilibsə
                if (reset_password == "true")
                {
                    //Tələbənin şifrəsini yenidən CA123456 olaraq şifrələyir və təyin edir
                    update_student.student_password = PasswordStorage.CreateHash("CA123456");
                    update_student.student_first_login = true;
                    update_student.student_device_id = null;
                }

                //Əgər Hesabı Bağla check edilibsə
                if (close_account == "true")
                {
                    update_student.student_status = false;
                }
                else
                {
                    update_student.student_status = true;
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.student_gender_id = new SelectList(db.Genders, "gender_id", "gender_name", students.student_gender_id);
            ViewBag.student_group_id = new SelectList(db.Groups, "group_id", "group_name", students.student_group_id);
            return View(students);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Students student = db.Students.Find(id);
                db.Students.Remove(student);
                db.SaveChanges();
                return Json(new { result = "student_deleted" }, JsonRequestBehavior.AllowGet);
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
