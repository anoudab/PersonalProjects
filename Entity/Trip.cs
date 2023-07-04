using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BMS_Project.Entity
{
    //Trip Entity - For the code-first entity framework approach.
    //An admin can create a new trip. A client can book a ticket from a trip.
    public class Trip
    {
        //Primary Key - will autoincrement
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TripId { get; set; }

        [Required(ErrorMessage = "This field is requiered")]
        public string TripName { get; set; }

        [Required(ErrorMessage = "This field is requiered")]
        public int NoOfTickets { get; set; }//check where

        public int AvailableTickets { get; set; }

        [Required(ErrorMessage = "This field is requiered")]
        public string StartDestination { get; set; }

        [Required(ErrorMessage = "This field is requiered")]
        public string ArrivalDestination { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string BusPlate { get; set; }
        public List<Ticket> Tickets { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set;}

        [DataType(DataType.Time)]
        public DateTime StartTime { get; set;}

        [DataType(DataType.Time)]
        public DateTime ArrivalTime{ get; set; }

        public int IsDeleted { get; set; }

    }
}