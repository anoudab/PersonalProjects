using BMS_Project.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BMS_Project.Controllers
{
    //[Authorize]
    public class BusDriverController : Controller
    {
        private DBContext db = new DBContext();
        public ActionResult Homepage()
        {
            return View();
        }
        public ActionResult JobList()
        { 
            int space = (Session["Name"].ToString()).IndexOf(' ');
            string firstName = (Session["Name"].ToString()).Substring(0,space);

            List<Bus> driverJobs = db.Bus.Where(b => b.DriverName == firstName && b.IsDeleted == 0).ToList();
            List<Trip> trip = new List<Trip>();
            
            foreach(Bus bus in driverJobs)
            {
                trip.Add(db.Trip.Where(b => b.BusPlate == bus.Plate).FirstOrDefault());
            }

            return View(trip);
        }
        public ActionResult Account()
        {
            if (Session["Id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int uId = Convert.ToInt32(Session["Id"]);
            User user = db.User.Where(u => u.UserId == uId).FirstOrDefault();

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        public ActionResult EditAccount(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccount(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Homepage");
            }
            return View(user);
        }

        public ActionResult JobDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trip trip = db.Trip.Find(id);
            if (trip == null)
            {
                return HttpNotFound();
            }
            return View(trip);
        }

    }
}