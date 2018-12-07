using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace OpenCourseWareProject.Controllers
{
    public class MyCourseController : Controller
    {
        private OpenCourseWareDbEntities db = new OpenCourseWareDbEntities();
        // GET: MyCourse
        public ActionResult Index(string currentFilter = null, int page = 1, int pageSize = 25)
        {
            if(User.Identity.IsAuthenticated)
            {
                if (User.Identity.IsAuthenticated && (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin"))
                {
                    ViewBag.Admin = 1;
                }
                else if(User.Identity.IsAuthenticated && db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
                {
                    ViewBag.Admin = 2;
                }
                else
                {
                    ViewBag.Admin = 0;
                }
                ViewBag.CurrentFilter = String.IsNullOrEmpty(currentFilter) ? "" : currentFilter;
                page = page > 0 ? page : 1;
                pageSize = pageSize > 0 ? pageSize : 25;
                string id = User.Identity.GetUserId();
                var enrolled = db.MyCourses.Where(e => e.UserId == id).ToList();
                List<Course> courses = new List<Course>();
                foreach (MyCourses item in enrolled)
                {
                    var curr = db.Courses.Where(c => c.CId == item.CId && c.Status == "Active").ToList();
                    foreach (Course item2 in curr)
                    {
                        courses.Add(item2);
                    }
                }
                if (!String.IsNullOrEmpty(currentFilter))
                {
                    var courses2 = courses.Where(c => c.CourseId.Contains(currentFilter) || c.CourseTitle.Contains(currentFilter) || c.Department.Contains(currentFilter) || c.Instructor.Contains(currentFilter));
                    courses2 = courses2.OrderBy(c => c.CId);
                    return View(courses2.ToPagedList(page, pageSize));
                }
                var courses3 = courses.OrderBy(c => c.CId);
                return View(courses3.ToPagedList(page, pageSize));
            }
            return HttpNotFound();
        }
    }
}