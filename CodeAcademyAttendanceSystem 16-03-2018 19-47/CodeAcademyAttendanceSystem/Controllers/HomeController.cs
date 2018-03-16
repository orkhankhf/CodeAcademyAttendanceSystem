using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Mvc;
using ZXing;
using System.Linq;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using CodeAcademyAttendanceSystem.Models;
using System.Collections.Generic;

namespace CodeAcademyAttendanceSystem.Controllers
{
    public class HomeController : Controller
    {
        CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Generate()
        {
            return View();
        }
        public ActionResult Generate_Qr_Code()
        {
            QrCodeEncodingOptions options = new QrCodeEncodingOptions();
            Random rand = new Random();
            string Qr_Code_Value = (rand.Next(100000000, 900000000)).ToString();
            int group_id = 1;
            DateTime today = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = 500,
                Height = 500
            };
            var writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;
            options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
            var qr = new BarcodeWriter();
            qr.Options = options;
            qr.Format = BarcodeFormat.QR_CODE;
            var result = new Bitmap(qr.Write(Qr_Code_Value));
            string logo_img = Path.Combine(Server.MapPath("~/assets/img/"),"logo.png");
            Image logo = Image.FromFile(logo_img);
            int left = (result.Width / 2) - (logo.Width / 2);
            int top = (result.Height / 2) - (logo.Height / 2);
            Graphics g = Graphics.FromImage(result);
            g.DrawImage(logo, new Point(left, top));
            MemoryStream stream = new MemoryStream();
            result.Save(stream, ImageFormat.Jpeg);
            byte[] byteArray = stream.GetBuffer();
            try
            {
                Qr_Codes qr_code = db.Qr_Codes.Where(q => q.qr_codes_date == today && q.qr_codes_group_id == group_id).Single();
                qr_code.qr_codes_value = Qr_Code_Value;
                qr_code.qr_codes_status = true;
            }
            catch
            {
                var Qr_Code = new Qr_Codes()
                {
                    qr_codes_date = today,
                    qr_codes_status = true,
                    qr_codes_value = Qr_Code_Value,
                    qr_codes_group_id = 1
                };
                db.Qr_Codes.Add(Qr_Code);

                List<int> students = db.Students.Where(s => s.student_group_id == group_id).Select(s => s.student_id).ToList();
                foreach (var item in students)
                {
                    var new_attendance = new Students_Attendance()
                    {
                        students_attendance_date = today,
                        students_attendance_sender_ip = null,
                        students_attendance_status = false,
                        students_attendance_student_id = item
                    };
                    db.Students_Attendance.Add(new_attendance);
                }
            }
            db.SaveChanges();
            return File(byteArray, "image/jpeg");
        }
    }
}