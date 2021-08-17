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
    public class tblReservesController : Controller
    {
        private LibraryDBContext db = new LibraryDBContext();

        // GET: tblReserves
        public ActionResult Index(string username)
        {
            //utilizes username cookie and shows only the users reservation
            username = Session["username"].ToString();
            var tblReserves = db.tblReserves.Include(t => t.tblBook).Include(t => t.tblUser);
            return View(db.tblReserves.Where(x => x.User_Name.Equals(username)).ToList());
        }

        public ActionResult AdminIndex(string username)
        {
            return View(db.tblReserves.ToList());
        }


        // GET: tblReserves/Create
        public ActionResult Create(int id)
        {
            //checks if the user has more than three book and if they do the are stopped
            string username = Session["username"].ToString();
            var limit = db.tblReserves.Where(x => x.User_Name == username).ToList();
            if (limit.Count <= 2)
            {
                Session["BookID"] = id;
                return View();
            }
            else
            {
                return RedirectToAction("index", "tblReserves");
            }

        }

        // POST: tblReserves/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,User_Id,User_Name,Book_Id,Book_Name,Date,Due_Date,Overdue_Fee")] tblReserve tblReserve, int id)
        {
            if (ModelState.IsValid)
            {
                ///fills in the info for the reservation
                tblReserve reserve = new tblReserve();
                tblBook book = new tblBook();
                db.tblReserves.Add(tblReserve);
                tblReserve.Book_Id = id;
                tblReserve.Date = DateTime.Now;
                tblReserve.Due_Date = DateTime.Now.AddDays(14); //Makes reservation expire in 14 days
                tblReserve.User_Name = Session["username"].ToString();
                tblReserve.User_Id = Int32.Parse(Session["UserID"].ToString());

                var bookreserve = db.tblBooks.Find(tblReserve.Book_Id);
                bookreserve.Availability = "no"; //changes the book to being unavailable
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Book_Id = new SelectList(db.tblBooks, "Id", "Book_Name", tblReserve.Book_Id);
            ViewBag.User_Id = new SelectList(db.tblUsers, "Id", "User_Name", tblReserve.User_Id);
            return View(tblReserve);
        }

        // GET: tblReserves/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblReserve tblReserve = db.tblReserves.Find(id);
            if (tblReserve == null)
            {
                return HttpNotFound();
            }
            ViewBag.Book_Id = new SelectList(db.tblBooks, "Id", "Book_Name", tblReserve.Book_Id);
            ViewBag.User_Id = new SelectList(db.tblUsers, "Id", "User_Name", tblReserve.User_Id);
            return View(tblReserve);
        }

        // POST: tblReserves/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,User_Id,User_Name,Book_Id,Book_Name,Date,Due_Date,Overdue_Fee")] tblReserve tblReserve)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblReserve).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }
            ViewBag.Book_Id = new SelectList(db.tblBooks, "Id", "Book_Name", tblReserve.Book_Id);
            ViewBag.User_Id = new SelectList(db.tblUsers, "Id", "User_Name", tblReserve.User_Id);
            return View(tblReserve);
        }

        // GET: tblReserves/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblReserve tblReserve = db.tblReserves.Find(id);
            if (tblReserve == null)
            {
                return HttpNotFound();
            }
            return View(tblReserve);
        }

        // POST: tblReserves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblReserve tblReserve = db.tblReserves.Find(id);
            var bookreserve = db.tblBooks.Find(tblReserve.Book_Id);
            bookreserve.Availability = "yes"; //changes the book to being available
            db.tblReserves.Remove(tblReserve);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: tblReserves/Delete/5
        public ActionResult AdminDelete(int? id)//Admin delete page to return back to admin page
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblReserve tblReserve = db.tblReserves.Find(id);
            if (tblReserve == null)
            {
                return HttpNotFound();
            }
            return View(tblReserve);
        }

        // POST: tblReserves/Delete/5
        [HttpPost, ActionName("AdminDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult AdminDeleteConfirmed(int id)
        {
            tblReserve tblReserve = db.tblReserves.Find(id);
            var bookreserve = db.tblBooks.Find(tblReserve.Book_Id);
            bookreserve.Availability = "yes";
            db.tblReserves.Remove(tblReserve);
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
