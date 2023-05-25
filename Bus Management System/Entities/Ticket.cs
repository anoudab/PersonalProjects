namespace BusManagementSystem.Entities
{
    public class Ticket
    {
            public int TicketID
            { get; set; }

            public string PassengerName
            { get; set; }

            public double Price
            { get; set; }

            public string StartDestination
            { get; set; }

            public string ArrivalDestination
            { get; set; }

            public int BusID
            { get; set; }
  

    }
}
