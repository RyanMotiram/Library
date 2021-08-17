using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Library.Models;

namespace Library.Controllers
{
    public class tblBooksController : Controller
    {
        private LibraryDBContext db = new LibraryDBContext();

        // GET: tblBooks
        public ActionResult Index(string SearchBy, string search)
        {
            //shows books based on info given from search bar
            if (SearchBy == "Name")
            {
                return View(db.tblBooks.Where(x => x.Book_Name.Contains(search) && x.Availability != "no" || search == null).ToList());
            }
            else if (SearchBy == "Category")
            {
                return View(db.tblBooks.Where(x => x.Category.Contains(search) && x.Availability != "no" || search == null).ToList());
            }
            else
            {
                return View(db.tblBooks.Where(x => x.Availability != "no").ToList());
            }
        }

        public ActionResult AdminIndex(string SearchBy, string search)
        {
                return View(db.tblBooks.ToList());

        }

        // GET: tblBooks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBook tblBook = db.tblBooks.Find(id);
            if (tblBook == null)
            {
                return HttpNotFound();
            }
            return View(tblBook);
        }

        // GET: tblBooks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tblBooks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Book_Name,Category,ISBN,Availability")] tblBook tblBook)
        {
            if (ModelState.IsValid)
            {
                db.tblBooks.Add(tblBook);
                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }

            return View(tblBook);
        }


        // GET: tblBooks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBook tblBook = db.tblBooks.Find(id);
            if (tblBook == null)
            {
                return HttpNotFound();
            }
            return View(tblBook);
        }

        // POST: tblBooks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Book_Name,Category,ISBN,Availability")] tblBook tblBook)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblBook).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }
            return View(tblBook);
        }

        // GET: tblBooks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBook tblBook = db.tblBooks.Find(id);
            if (tblBook == null)
            {
                return HttpNotFound();
            }
            return View(tblBook);
        }

        // POST: tblBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblBook tblBook = db.tblBooks.Find(id);
            tblBook.Availability = "yes"; //Changes books availability to yes
            db.tblBooks.Remove(tblBook);
            db.SaveChanges();
            return RedirectToAction("AdminIndex");
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
