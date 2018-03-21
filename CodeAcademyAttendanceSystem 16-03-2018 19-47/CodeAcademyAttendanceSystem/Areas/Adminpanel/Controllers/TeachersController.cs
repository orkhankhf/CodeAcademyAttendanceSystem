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
    //[AdminpanelLoginFilter]
    public class TeachersController : Controller
    {
        private CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();
        public string teacher_photo_name = null;

        [HttpGet]
        public ActionResult Index()
        {
            var teachers = db.Teachers.Include(t => t.Genders).Include(t => t.Role_Types);
            return View(teachers.OrderByDescending(t=>t.teacher_id).ToList());
        }

        [HttpGet]
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "teacher_id,teacher_email,teacher_password,teacher_name,teacher_surname,teacher_phone,teacher_gender_id,teacher_role_types_id,teacher_first_login")] Teachers teachers, HttpPostedFileBase teacher_photo)
        {
            if (ModelState.IsValid)
            {
                if (teacher_photo != null)
                {
                    teacher_photo_name = (DateTime.Now.ToString("yyyyMMddHHmmss")) + Path.GetExtension(teacher_photo.FileName);
                    var teacher_photo_path = Path.Combine(Server.MapPath("~/Areas/Adminpanel/Assets-Adminpanel/Teacher_Photos"), teacher_photo_name);
                    teacher_photo.SaveAs(teacher_photo_path);
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

        [HttpGet]
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "teacher_id,teacher_email,teacher_name,teacher_surname,teacher_phone,teacher_gender_id,teacher_role_types_id,teacher_first_login")] Teachers teachers, string reset_password, HttpPostedFileBase teacher_photo)
        {
            if (ModelState.IsValid)
            {
                //Update olunacaq müəllimi databazadan gətir
                Teachers update_teacher = db.Teachers.Find(teachers.teacher_id);

                update_teacher.teacher_email = teachers.teacher_email;
                update_teacher.teacher_name = teachers.teacher_name;
                update_teacher.teacher_surname = teachers.teacher_surname;
                update_teacher.teacher_phone = teachers.teacher_phone;
                update_teacher.teacher_gender_id = teachers.teacher_gender_id;
                update_teacher.teacher_role_types_id = teachers.teacher_role_types_id;




                //Əgər update formunda reset password check edilibsə
                if (reset_password == "true")
                {
                    //Müəllimin şifrəsini yenidən Code123456 olaraq şifrələyir və təyin edir
                    update_teacher.teacher_password = PasswordStorage.CreateHash("Code123456");
                    update_teacher.teacher_first_login = true;
                }

                //Əgər update formunda şəkil seçilibsə
                if (teacher_photo != null)
                {
                    //Əgər müəllimin əvvəldən şəkli olubsa (null deyilsə)
                    if (update_teacher.teacher_photo != null)
                    {
                        teacher_photo_name = (Path.GetFileNameWithoutExtension(update_teacher.teacher_photo) + Path.GetExtension(teacher_photo.FileName));
                        var teacher_photo_path = Path.Combine(Server.MapPath("~/Areas/Adminpanel/Assets-Adminpanel/Teacher_Photos"), teacher_photo_name);
                        teacher_photo.SaveAs(teacher_photo_path);
                    }
                    else
                    {
                        //Müəllimin əvvəldən şəkli olmayıbsa yeni şəkil adı generate edib databazaya atır və eynilədə şəkillər papkasına atır.
                        teacher_photo_name = (DateTime.Now.ToString("yyyyMMddHHmmss")) + Path.GetExtension(teacher_photo.FileName);
                        var teacher_photo_path = Path.Combine(Server.MapPath("~/Areas/Adminpanel/Assets-Adminpanel/Teacher_Photos"), teacher_photo_name);
                        teacher_photo.SaveAs(teacher_photo_path);
                        update_teacher.teacher_photo = teacher_photo_name;
                    }
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.teacher_gender_id = new SelectList(db.Genders, "gender_id", "gender_name", teachers.teacher_gender_id);
            ViewBag.teacher_role_types_id = new SelectList(db.Role_Types, "role_types_id", "role_types_name", teachers.teacher_role_types_id);
            return View(teachers);
        }

        // POST: Adminpanel/Teachers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Teachers teachers = db.Teachers.Find(id);
                db.Teachers.Remove(teachers);
                db.SaveChanges();
                return Json(new { result = "teacher_deleted" }, JsonRequestBehavior.AllowGet);
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
