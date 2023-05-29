namespace BMS.Entities
{
    public class Ticket
    {
            public int TicketId { get; set; }
            public string Available { get; set; }
            public double Price { get; set; }
            public string StartDestination { get; set; }
            public string ArrivalDestination { get; set; }
            public int BusId { get; set; }
            public int UserId { get; set; }

    }
}
