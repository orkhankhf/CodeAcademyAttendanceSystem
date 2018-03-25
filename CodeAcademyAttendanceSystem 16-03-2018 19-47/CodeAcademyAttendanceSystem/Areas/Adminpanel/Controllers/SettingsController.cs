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
    public class SettingsController : Controller
    {
        private CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();
        
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            System_Settings system_Settings = db.System_Settings.Find(id);
            if (system_Settings == null)
            {
                return HttpNotFound();
            }
            return View(system_Settings);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "system_settings_id,system_settings_academy_ip,system_settings_qr_code_available_minute")] System_Settings system_Settings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(system_Settings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit");
            }
            return View(system_Settings);
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
