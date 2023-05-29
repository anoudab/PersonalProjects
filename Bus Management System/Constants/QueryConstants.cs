namespace BMS.Constants
{
    public class QueryConstants
    {
        //Select
        //Admin
        public const string SelectAllTicket = "SELECT * FROM Ticket";//alanood
        public const string SelectAllBuses = "SELECT * FROM Bus";//alanood+Rafeef 
        //Client
        public const string SelectAllAvailableTicket = "SELECT * FROM Ticket where Available =@Available ";//Rafeef
        public const string SelectAllClintTicket= "SELECT * FROM Ticket where UserId =@UserId ";//Rafeef
        public const string SelectwhereTicket = "SELECT * FROM Ticket where TicketId =@TicketId ";//Rafeef

        //Insert 
        public const string CreateTicket = "INSERT INTO Ticket (Available,Price,StartDestination,ArrivalDestination,BusId)" +
            "VALUES (@Available,@Price,@StartDestination,@ArrivalDestination,@BusId)";//alanood+Rafeef

        //Update
        //Admin
        public const string UpdateTicket = "UPDATE Ticket SET Available = @Available, Price = @Price, StartDestination " +
            "= @StartDestination, ArrivalDestination = @ArrivalDestination, BusId = @BusId ,UserId = @UserId WHERE TicketID = @TicketId";//alanood+Rafeef

        public const string UpdateBus = "UPDATE Bus SET NoOfSeats = @NoOfSeats, Region = @Region, DriverName = @DriverName, Plate = @Plate  WHERE BusId = @BusId";// alanood+Rafeef
        //Client
        public const string BookTicket = "UPDATE Ticket SET Available = 'false' ,UserId = @UserId WHERE TicketId =@TicketId";//Rafeef
        public const string CancelBookedTicket = "UPDATE Ticket SET Available = 'true' ,UserId = NULL WHERE TicketId =@TicketId ";//Rafeef

        //Delete
        public const string DeleteTicket = "DELETE FROM Ticket WHERE TicketId=@TicketId";//Rafeef
    }
}
