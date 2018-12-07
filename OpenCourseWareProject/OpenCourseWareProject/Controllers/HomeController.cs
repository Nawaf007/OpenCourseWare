using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpenCourseWareProject.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OpenCourseWareProject.Controllers
{
    public class HomeController : Controller
    {
        private OpenCourseWareDbEntities db = new OpenCourseWareDbEntities();
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated && db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
            {
                ViewBag.Admin = 2;
            }
            else if (User.Identity.IsAuthenticated && (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin"))
            {
                ViewBag.Admin = 1;
            }
            else
            {
                ViewBag.Admin = 0;
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            if (User.Identity.IsAuthenticated && db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
            {
                ViewBag.Admin = 2;
            }
            else if (User.Identity.IsAuthenticated && (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin"))
            {
                ViewBag.Admin = 1;
            }
            else
            {
                ViewBag.Admin = 0;
            }
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            if (User.Identity.IsAuthenticated && db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
            {
                ViewBag.Admin = 2;
            }
            else if (User.Identity.IsAuthenticated && (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin"))
            {
                ViewBag.Admin = 1;
            }
            else
            {
                ViewBag.Admin = 0;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (User.Identity.IsAuthenticated && db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
            {
                ViewBag.Admin = 2;
            }
            else if (User.Identity.IsAuthenticated && (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin"))
            {
                ViewBag.Admin = 1;
            }
            else
            {
                ViewBag.Admin = 0;
            }
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                var Superadmin = db.AspNetUsers.Where(a => a.Admin == "SuperAdmin");
                foreach(AspNetUser item in Superadmin)
                {
                    message.To.Add(new MailAddress(item.Email));  // replace with valid value
                } 
                message.From = new MailAddress("nawaf.shaikh@hotmail.com");  // replace with valid value
                message.Subject = model.FromSubject;
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "nawaf.shaikh@hotmail.com",  // replace with valid value
                        Password = "nawaf007/"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.live.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Contact");
                }
            }
            return View(model);
        }
    }
}