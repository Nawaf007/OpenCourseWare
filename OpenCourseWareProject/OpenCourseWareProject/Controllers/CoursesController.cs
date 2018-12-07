using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Web.Routing;
using System.Collections.Specialized;
using X.PagedList;

namespace OpenCourseWareProject.Controllers
{
    public class CoursesController : Controller
    {
        private OpenCourseWareDbEntities db = new OpenCourseWareDbEntities();

        // GET: Courses
        public ActionResult Index(string currentFilter = null, int page = 1, int pageSize = 25)
        {
            ViewBag.CurrentFilter = String.IsNullOrEmpty(currentFilter) ? "" : currentFilter;
            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 25;
            var courses = db.Courses.Where(c => c.Status == "Active");
            if (!String.IsNullOrEmpty(currentFilter))
            {
                courses = courses.Where(c => c.CourseId.Contains(currentFilter) || c.CourseTitle.Contains(currentFilter) || c.Department.Contains(currentFilter) || c.Instructor.Contains(currentFilter));
            }
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
            courses = courses.OrderBy(c => c.CId);
            return View(courses.ToPagedList(page, pageSize));
        }

        /*
        public ActionResult _Index()
        {
            string id = User.Identity.GetUserId();
            //var courses = db.Courses.Include(c => c.AspNetUser);
            var courses = from c in db.Courses where c.AspNetUser.Id == id select c;
            return PartialView(courses.OrderBy(x => x.CourseTitle).ToList());
        }*/

        public ActionResult Table(string sortOrder = null, string currentFilter = null, int page = 1, int pageSize = 25)
        {
            if (User.Identity.IsAuthenticated && (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin" || db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin"))
            {
                ViewBag.CurrentFilter = String.IsNullOrEmpty(currentFilter) ? "" : currentFilter;
                page = page > 0 ? page : 1;
                pageSize = pageSize > 0 ? pageSize : 25;

                ViewBag.IdSortParm = sortOrder == "Id" ? "Id_desc" : "Id";
                ViewBag.TitleSortParm = sortOrder == "Title" ? "Title_desc" : "Title";
                ViewBag.DepartmentSortParm = sortOrder == "Department" ? "Department_desc" : "Department";
                ViewBag.InstructorSortParm = sortOrder == "Instructor" ? "Instructor_desc" : "Instructor";
                ViewBag.StatusSortParm = sortOrder == "Status" ? "Status_desc" : "Status";
                ViewBag.AddedSortParm = sortOrder == "Added" ? "Added_desc" : "Added";

                ViewBag.CurrentSort = sortOrder;

                if (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
                {
                    ViewBag.Admin = 2;
                    string id = User.Identity.GetUserId();
                    //var courses = db.Courses.Include(c => c.AspNetUser);
                    var courses = from c in db.Courses where c.AspNetUser.Admin != "Just to move from loop" select c;
                    if(!String.IsNullOrEmpty(currentFilter))
                    {
                        courses = courses.Where(c => c.CourseId.Contains(currentFilter) || c.CourseTitle.Contains(currentFilter) || c.Department.Contains(currentFilter) || c.Instructor.Contains(currentFilter) || c.AspNetUser.Email.Contains(currentFilter));
                    }
                    switch (sortOrder)
                    {
                        case "Id":
                            courses = courses.OrderBy(x => x.CourseId);
                            break;
                        case "Id_desc":
                            courses = courses.OrderByDescending(x => x.CourseId);
                            break;
                        case "Title":
                            courses = courses.OrderBy(x => x.CourseTitle);
                            break;
                        case "Title_desc":
                            courses = courses.OrderByDescending(x => x.CourseTitle);
                            break;
                        case "Department":
                            courses = courses.OrderBy(x => x.Department);
                            break;
                        case "Department_desc":
                            courses = courses.OrderByDescending(x => x.Department);
                            break;
                        case "Instructor":
                            courses = courses.OrderBy(x => x.Instructor);
                            break;
                        case "Instructor_desc":
                            courses = courses.OrderByDescending(x => x.Instructor);
                            break;
                        case "Status":
                            courses = courses.OrderBy(x => x.Status);
                            break;
                        case "Status_desc":
                            courses = courses.OrderByDescending(x => x.Status);
                            break;
                        case "Added":
                            courses = courses.OrderBy(x => x.AspNetUser.Email);
                            break;
                        case "Added_desc":
                            courses = courses.OrderByDescending(x => x.AspNetUser.Email);
                            break;
                        default:
                            courses = courses.OrderBy(c => c.CId);
                            break;
                    }
                    return View(courses.ToPagedList(page, pageSize));
                }
                else
                {
                    ViewBag.Admin = 1;
                    string id = User.Identity.GetUserId();
                    //var courses = db.Courses.Include(c => c.AspNetUser);
                    var courses = from c in db.Courses where c.AspNetUser.Id == id select c;
                    if (!String.IsNullOrEmpty(currentFilter))
                    {
                        courses = courses.Where(c => c.CourseId.Contains(currentFilter) || c.CourseTitle.Contains(currentFilter) || c.Department.Contains(currentFilter) || c.Instructor.Contains(currentFilter) || c.AspNetUser.Email.Contains(currentFilter));
                    }
                    switch (sortOrder)
                    {
                        case "Id":
                            courses = courses.OrderBy(x => x.CourseId);
                            break;
                        case "Id_desc":
                            courses = courses.OrderByDescending(x => x.CourseId);
                            break;
                        case "Title":
                            courses = courses.OrderBy(x => x.CourseTitle);
                            break;
                        case "Title_desc":
                            courses = courses.OrderByDescending(x => x.CourseTitle);
                            break;
                        case "Department":
                            courses = courses.OrderBy(x => x.Department);
                            break;
                        case "Department_desc":
                            courses = courses.OrderByDescending(x => x.Department);
                            break;
                        case "Instructor":
                            courses = courses.OrderBy(x => x.Instructor);
                            break;
                        case "Instructor_desc":
                            courses = courses.OrderByDescending(x => x.Instructor);
                            break;
                        case "Status":
                            courses = courses.OrderBy(x => x.Status);
                            break;
                        case "Status_desc":
                            courses = courses.OrderByDescending(x => x.Status);
                            break;
                        case "Added":
                            courses = courses.OrderBy(x => x.AspNetUser.Email);
                            break;
                        case "Added_desc":
                            courses = courses.OrderByDescending(x => x.AspNetUser.Email);
                            break;
                        default:
                            courses = courses.OrderBy(c => c.CId);
                            break;
                    }
                    return View(courses.ToPagedList(page, pageSize));
                }
            }
            ViewBag.Admin = 0;
            return HttpNotFound();
        }

        public ActionResult Dashboard(string sortOrder = null, string currentFilter = null, int page = 1, int pageSize = 25)
        {
            if (User.Identity.IsAuthenticated && (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin" || db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin"))
            {
                ViewBag.CurrentFilter = String.IsNullOrEmpty(currentFilter) ? "" : currentFilter;
                page = page > 0 ? page : 1;
                pageSize = pageSize > 0 ? pageSize : 25;

                ViewBag.IdSortParm = sortOrder == "Id" ? "Id_desc" : "Id";
                ViewBag.TitleSortParm = sortOrder == "Title" ? "Title_desc" : "Title";
                ViewBag.DepartmentSortParm = sortOrder == "Department" ? "Department_desc" : "Department";

                ViewBag.CurrentSort = sortOrder;
                string id = User.Identity.GetUserId();
                var courses = from c in db.Courses where c.AspNetUser.Id == id select c;
                if (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
                {
                    ViewBag.Admin = 2;
                    courses = from c in db.Courses where true select c;
                }
                else
                {
                    ViewBag.Admin = 1;
                }
                if (!String.IsNullOrEmpty(currentFilter))
                {
                    courses = courses.Where(c => c.CourseId.Contains(currentFilter) || c.CourseTitle.Contains(currentFilter) || c.Department.Contains(currentFilter) || c.Instructor.Contains(currentFilter) || c.AspNetUser.Email.Contains(currentFilter));
                }
                switch (sortOrder)
                {
                    case "Id":
                        courses = courses.OrderBy(x => x.CourseId);
                        break;
                    case "Id_desc":
                        courses = courses.OrderByDescending(x => x.CourseId);
                        break;
                    case "Title":
                        courses = courses.OrderBy(x => x.CourseTitle);
                        break;
                    case "Title_desc":
                        courses = courses.OrderByDescending(x => x.CourseTitle);
                        break;
                    case "Department":
                        courses = courses.OrderBy(x => x.Department);
                        break;
                    case "Department_desc":
                        courses = courses.OrderByDescending(x => x.Department);
                        break;
                    default:
                        courses = courses.OrderBy(c => c.CId);
                        break;
                }
                var students = db.MyCourses.Where(c => true);
                HashSet<int> StudentIds = new HashSet<int>();
                ViewBag.count = courses.Count();
                ViewBag.Archived = 0;
                ViewBag.Active = 0;
                ViewBag.Students = 0;
                foreach (Course item in courses)
                {
                    if (item.Status == "Archive")
                    {
                        ViewBag.Archived = ViewBag.Archived + 1;
                    }
                    else
                    {
                        ViewBag.Active = ViewBag.Active + 1;
                    }
                    var student = from s in db.MyCourses where s.CId == item.CId select s;
                    foreach (MyCourses item2 in student)
                    {
                        if (item2.CId == item.CId)
                        {
                            StudentIds.Add(item2.Id);
                        }
                    }
                    ViewBag.Students = StudentIds.Count;
                }
                Tuple<IPagedList<Course>, IPagedList<MyCourses>> tuple = new Tuple<IPagedList<Course>, IPagedList<MyCourses>>(courses.ToPagedList(page, pageSize), students.ToPagedList());
                return View(tuple);
            }
            ViewBag.Admin = 0;
            return HttpNotFound();
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null || course.Status == "Archive")
            {
                return HttpNotFound();
            }
            string UserId = User.Identity.GetUserId();
            if(db.MyCourses.Where(m => m.CId == id && m.UserId == UserId).Count() > 0)
            {
                ViewBag.Added = true;
            }
            else
            {
                ViewBag.Added = false;
            }
            return View(course);
        }

        public ActionResult Add(int id)
        {
            if(User.Identity.IsAuthenticated)
            {
                if (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
                {
                    ViewBag.Admin = 2;
                }
                else if ((db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin"))
                {
                    ViewBag.Admin = 1;
                }
                else
                {
                    ViewBag.Admin = 0;
                }
    
                MyCourses courses = new MyCourses();
                courses.CId = id;
                courses.UserId = User.Identity.GetUserId();
                if (ModelState.IsValid)
                {
                    db.MyCourses.Add(courses);
                    db.SaveChanges();
                }
                return RedirectToAction("Details", new { id = id });
            }
            ViewBag.Admin = 0;
            return RedirectToAction("Login","Account");
        }

        public ActionResult Remove(int? id)
        {
            if(User.Identity.IsAuthenticated)
            {
                if (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
                {
                    ViewBag.Admin = 2;
                }
                else if ((db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin"))
                {
                    ViewBag.Admin = 1;
                }
                else
                {
                    ViewBag.Admin = 0;
                }
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var UserId = User.Identity.GetUserId();
                var courses = db.MyCourses.Where(c => c.CId == id && c.UserId == UserId);
                MyCourses course = courses.ToList()[0];
                if (course == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                db.MyCourses.Remove(course);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = id });
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult _Details(int? id)
        {
            if(User.Identity.IsAuthenticated)
            {
                if(db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin")
                {
                    ViewBag.Admin = 1;
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Course course = db.Courses.Find(id);
                    if (course == null || course.AspNetUser.Id != User.Identity.GetUserId().ToString())
                    {
                        return HttpNotFound();
                    }
                    return PartialView(course);
                }
                if(db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
                {
                    ViewBag.Admin = 2;
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Course course = db.Courses.Find(id);
                    if (course == null)
                    {
                        return HttpNotFound();
                    }
                    return PartialView(course);
                }
            }
            ViewBag.Admin = 0;
            return HttpNotFound();
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            if(User.Identity.IsAuthenticated && (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin" || db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin"))
            {
                if (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
                {
                    ViewBag.Admin = 2;
                }
                else 
                {
                    ViewBag.Admin = 1;
                }
                Course course = new Course();
                course.AddedBy = User.Identity.GetUserId().ToString();
                course.ImagePath = "null";
                ViewBag.AddedBy = User.Identity.GetUserId().ToString();
                return View(course);
            }
            ViewBag.Admin = 0;
            return HttpNotFound();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CId,CourseId,CourseTitle,Department,Instructor,Status,AddedBy,ImagePath")] Course course, HttpPostedFileBase file)
        {
            if(User.Identity.IsAuthenticated && (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin" || db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin"))
            {
                if (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
                {
                    ViewBag.Admin = 2;
                }
                else
                {
                    ViewBag.Admin = 1;
                }
                if (ModelState.IsValid)
                {
                    db.Courses.Add(course);
                    db.SaveChanges();
                    if (file != null && file.ContentLength > 0)
                    {
                        string path = System.IO.Path.Combine(Server.MapPath("~/images"), course.CId.ToString() + ".jpg");
                        course.ImagePath = "/images/" + course.CId + ".jpg";
                        file.SaveAs(path);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Table");
                }

                ViewBag.AddedBy = new SelectList(db.AspNetUsers, "Id", "Email", course.AddedBy);
                return View(course);
            }
            ViewBag.Admin = 0;
            return HttpNotFound();
        }

        /*public ActionResult _Create()
        {
            Course course = new Course();
            course.AddedBy = User.Identity.GetUserId().ToString();
            course.ImagePath = "null";
            ViewBag.AddedBy = User.Identity.GetUserId().ToString();
            return PartialView(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult _Create([Bind(Include = "CId,CourseId,CourseTitle,Department,Instructor,Status,AddedBy,ImagePath")] Course course, string answer, HttpPostedFileBase file)
        {
            if (ModelState.IsValid && answer == "Create")
            {
                db.Courses.Add(course);
                db.SaveChanges();
                if (file != null && file.ContentLength > 0)
                {
                    string path = System.IO.Path.Combine(Server.MapPath("~/images"), course.CId.ToString() + ".jpg");
                    course.ImagePath = "/images/" + course.CId + ".jpg";
                    file.SaveAs(path);
                    db.SaveChanges();
                }
                Course course2 = new Course();
                course2.AddedBy = User.Identity.GetUserId().ToString();
                return PartialView(course2);
            }

            ViewBag.AddedBy = new SelectList(db.AspNetUsers, "Id", "Email", course.AddedBy);
            return PartialView(course);
        }*/


        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if(User.Identity.IsAuthenticated)
            {
                if(db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin")
                {
                    ViewBag.Admin = 1;
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Course course = db.Courses.Find(id);
                    if (course == null || course.AspNetUser.Id != User.Identity.GetUserId().ToString())
                    {
                        return HttpNotFound();
                    }
                    ViewBag.AddedBy = new SelectList(db.AspNetUsers, "Id", "Email", course.AddedBy);
                    return View(course);
                }
                if(db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
                {
                    ViewBag.Admin = 2;
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Course course = db.Courses.Find(id);
                    if (course == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.AddedBy = new SelectList(db.AspNetUsers, "Id", "Email", course.AddedBy);
                    return View(course);
                }
            }
            ViewBag.Admin = 0;
            return HttpNotFound();
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CId,CourseId,CourseTitle,Department,Instructor,Status,AddedBy,ImagePath")] Course course, HttpPostedFileBase file)
        {
            if(User.Identity.IsAuthenticated && (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin" || db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin"))
            {
                if (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
                {
                    ViewBag.Admin = 2;
                }
                else
                {
                    ViewBag.Admin = 1;
                }
                if (ModelState.IsValid)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        if (System.IO.File.Exists(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + course.ImagePath))
                        {
                            System.IO.File.Delete(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + course.ImagePath);
                        }
                        string path = System.IO.Path.Combine(Server.MapPath("~/images"), course.CId.ToString() + ".jpg");
                        course.ImagePath = "/images/" + course.CId + ".jpg";
                        file.SaveAs(path);
                    }
                    db.Entry(course).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Table");
                }
                ViewBag.AddedBy = new SelectList(db.AspNetUsers, "Id", "Email", course.AddedBy);
                return View(course);
            }
            ViewBag.Admin = 0;
            return HttpNotFound();
        }

        /*
        public ActionResult _Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddedBy = new SelectList(db.AspNetUsers, "Id", "Email", course.AddedBy);
            ViewBag.Reload = 0;
            return PartialView(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Edit([Bind(Include = "CId,CourseId,CourseTitle,Department,Instructor,Status,AddedBy,ImagePath")] Course course, string answer, HttpPostedFileBase file)
        {
            if (ModelState.IsValid && answer == course.CId.ToString())
            {
                if (file != null && file.ContentLength > 0)
                {
                    string path = System.IO.Path.Combine(Server.MapPath("~/images"), course.CId.ToString() + ".jpg");
                    course.ImagePath = "/images/" + course.CId + ".jpg";
                    file.SaveAs(path);
                }
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Reload = 1;
                Course course2 = new Course();
                course2.AddedBy = User.Identity.GetUserId().ToString();
                return PartialView(course);
            }
            ViewBag.AddedBy = new SelectList(db.AspNetUsers, "Id", "Email", course.AddedBy);
            return PartialView(course);
        }
        */
        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if(User.Identity.IsAuthenticated)
            {
                if(db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin")
                {
                    ViewBag.Admin = 1;
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Course course = db.Courses.Find(id);
                    if (course == null || course.AspNetUser.Id != User.Identity.GetUserId().ToString())
                    {
                        return HttpNotFound();
                    }
                    if (System.IO.File.Exists(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + course.ImagePath))
                    {
                        System.IO.File.Delete(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + course.ImagePath);
                    }
                    var mat = db.Materials.Where(m => m.CId == course.CId);
                    foreach (Material item in mat)
                    {
                        db.Materials.Remove(item);
                    }
                    var curr = db.MyCourses.Where(c => c.CId == course.CId);
                    foreach (MyCourses item in curr)
                    {
                        db.MyCourses.Remove(item);
                    }
                    db.Courses.Remove(course);
                    db.SaveChanges();
                    return RedirectToAction("Table");
                }
                if(db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
                {
                    ViewBag.Admin = 2;
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Course course = db.Courses.Find(id);
                    if (course == null)
                    {
                        return HttpNotFound();
                    }
                    if (System.IO.File.Exists(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + course.ImagePath))
                    {
                        System.IO.File.Delete(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + course.ImagePath);
                    }
                    var mat = db.Materials.Where(m => m.CId == course.CId);
                    foreach (Material item in mat)
                    {
                        db.Materials.Remove(item);
                    }
                    var curr = db.MyCourses.Where(c => c.CId == course.CId);
                    foreach (MyCourses item in curr)
                    {
                        db.MyCourses.Remove(item);
                    }
                    db.Courses.Remove(course);
                    db.SaveChanges();
                    return RedirectToAction("Table");
                }
            }
            ViewBag.Admin = 0;
            return HttpNotFound();
        }

        /*
        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult _Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return PartialView(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("_Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult _DeleteConfirmed(int id, string delete)
        {
            if(delete == id.ToString())
            {
                Course course = db.Courses.Find(id);
                if (System.IO.File.Exists(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + course.ImagePath))
                {
                    System.IO.File.Delete(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + course.ImagePath);
                }
                db.Courses.Remove(course);
                db.SaveChanges();
            }
            Course course2 = new Course();
            course2.AddedBy = User.Identity.GetUserId().ToString();
            return PartialView(course2);
        }
        */
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
