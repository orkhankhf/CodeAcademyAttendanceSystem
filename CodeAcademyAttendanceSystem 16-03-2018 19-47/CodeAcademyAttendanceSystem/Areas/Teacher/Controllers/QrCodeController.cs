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

namespace CodeAcademyAttendanceSystem.Areas.Teacher.Controllers
{
    public class QrCodeController : Controller
    {
        CodeAcademyAttendanceSystem_dbEntities db = new CodeAcademyAttendanceSystem_dbEntities();
        
        public ActionResult Generate(int group_id, string qr_code_value)
        {
            QrCodeEncodingOptions options = new QrCodeEncodingOptions();

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
            var result = new Bitmap(qr.Write(qr_code_value));
            string logo_img = Path.Combine(Server.MapPath("~/assets/img/"), "logo.png");
            Image logo = Image.FromFile(logo_img);
            int left = (result.Width / 2) - (logo.Width / 2);
            int top = (result.Height / 2) - (logo.Height / 2);
            Graphics g = Graphics.FromImage(result);
            g.DrawImage(logo, new Point(left, top));
            MemoryStream stream = new MemoryStream();
            result.Save(stream, ImageFormat.Jpeg);
            byte[] byteArray = stream.GetBuffer();

            Qr_Codes current_qr_code = db.Qr_Codes.Where(q => q.qr_codes_date == today && q.qr_codes_group_id == group_id).FirstOrDefault();

            if (current_qr_code != null)
            {
                current_qr_code.qr_codes_value = qr_code_value;
            }
            else
            {
                var Qr_Code = new Qr_Codes()
                {
                    qr_codes_date = today,
                    qr_codes_status = true,
                    qr_codes_value = qr_code_value,
                    qr_codes_group_id = group_id
                };
                db.Qr_Codes.Add(Qr_Code);

                List<int> students = db.Students.Where(s => s.student_group_id == group_id && s.student_status == true).Select(s => s.student_id).ToList();


                foreach (var item in students)
                {
                    Students_Attendance student_attendance = db.Students_Attendance.Where(a => a.students_attendance_student_id == item && a.students_attendance_date == today).FirstOrDefault();
                    if (student_attendance == null)
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
            }
            db.SaveChanges();
            return File(byteArray, "image/jpeg");
        }
    }
}