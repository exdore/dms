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

namespace DMS.Controllers
{
    public class DocumentsController : Controller
    {
        private DMSContext db = new DMSContext();

        // GET: Documents
        public ActionResult Index(int? type, DateTime? firstDate, DateTime? secondDate)
        {
            var user = db.Users.FirstOrDefault(item => item.Login == User.Identity.Name);
            var documents = db.Documents.Include(d => d.Class).Include(d => d.Executor).Include(d => d.User)
                .Where(item => item.User.Id == user.Id || item.Executor.Id == user.Id);
            if (type != null && type != 0)
                documents = documents.Where(item => item.ClassId == type);
            if(firstDate != null)
                documents = documents.Where(item => item.CreationTime >= firstDate);
            if (secondDate != null)
                documents = documents.Where(item => item.CreationTime <= secondDate);
            var types = db.Classes.ToList();
            types.Insert(0, new Class { Name = "Все", Id = 0 });
            DocumentViewModel dvm = new DocumentViewModel
            {
                Classes = new SelectList(types, "Id", "Name"),
                Documents = documents.ToList()
            };
            return View(dvm);
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
        public ActionResult Create([Bind(Include = "Id,Number,ClassId,ExecutorId,UserId,CreationTime,ExecutionTime")] Document document)
        {
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
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
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

        [Authorize(Roles = "admin")]
        public ActionResult Close(int id)
        {
            Document document = db.Documents.Find(id);
            document.ExecutionTime = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Documents/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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
