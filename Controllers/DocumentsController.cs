using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DMS;
using DMS.Models;
using System.IO;
using System.Net.Mime;

namespace DMS.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private DMSContext db = new DMSContext();

        // GET: Documents
        public ActionResult Index(int? type, int? executor, DateTime? firstDate, DateTime? secondDate, int page = 1)
        {
            var user = db.Users.FirstOrDefault(item => item.Login == User.Identity.Name);
            var documents = db.Documents.Include(d => d.Class).Include(d => d.Executor).Include(d => d.User);
            var role = db.Roles.FirstOrDefault(item => item.Id == user.RoleId);
            if (role.Name == "user" || role.Name == "employee")
            {
                documents = documents.Where(item => item.User.Id == user.Id || item.Executor.Id == user.Id);
            }
            if(role.Name == "manager")
            {
                documents = documents.Where(item => (item.User.DepartmentId == user.DepartmentId || item.Executor.DepartmentId == user.DepartmentId));
            }
            if (executor != null && executor != 0)
                documents = documents.Where(item => item.ExecutorId == executor);
            if (type != null && type != 0)
                documents = documents.Where(item => item.ClassId == type);
            if(firstDate != null)
                documents = documents.Where(item => item.CreationTime >= firstDate);
            if (secondDate != null)
                documents = documents.Where(item => item.CreationTime <= secondDate);
            var types = db.Classes.ToList();
            types.Insert(0, new Class { Name = "Все", Id = 0 });
            var executors = db.Users.ToList();
            executors.Insert(0, new User { Name = "Все", Id = 0 });
            int pageSize = 5; 
            IEnumerable<Document> documentsPerPages = documents.ToList().Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = documents.Count() };
            DocumentViewModel dvm = new DocumentViewModel
            {
                Classes = new SelectList(types, "Id", "Name"),
                Executors = new SelectList(executors, "Id", "Name"),
                Documents = documentsPerPages.ToList()
            };
            DocumentIndexViewModel ivm = new DocumentIndexViewModel { PageInfo = pageInfo, Documents = dvm };
            return View(ivm);
        }

        // GET: Documents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: Documents/Create
        public ActionResult Create()
        {
            ViewBag.ClassId = new SelectList(db.Classes, "Id", "Name");
            ViewBag.ExecutorId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Documents/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Number,ClassId,ExecutorId,UserId,CreationTime,ExecutionTime")] Document document, HttpPostedFileBase file)
        {
            var fileName = Guid.NewGuid() + Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
            file.SaveAs(path);
            document.Number = fileName;
            document.CreationTime = DateTime.Now;
            document.UserId = db.Users.FirstOrDefault(item => item.Login == User.Identity.Name).Id;
            db.Documents.Add(document);
            db.SaveChanges();
            return RedirectToAction("Index");

            //ViewBag.ClassId = new SelectList(db.Classes, "Id", "Name", document.ClassId);
            //ViewBag.ExecutorId = new SelectList(db.Users, "Id", "Name", document.ExecutorId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "Name", document.UserId);
            //return View(document);
        }

        public FileResult Open(int id)
        {
            var document = db.Documents.FirstOrDefault(item => item.Id == id);
            var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), document.Number);
            return File(path, MediaTypeNames.Application.Octet, Path.GetFileName(path));
        }

        // GET: Documents/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassId = new SelectList(db.Classes, "Id", "Name", document.ClassId);
            ViewBag.ExecutorId = new SelectList(db.Users, "Id", "Name", document.ExecutorId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", document.UserId);
            return View(document);
        }

        // POST: Documents/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Number,ClassId,ExecutorId,UserId,CreationTime,ExecutionTime")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassId = new SelectList(db.Classes, "Id", "Name", document.ClassId);
            ViewBag.ExecutorId = new SelectList(db.Users, "Id", "Name", document.ExecutorId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", document.UserId);
            return View(document);
        }

        // GET: Documents/Delete/5

        [Authorize(Roles = "admin, moderator")]
        public ActionResult Close(int id)
        {
            Document document = db.Documents.Find(id);
            document.ExecutionTime = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Documents/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            Document document = db.Documents.Find(id);
            db.Documents.Remove(document);
            db.SaveChanges();
            return RedirectToAction("Index");
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
