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
    public class GroupTypesController : Controller
    {
        private CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();

        [HttpGet]
        public ActionResult Index()
        {
            return View(db.Group_Types.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "group_types_id,group_types_name")] Group_Types group_Types)
        {
            if (ModelState.IsValid)
            {
                db.Group_Types.Add(group_Types);
                db.SaveChanges();
                return RedirectToAction("Create", "GroupTypes");
            }

            return View(group_Types);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group_Types group_Types = db.Group_Types.Find(id);
            if (group_Types == null)
            {
                return HttpNotFound();
            }
            return View(group_Types);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "group_types_id,group_types_name")] Group_Types group_Types)
        {
            if (ModelState.IsValid)
            {
                Group_Types update_group_type = db.Group_Types.Find(group_Types.group_types_id);
                update_group_type.group_types_name = group_Types.group_types_name;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group_Types);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Group_Types group_Types = db.Group_Types.Find(id);
                db.Group_Types.Remove(group_Types);
                db.SaveChanges();
                return Json(new { result = "group_type_deleted" }, JsonRequestBehavior.AllowGet);
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
