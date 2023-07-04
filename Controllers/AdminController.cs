using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BMS_Project.Entity;

namespace BMS_Project.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        private DBContext db = new DBContext();

        //---------------------------------Trips------------------------------------------

        //Admin Homepage - the first page the admin will see after loggin in.
        public ActionResult Homepage()
        {
            return View();
        }

        //Trip details - Will get the ID and show the details of the trip
        public ActionResult Trips()
        {
            return View(db.Trip.Where(t => t.IsDeleted == 0).ToList());
        }

        //Trip details - Will show the details of each trip
        public ActionResult TripDetails(int? id)
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

            string currentBusDriver = db.Bus.Where(b => b.Plate == trip.BusPlate && b.IsDeleted == 0).Select(b => b.DriverName).FirstOrDefault();
            double currentPrice = db.Ticket.Where(t => t.TripId == id).Select(t => t.Price).FirstOrDefault();

            ViewBag.CurrentBusDriver = currentBusDriver;
            ViewBag.CurrentPrice = currentPrice;
            return View(trip);
        }

        //Creating a new trip - will show the form for the user to fill.
        public ActionResult CreateTrip()
        {
            var plates = db.Bus.Where(b => b.Booked == 0 && b.IsDeleted == 0).Select(b => b.Plate).ToList();

            if (plates == null)
            {
                return HttpNotFound();
            }
            ViewBag.BusPlates = plates;

            List<String> busDriver = db.User.Where(u => u.Role == "BusDriver").Select(u => u.FirstName).ToList();
            List<String> availableDrivers = new List<String>();
           
             foreach (string name in busDriver)
            {
                if (db.Bus.Where(b => b.Booked == 1 && b.IsDeleted == 0 && b.DriverName == name).FirstOrDefault() != null)
                {
                    continue;
                }

                availableDrivers.Add(name);
            }


            if (availableDrivers == null)
            {
                return HttpNotFound();
            }
            ViewBag.BusDrivers = availableDrivers;
            return View();
        }

        //Creating a new trip - will validate the information and return the values in order to add to database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTrip(Trip trip, String BusDriver, double Price)
        {
            if (ModelState.IsValid)
            {
                int Bid = db.Bus.Where(b => b.Plate == trip.BusPlate).Select(b => b.BusId).FirstOrDefault();
                Bus bus = db.Bus.Find(Bid);


                if (trip.NoOfTickets > bus.NoOfSeats)
                {
                    ViewBag.Message = "Invalid Number of Tickets; Please enter a valid number.";
                    return RedirectToAction("CreateTrip");
                }

                if (trip.Date < DateTime.Now)
                {
                    ViewBag.Message = "Invalid Date; Please choose a valid date.";
                    return RedirectToAction("CreateTrip");
                }

                bus.Booked = 1;
                bus.DriverName = BusDriver;
                db.Entry(bus).State = EntityState.Modified;

                List<Ticket> ticketList = new List<Ticket>(new Ticket[trip.NoOfTickets]);

                int i = 0;
                foreach (var t in ticketList)
                {
                    Ticket ticket = new Ticket(); //new Ticket[trip.NoOfTickets]
                    ticket.Price = Price;
                    ticket.Available = 1;
                    ticket.TripId = trip.TripId;
                    ticket.SerialNumber = ((trip.ArrivalDestination).Substring(0, 3)) + (i);
                    i++;
                    db.Ticket.Add(ticket);

                }

                trip.AvailableTickets = trip.NoOfTickets;
                db.Trip.Add(trip);
                db.SaveChanges();

                return RedirectToAction("Trips");
            }

            return View(trip);
        }

        [HttpGet]

        public ActionResult GetAvailableOptions(DateTime date, DateTime startTime, DateTime arrivalTime, int? tripId)
        {
            var assignedBusPlates = new List<string>();

            foreach (var trip in db.Trip.Where(t => t.TripId != tripId && ((t.Date == date && startTime >= t.StartTime && startTime <= t.ArrivalTime) || (arrivalTime >= t.StartTime && arrivalTime <= t.ArrivalTime))))

            {
                assignedBusPlates.Add(trip.BusPlate);
            }

            var assignedBuses = db.Bus
                .Where(b => assignedBusPlates.Contains(b.Plate))
                .ToList();

            var assignedBusDrivers = assignedBuses
                .Select(b => b.DriverName)
                .ToList();

            var availableBuses = db.Bus
                .Where(b => b.IsDeleted == 0 && !assignedBusPlates.Contains(b.Plate)) //b.Booked == 0 
                .Select(b => b.Plate)
                .ToList();

            var availableDrivers = db.User
                .Where(u => u.Role == "BusDriver" && !assignedBusDrivers.Contains(u.FirstName))
                .Select(u => u.FirstName)
                .ToList();

            return Json(new { Buses = availableBuses, BusDrivers = availableDrivers }, JsonRequestBehavior.AllowGet);
        }

        //Edit a specific trip - will get the ID of the trip and show a form with the initial information filled in for the admin to edit.
        public ActionResult EditTrip(int? id)
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

            var plates = db.Bus.Where(b => b.Booked == 0).Select(b => b.Plate).ToList();

            if (plates == null)
            {
                return HttpNotFound();
            }
            ViewBag.BusPlates = plates;

            var busDriver = db.User.Where(u => u.Role == "BusDriver").ToList();
            var bookedDriver = db.Bus.Where(b => b.Booked == 1).Select(b => b.DriverName).ToList();

            List<string> availableDrivers = new List<String>(); 
            foreach (var b in busDriver)
            {
                if (!b.FirstName.Equals(bookedDriver))
                {
                    availableDrivers.Add(b.FirstName);
                }
            }
            if (availableDrivers == null)
            {
                return HttpNotFound();
            }

            string currentBusDriver = db.Bus.Where(b => b.Plate == trip.BusPlate && b.IsDeleted == 0).Select(b => b.DriverName).FirstOrDefault();
            double currentPrice = db.Ticket.Where(t => t.TripId == id).Select(t => t.Price).FirstOrDefault();

            ViewBag.CurrentBusDriver = currentBusDriver;
            ViewBag.CurrentPrice = currentPrice;
            ViewBag.BusDrivers = availableDrivers;
            return View(trip);
        }

        //Editing a specific trip - will validate the information and return the values in order to add to database.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTrip(Trip trip, String BusDriver, double Price)
        {
            if (ModelState.IsValid)
            {
                db.Trip.Add(trip);
                int Bid = db.Bus.Where(b => b.Plate == trip.BusPlate).Select(b => b.BusId).FirstOrDefault();
                Bus bus = db.Bus.Find(Bid);


                if (trip.NoOfTickets > bus.NoOfSeats)
                {
                    ViewBag.InvalidSeatNumber = "Invalid Number of Tickets; Please enter a valid number.";
                    return RedirectToAction("CreateTrip");
                }

                if (trip.Date < DateTime.Now)
                {
                    ViewBag.InvalidDate = "Invalid Date; Please choose a valid date.";
                    return RedirectToAction("CreateTrip");
                }

                bus.Booked = 1;
                bus.DriverName = BusDriver;
                db.Entry(bus).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Trips");
            }

            return View(trip);
        }

        //Delete a certain trip - will get the ID of a trip and then ask the admin to confirm the delete action.
        public ActionResult DeleteTrip(int? id)
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

        //Delete a specific trip - will get confirmation to delete the trip and update the status of the trip in the database.
        [HttpPost, ActionName("DeleteTrip")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTripConfirmed(int id)
        {
            Trip trip = db.Trip.Find(id);
            trip.IsDeleted = 1;
            db.Entry(trip).State = EntityState.Modified;
            db.SaveChanges();


            Bus bus = db.Bus.Where(b => b.Plate == trip.BusPlate).FirstOrDefault();
            bus.Booked = 0;
            bus.DriverName = null;
            db.Entry(bus).State = EntityState.Modified;
            db.SaveChanges();

            List<Ticket> tickets = db.Ticket.Where(t => t.TripId == trip.TripId).ToList();
            foreach (Ticket t in tickets)
            {
                db.Ticket.Remove(t);
            }
            db.SaveChanges();

            return RedirectToAction("Trips");
        }

        //---------------------------------Buses------------------------------------------

        //Bus List - Will show the list of all buses.
        public ActionResult Buses()
        {
            return View(db.Bus.Where(t => t.IsDeleted == 0).ToList());
        }

        //Bus details - Will get the ID and show the details of the bus.
        public ActionResult BusDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bus bus = db.Bus.Find(id);
            if (bus == null)
            {
                return HttpNotFound();
            }
            return View(bus);
        }

        //Creating a new bus - will show the form for the user to fill.
        public ActionResult CreateBus()
        {
            return View();
        }

        //Creating a new bus - will validate the information and return the values in order to add to database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBus([Bind(Include = "BusId,Plate,NoOfSeats,Region")] Bus bus)
        {
            List<Bus> buses = db.Bus.Where(t => t.IsDeleted == 0).ToList();
            
            
            if (ModelState.IsValid)
            {
                db.Bus.Add(bus);
                db.SaveChanges();
                return RedirectToAction("Buses");
            }

            return View(bus);
        }

        //Edit a specific bus - will get the ID of the trip and show a form with the initial information filled in for the admin to edit.
        
        public ActionResult EditBus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bus bus = db.Bus.Find(id);
            if (bus == null)
            {
                return HttpNotFound();
            }
            return View(bus);
        }

        //Editing a specific trip - will validate the information and return the values in order to add to database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBus([Bind(Include = "BusId,Plate,NoOfSeats,Region")] Bus bus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Buses");
            }
            return View(bus);
        }

        //Delete a certain bus - will get the ID of a bus and then ask the admin to confirm the delete action.
        public ActionResult DeleteBus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bus bus = db.Bus.Find(id);
            if (bus == null)
            {
                return HttpNotFound();
            }
            return View(bus);
        }

        //Delete a specific bus - will get confirmation to delete the bus and update the status of the bus in the database.
        [HttpPost, ActionName("DeleteBus")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBusConfirmed(int id)
        {
            Bus bus = db.Bus.Find(id);
            bus.IsDeleted = 1;
            db.Entry(bus).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Buses");
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