using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BMS_Project.Entity
{
    //Ticket Entity - For the code-first entity framework approach.
    //A client will be able to book a ticket from a trip.
    public class Ticket
    {
        //Primary Key - will autoincrement
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketId { get; set; }

        [StringLength(10)]
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "This field is requiered")]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "This field is requiered")]
        public int Available { get; set; }

        [Required(ErrorMessage = "This field is requiered")]
        public double Price { get; set; }
        public int UserId { get; set; }
        public int IsDeleted { get; set; }

        [Required(ErrorMessage = "This field is requiered")]
        public int TripId { get; set; }

    }
}