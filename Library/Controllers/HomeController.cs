using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        LibraryDBContext db = new LibraryDBContext();
        public ActionResult Index(string username, string password)
        {
            //When the program opens it will check and see what reservations are past due date
            //and remove them
            db.tblReserves.Where(x => x.Due_Date < DateTime.Now).ToList().
                ForEach(a => a.tblBook.Category = "yes");
            db.tblReserves.RemoveRange(db.tblReserves.Where(x => x.Due_Date < DateTime.Now).ToList());
            db.SaveChanges();

            //makes username cookie that will be used throughout
            Session["username"] = username;

            //verifies if the one who is loging in is an admin
            var adminlogin = db.tblUsers.Where(x => x.User_Name.Equals(username)
            && x.Password.Equals(password) && x.Admin.Equals("yes")).ToList();

            //verifies if the one who is loging in is a normal user
            var login = db.tblUsers.Where(x => x.User_Name.Equals(username)
            && x.Password.Equals(password) && x.Admin != "yes").ToList();

            //makes user id # cookie that will be used throughout
            var userId = db.tblUsers.Where(u => u.User_Name == username)
                .Select(u => u.Id).FirstOrDefault();
            Session["UserID"] = userId.ToString();

            //moves admin to admin page or user to userpage if username and password is correct
            if (adminlogin.Count() == 1)
            {
                return RedirectToAction("AdminPage", "tblUsers");
            }
            else if (login.Count() == 1)
            {
                return RedirectToAction("Index", "tblReserves");
            }
            else
            {
                ModelState.AddModelError("", "Incorrect Username/Password!");               
            }
            return View();
        }

    }
}