using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using Library.Models;

namespace Library.Controllers
{
    public class tblUsersController : Controller
    {
        private LibraryDBContext db = new LibraryDBContext();

        // GET: tblUsers
        public ActionResult Index()
        {
            return View(db.tblUsers.ToList());
        }

        // GET: tblUsers
        public ActionResult AdminPage()
        {
            return View(db.tblUsers.ToList());
        }

        // GET: tblUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = db.tblUsers.Find(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }
            return View(tblUser);
        }

        // GET: tblUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tblUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,User_Name,Password,Name,Age,Address,Phone_Number,Admin")] tblUser tblUser)
        {
            try//to check if sql error appears due to not allowing the same username
            {
                if (ModelState.IsValid)
                {
                    if (this.IsCaptchaValid("Captcha is not valid"))//Captcha verification
                    {
                        db.tblUsers.Add(tblUser);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }

                    ViewBag.ErrMessage = "Error: captcha is not valid.";//error message if captcha incorrect
                    return View();

                }
            }
            catch (Exception ex)
            {
                //error message if user is using an existing username
                ModelState.AddModelError("", "This username is taken! Please Choose another!");
            }
            return View(tblUser);
        }

        // GET: tblUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = db.tblUsers.Find(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }
            return View(tblUser);
        }

        // POST: tblUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,User_Name,Password,Name,Age,Address,Phone_Number,Admin")] tblUser tblUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblUser);
        }

        // GET: tblUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = db.tblUsers.Find(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }
            return View(tblUser);
        }

        // POST: tblUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {   //changes books that were reserved by user who is being deleted to available
            db.tblReserves.Where(x => x.User_Id == id).ToList().
                ForEach(a => a.tblBook.Availability = "yes");

            //removes reservations made by user who is getting deleted
            tblUser tblUser = db.tblUsers.Find(id);
            db.tblReserves.RemoveRange(db.tblReserves.Where(x => x.User_Id == id).ToList());

            //deletes user
            db.tblUsers.Remove(tblUser);
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
