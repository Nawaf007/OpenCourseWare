using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace OpenCourseWareProject.Controllers
{
    public class MaterialsController : Controller
    {
        private OpenCourseWareDbEntities db = new OpenCourseWareDbEntities();

        public FileResult Download(string path)
        {
            if (User.Identity.IsAuthenticated && db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
            {
                ViewBag.Admin = 2;
            }
            else if(User.Identity.IsAuthenticated && (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin"))
            {
                ViewBag.Admin = 1;
            }
            else
            {
                ViewBag.Admin = 0;
            }
            if(path != "Null" && System.IO.File.Exists(Server.MapPath("~") + path))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~") + path);
                string fileName = path.Substring(11);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            return null;
        }
        // GET: Materials
        public ActionResult Index(int id, string type)
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
            if (db.Courses.Find(id).Status == "Archive")
            {
                return HttpNotFound();
            }
            var materials = db.Materials.Where(m => m.CId == id &&  m.Type == type);
            ViewBag.Type = type;
            return View(materials.ToList());
        }

        public ActionResult Table(int? id, string sortOrder = null, string currentFilter = null, int page = 1, int pageSize = 25)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentFilter = String.IsNullOrEmpty(currentFilter) ? "" : currentFilter;
                page = page > 0 ? page : 1;
                pageSize = pageSize > 0 ? pageSize : 25;

                ViewBag.TypeSortParm = sortOrder == "Type" ? "Type_desc" : "Type";
                ViewBag.TitleSortParm = sortOrder == "Title" ? "Title_desc" : "Title";

                ViewBag.CurrentSort = sortOrder;

                if (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin")
                {
                    ViewBag.Admin = 1;
                    Course course = db.Courses.Find(id);
                    if(course == null || course.AspNetUser.Id != User.Identity.GetUserId())
                    {
                        return HttpNotFound();
                    }
                    var materials = db.Materials.Where(m => m.CId == id);
                    ViewBag.CId = id;
                    if (!String.IsNullOrEmpty(currentFilter))
                    {
                        materials = materials.Where(m => m.Type.Contains(currentFilter) || m.Title.Contains(currentFilter));
                    }
                    switch (sortOrder)
                    {
                        case "Type":
                            materials = materials.OrderBy(x => x.Type);
                            break;
                        case "Type_desc":
                            materials = materials.OrderByDescending(x => x.Type);
                            break;
                        case "Title":
                            materials = materials.OrderBy(x => x.Title);
                            break;
                        case "Title_desc":
                            materials = materials.OrderByDescending(x => x.Title);
                            break;
                        default:
                            materials = materials.OrderBy(x => x.MId);
                            break;
                    }
                    return View(materials.ToPagedList(page, pageSize));
                }
                if(db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
                {
                    ViewBag.Admin = 2;
                    Course course = db.Courses.Find(id);
                    if (course == null)
                    {
                        return HttpNotFound();
                    }
                    var materials = db.Materials.Where(m => m.CId == id);
                    ViewBag.CId = id;
                    if (!String.IsNullOrEmpty(currentFilter))
                    {
                        materials = materials.Where(m => m.Type.Contains(currentFilter) || m.Title.Contains(currentFilter));
                    }
                    switch (sortOrder)
                    {
                        case "Type":
                            materials = materials.OrderBy(x => x.Type);
                            break;
                        case "Type_desc":
                            materials = materials.OrderByDescending(x => x.Type);
                            break;
                        case "Title":
                            materials = materials.OrderBy(x => x.Title);
                            break;
                        case "Title_desc":
                            materials = materials.OrderByDescending(x => x.Title);
                            break;
                        default:
                            materials = materials.OrderBy(x => x.MId);
                            break;
                    }
                    return View(materials.ToPagedList(page, pageSize));
                }
            }
            ViewBag.Admin = 0;
            return HttpNotFound();
        }
        /*
        public ActionResult _Index(int? id)
        {
            var materials = db.Materials.Where(m => m.Course.CId == id);
            return PartialView(materials.ToList());
        }
        
        // GET: Materials/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Materials.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            return View(material);
        }
        */
        // GET: Materials/Create
        public ActionResult Create(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin" )
                {
                    ViewBag.Admin = 1;
                    Course course = db.Courses.Find(id);
                    if (course == null || course.AspNetUser.Id != User.Identity.GetUserId())
                    {
                        return HttpNotFound();
                    }
                    Material material = new Material();
                    material.CId = id;
                    material.FilePath = "null";
                    material.Course = db.Courses.Find(id);
                    return View(material);
                }
                if(db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
                {
                    ViewBag.Admin = 2;
                    Course course = db.Courses.Find(id);
                    if (course == null)
                    {
                        return HttpNotFound();
                    }
                    Material material = new Material();
                    material.CId = id;
                    material.FilePath = "null";
                    material.Course = db.Courses.Find(id);
                    return View(material);
                }
            }
            ViewBag.Admin = 0;
            return HttpNotFound();
        }

        // POST: Materials/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MId,Title,Type,CId,FilePath")] Material material, HttpPostedFileBase file)
        {
            if (User.Identity.IsAuthenticated && (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin" || db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin"))
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
                    db.Materials.Add(material);
                    db.SaveChanges();
                    if (file != null && file.ContentLength > 0)
                    {
                        string path = System.IO.Path.Combine(Server.MapPath("~/Materials"), material.MId.ToString() + System.IO.Path.GetExtension(file.FileName));
                        material.FilePath = "/Materials/" + material.MId + System.IO.Path.GetExtension(file.FileName);
                        file.SaveAs(path);
                        db.SaveChanges();
                    }
                    ViewBag.CId = material.CId;
                    return RedirectToAction("Table", new { id = material.CId });
                }

                ViewBag.CId = material.CId;
                return View(material);
            }
            ViewBag.Admin = 0;
            return HttpNotFound();
        }

        /*
        public ActionResult _Create()
        {
            ViewBag.CId = new SelectList(db.Courses, "CId", "CourseId");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create([Bind(Include = "MId,Title,Type,CId")] Material material)
        {
            if (ModelState.IsValid)
            {
                db.Materials.Add(material);
                db.SaveChanges();
                return null;
            }

            ViewBag.CId = new SelectList(db.Courses, "CId", "CourseId", material.CId);
            return null;
        }
        */
        // GET: Materials/Edit/5
        public ActionResult Edit(int? id)
        {
            if(User.Identity.IsAuthenticated)
            {
                if (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin")
                {
                    ViewBag.Admin = 1;
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Material material = db.Materials.Find(id);
                    if (material == null)
                    {
                        return HttpNotFound();
                    }
                    Course course = db.Courses.Find(material.CId);
                    if (course == null || course.AspNetUser.Id != User.Identity.GetUserId())
                    {
                        return HttpNotFound();
                    }
                    ViewBag.CId = new SelectList(db.Courses, "CId", "CourseId", material.CId);
                    return View(material);
                }
                if(db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
                {
                    ViewBag.Admin = 2;
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Material material = db.Materials.Find(id);
                    if (material == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.CId = new SelectList(db.Courses, "CId", "CourseId", material.CId);
                    return View(material);
                }
            }
            ViewBag.Admin = 0;
            return HttpNotFound();
        }

        // POST: Materials/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MId,Title,Type,CId,FilePath")] Material material, HttpPostedFileBase file)
        {
            if (User.Identity.IsAuthenticated && (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin" || db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin"))
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
                    db.Entry(material).State = EntityState.Modified;
                    db.SaveChanges();
                    if (file != null && file.ContentLength > 0)
                    {
                        if (System.IO.File.Exists(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + material.FilePath))
                        {
                            System.IO.File.Delete(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + material.FilePath);
                        }
                        string path = System.IO.Path.Combine(Server.MapPath("~/Materials"), material.MId.ToString() + System.IO.Path.GetExtension(file.FileName));
                        material.FilePath = "/Materials/" + material.MId + System.IO.Path.GetExtension(file.FileName);
                        file.SaveAs(path);
                        db.SaveChanges();
                    }
                    ViewBag.CId = material.CId;
                    return RedirectToAction("Table", new { id = material.CId });
                }
                ViewBag.CId = material.CId;
                return View(material);
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
            Material material = db.Materials.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            ViewBag.CId = new SelectList(db.Courses, "CId", "CourseId", material.CId);
            return PartialView(material);
        }

        // POST: Materials/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Edit([Bind(Include = "MId,Title,Type,CId")] Material material)
        {
            if (ModelState.IsValid)
            {
                db.Entry(material).State = EntityState.Modified;
                db.SaveChanges();
                Material material2 = new Material();
                return null;
            }
            ViewBag.CId = new SelectList(db.Courses, "CId", "CourseId", material.CId);
            return null;
        }
        */
        // GET: Materials/Delete/5
        public ActionResult Delete(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "Admin")
                {
                    ViewBag.Admin = 1;
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Material material = db.Materials.Find(id);
                    if (material == null)
                    {
                        return HttpNotFound();
                    }
                    Course course = db.Courses.Find(material.CId);
                    if(course == null || course.AspNetUser.Id != User.Identity.GetUserId())
                    {
                        return HttpNotFound();
                    }
                    if (System.IO.File.Exists(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + material.FilePath))
                    {
                        System.IO.File.Delete(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + material.FilePath);
                    }
                    db.Materials.Remove(material);
                    db.SaveChanges();
                    ViewBag.CId = id;
                    return RedirectToAction("Table", new { id = material.CId });
                }
                if(db.AspNetUsers.Find(User.Identity.GetUserId()).Admin == "SuperAdmin")
                {
                    ViewBag.Admin = 2;
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Material material = db.Materials.Find(id);
                    if (material == null)
                    {
                        return HttpNotFound();
                    }
                    Course course = db.Courses.Find(material.CId);
                    if (course == null)
                    {
                        return HttpNotFound();
                    }
                    if (System.IO.File.Exists(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + material.FilePath))
                    {
                        System.IO.File.Delete(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "../../" + material.FilePath);
                    }
                    db.Materials.Remove(material);
                    db.SaveChanges();
                    ViewBag.CId = id;
                    return RedirectToAction("Table", new { id = material.CId });
                }
                ViewBag.Admin = 1;
                return HttpNotFound();
            }
            ViewBag.Admin = 0;
            return HttpNotFound();
        }
        /*
        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Material material = db.Materials.Find(id);
            db.Materials.Remove(material);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult _Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Materials.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            return PartialView(material);
        }

        [HttpPost, ActionName("_Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult _DeleteConfirmed(int id)
        {
            Material material = db.Materials.Find(id);
            db.Materials.Remove(material);
            db.SaveChanges();
            Material material2 = new Material();
            return null;
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
