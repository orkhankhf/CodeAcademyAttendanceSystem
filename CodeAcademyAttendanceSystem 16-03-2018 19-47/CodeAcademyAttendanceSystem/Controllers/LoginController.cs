﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeAcademyAttendanceSystem.Models;
using CodeAcademyAttendanceSystem.Models.PasswordSecurity;
using CodeAcademyAttendanceSystem.Filters;

namespace CodeAcademyAttendanceSystem.Controllers
{
    public class LoginController : Controller
    {
        CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        

        [HttpPost]
        public ActionResult Index([Bind(Include = "teacher_email, teacher_password")] Teachers teacher_login_form)
        {

            Teachers check_teacher_email = db.Teachers.Where(t => t.teacher_email == teacher_login_form.teacher_email).FirstOrDefault();
            if (check_teacher_email == null)
            {
                ViewBag.LoginError = "Email düzgün daxil edilməyib!";
                return View();
            }
            
            if (!PasswordStorage.VerifyPassword(teacher_login_form.teacher_password, check_teacher_email.teacher_password))
            {
                ViewBag.LoginError = "Şifrə düzgün daxil edilməyib!";
                return View();
            }

            Role_Types teacher_role = db.Role_Types.Where(r => r.role_types_id == check_teacher_email.teacher_role_types_id).FirstOrDefault();
            
            if (Convert.ToBoolean(check_teacher_email.teacher_first_login))
            {
                Session["FirstLogin_Email"] = check_teacher_email.teacher_email;
                Session["FirstLogin_Id"] = check_teacher_email.teacher_id.ToString();
                return RedirectToAction("SetNewPassword", "Login");
            }
            

            return RedirectToAction("Index", "Dashboard", new { Area = "Adminpanel" });
        }


        [HttpGet]
        [SetNewPasswordFilter]
        public ActionResult SetNewPassword()
        {
            return View();
        }


        [HttpPost]
        [SetNewPasswordFilter]
        public ActionResult SetNewPassword(string password, string confirm_password)
        {
            if(password != confirm_password)
            {
                ViewBag.SetNewPasswordError = "Şifrələr eyni deyil!";
                return View();
            }
            else
            {
                int Teacher_Id = Convert.ToInt32(Session["FirstLogin_Id"]);
                string Teacher_Email = Session["FirstLogin_Email"].ToString();
                string hashed_password = PasswordStorage.CreateHash(password);
                Teachers teacher = db.Teachers.FirstOrDefault(t => t.teacher_id == Teacher_Id && t.teacher_email == Teacher_Email);
                teacher.teacher_first_login = false;
                teacher.teacher_password = hashed_password;
                db.SaveChanges();
                Session.Remove("FirstLogin_Id");
                Session.Remove("FirstLogin_Email");
                TempData["PasswordChanged"] = "Yeni şifrənizlə hesabınıza daxil olun.";
                return RedirectToAction("Index", "Login");
            }
        }
    }
}