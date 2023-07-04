using BMS_Project.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BMS_Project.Controllers
{
    public class HomeController : Controller
    {

        private DBContext db = new DBContext();
        List<string> Roles = new List<string>() { "Admin", "Client", "BusDriver" };

        public ActionResult Homepage()
        {
            List<Trip> availableTrips = db.Trip.Where(t => t.IsDeleted == 0).ToList();
            return View(availableTrips);
        }

        public ActionResult Login()
        {
            ViewBag.Roles = new SelectList(Roles);
            return View();

        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            var trylog = db.User.Where(u => u.Username == user.Username && u.Password == user.Password && u.Role.ToLower() == user.Role.ToLower()).FirstOrDefault();

            if (trylog == null)
            {
                TempData["ErrorMessage"] = "User does not exist.";
                ViewBag.Roles = new SelectList(Roles);
                return View(user);
            }


            Session["Id"] = trylog.UserId;
            Session["Name"] = trylog.FirstName.ToString() + " " + trylog.LastName.ToString();
            Session["Role"] = trylog.Role.ToString();

            if (user.Role.Contains("Admin"))
            {
                return RedirectToAction("Homepage", "Admin");
            }
            else if (user.Role.Contains("Client")) { 
                return RedirectToAction("Homepage", "Client");
            }
            else if (user.Role.Contains("BusDriver"))
            { return RedirectToAction("Homepage", "BusDriver"); }

            return View();
        }

        public ActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            //if (!ModelState.IsValid)
            //{
            //    ViewBag.Message = "Invalid Input: Please check the entered input.";
            //    var errors = ModelState.Values.SelectMany(v => v.Errors);
            //    foreach (var error in errors)
            //    {
            //        var e = error.ToString();
            //    }

            //    return View();
            //}
            try
            {
                var trylog = db.User.Find(user.UserId);
                while (trylog != null)
                {
                    ViewBag.Message = "Registration failed: This user already exists.";
                    return View();
                }
                user.Role = "Client";
                db.User.Add(user);
                db.SaveChanges();

                ViewBag.Message = "Registration Successful";
                return RedirectToAction("Homepage");

        }
            catch (Exception)
            {
                ViewBag.Message = "There was an error in the registration process.";
                return View();
    }
}

        public ActionResult Logout()
        {
            if (Session["Id"] != null)
                Session["Id"] = null;
            Session["Name"] = null;
            Session["Role"] = null;
            return RedirectToAction("Homepage", "Home");
        }

    }
}