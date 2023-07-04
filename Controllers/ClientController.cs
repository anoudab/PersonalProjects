using System;
using System.Data;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BMS_Project.Entity;
using QRCoder;

namespace BMS_Project.Controllers
{
    //[Authorize]
    public class ClientController : Controller
    {
        private DBContext db = new DBContext();

        public ActionResult Homepage()
        {
            return View();
        }
        public ActionResult AvailableTrips()
        {
            var unavailableTrips = db.Trip.Where(t => t.Date < DateTime.Now || t.AvailableTickets == 0 ).ToList();

            foreach (var item in unavailableTrips)
            {
                item.IsDeleted = 1;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(db.Trip.Where(t => t.IsDeleted == 0).ToList());
        }

        [HttpPost]
        public ActionResult MyTickets()
        {
            int userId = Convert.ToInt32(Session["Id"]);
            var tickets = db.Ticket.Where(t => t.UserId == userId && t.IsDeleted == 0).ToList();
            using (MemoryStream ms = new MemoryStream())
            {

                QRCodeGenerator qrGen = new QRCodeGenerator();
                QRCodeData qrData = qrGen.CreateQrCode(QRTicket, QRCodeGenerator.ECCLevel.Q);
                QRCode qr = new QRCode(qrData);
                using (Bitmap bmap = qr.GetGraphic(20))
                {
                    bmap.Save(ms, ImageFormat.Png);
                    ViewBag.QRCode = "data:image/png;base64" + Convert.ToBase64String(ms.ToArray());
                }

            }
            return View(tickets);
        }

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
        public ActionResult TicketDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        public ActionResult BookTicket(int? id)
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookTicket([Bind(Include = "TripId,TripName")] Trip trip, int NoOfTickets)
        {
            if (ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Invalid Input.";
                return View(trip);
            }

            var ticket = db.Ticket.Where(t => t.TripId == trip.TripId && t.Available == 1).ToList();
            int limit = 1;

            if (ticket.Count() == 0)
            {
                ViewBag.ErrorMessage = "No available tickets.";
                return View(trip);
            }

            foreach (Ticket t in ticket)
            {
                if (limit > NoOfTickets)
                {
                    break;
                }
                int i = t.TicketId;
                t.Available = 0;
                t.UserId = Convert.ToInt32(Session["Id"]);
                db.Entry(t).State = EntityState.Modified;
                db.SaveChanges();
                limit++;
                db.SaveChanges();
            }

            Trip currentTrip = db.Trip.Where(t => t.TripId == trip.TripId).FirstOrDefault();
            currentTrip.AvailableTickets -= NoOfTickets;
            db.Entry(currentTrip).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("MyTickets", new { userId = Session["Id"] });
        }
        public ActionResult CancelTicket(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        [HttpPost, ActionName("CancelTicket")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelTicketConfirmed(int id)
        {
            Ticket ticket = db.Ticket.Find(id);
            ticket.Available = 1;
            ticket.UserId = 0;
            db.Entry(ticket).State = EntityState.Modified;

            Trip currentTrip = db.Trip.Where(t => t.TripId == ticket.TripId).FirstOrDefault();
            currentTrip.AvailableTickets += 1;
            db.Entry(currentTrip).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("MyTickets");
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
            var unique = db.User.Where(u => u.Username == user.Username);

            if (unique != null)
            {
                ViewBag.ErrorMessage = "Username already exists. Please enter a new one.";
                return View(user);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Invalid Input. Please enter correct values.";
                return View(user);

            }
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Homepage");
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
