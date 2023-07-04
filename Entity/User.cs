using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace BMS_Project.Entity
{
    //User Entity - For the code-first entity framework approach.
    //The user of the system will wither be the client or the admin.

    public class User
    {
        //Primary Key - will autoincrement
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } 

        [Required(ErrorMessage = "this field is requiered")]
        public string Role { get; set; }

        [StringLength(20)]
        [Index(IsUnique = true)]
        [Display(Name = "Username")]
        [Required(ErrorMessage = "This field is requiered")]
        public string Username { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This field is requiered")]
        public string Password { get; set; }

        //Virtual value to check the password.
        [NotMapped]
        [Required(ErrorMessage = "This field is requiered")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "the Passwoord and rePassword must match")]
        public string RePassword { get; set; }
        public int IsDeleted { get; set; }

    }
}