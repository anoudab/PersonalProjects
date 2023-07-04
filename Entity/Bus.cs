using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BMS_Project.Entity
{
    //Bus Entity - For the code-first entity framework approach.
    //Each Bus will have one trip at a time.
    public class Bus
    {
        //Primary Key - will autoincrement
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BusId { get; set; }

        //Plate number for the bus will be unique but will not be the primary key.
        [StringLength(8)]
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "This field is requiered")]
        public string Plate { get; set; }
        public int Booked { get; set; }
        public int NoOfSeats { get; set; }
        public string Region { get; set; }
        public string DriverName { get; set; }
        public int IsDeleted { get; set; }

    }
}