namespace BMS.Entities
{ 

    public class Bus
    {
        public int BusId { get; set; }
        public int NoOfSeats { get; set; }
        public string Region { get; set; }
        public string DriverName { get; set; }
        public string Palte { get; set; }
        public object Plate { get; internal set; }
    }

}
