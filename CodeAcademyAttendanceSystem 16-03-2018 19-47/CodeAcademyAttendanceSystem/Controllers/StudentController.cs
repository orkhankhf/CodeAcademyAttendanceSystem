using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeAcademyAttendanceSystem.Models;
using ZXing;

namespace CodeAcademyAttendanceSystem.Controllers
{
    public class StudentController : Controller
    {
        CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();

        // GET: Student
        public ActionResult Index()
        {
            Session.Remove("student_id");
            Session.Remove("student_group_id");
            return View();
        }

        [HttpPost]
        public ActionResult Index([Bind(Include = "student_email, student_password")] Students student)
        {
            try
            {
                Students std = db.Students.Where(s => s.student_email == student.student_email && s.student_password == student.student_password).Single();
                if (std != null)
                {
                    Session["student_id"] = std.student_id;
                    Session["student_group_id"] = std.student_group_id;
                    return RedirectToAction("ProfilePage");
                }
            }
            catch
            {
                return View();
            }
            return View();
        }

        public ActionResult ProfilePage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Decode(HttpPostedFileBase qr_code)
        {
            var photo_name = Path.GetFileName(qr_code.FileName);
            string photo_path = Path.Combine(Server.MapPath("/assets/img"), photo_name);
            qr_code.SaveAs(photo_path);
            IBarcodeReader reader = new BarcodeReader();
            var barcodeBitmap = ConvertToBitmap(Server.MapPath("/assets/img/" + photo_name));
            var result = reader.Decode(barcodeBitmap);
            if (result != null)
            {
                var Qr_Code_Value = result.Text;
                int group_id = Convert.ToInt32(HttpContext.Session["student_group_id"]);
                int student_id = Convert.ToInt32(HttpContext.Session["student_id"]);
                DateTime today = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                try
                {
                    Qr_Codes qr_codes_search = db.Qr_Codes.Where(q => q.qr_codes_group_id == group_id && q.qr_codes_date == today && q.qr_codes_value == Qr_Code_Value && q.qr_codes_status == true).Single();
                    if(qr_codes_search != null)
                    {
                        var std_attendance = db.Students_Attendance.Where(a=>a.students_attendance_student_id == student_id && a.students_attendance_date == today).Single();
                        std_attendance.students_attendance_sender_ip = "67.125.26.48";
                        std_attendance.students_attendance_status = true;
                        db.SaveChanges();
                    }
                    return RedirectToAction("ProfilePage", "Student");
                }
                catch
                {
                    return Content("Sizin grup ucun bele bir QR Code movcud deyil !");
                }
            }
            return Content("QR Code oxunmadi !");
        }


        public Bitmap ConvertToBitmap(string fileName)
        {
            Bitmap bitmap;
            using (Stream bmpStream = System.IO.File.Open(fileName, FileMode.Open))
            {
                Image image = Image.FromStream(bmpStream);

                bitmap = new Bitmap(image);

            }
            return bitmap;
        }
    }
}