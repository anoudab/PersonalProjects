using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BMS_Project.Entity
{
    //DB Context class for our database.
    public class DBContext : DbContext
    {
        //Database will be named BMS_New
        public DBContext() : base("BMS")
        { }
        public DbSet<Bus> Bus { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<Trip> Trip { get; set; }
        public DbSet<User> User { get; set; }
    }
}