using System;


namespace BusManagementSystem
{
    internal class Program
    {
        public static readonly CRUD_Operation operation = CRUD_Operation.Instance;

        static void Main(string[] args)
        {

            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Welcome to the Bus Management System");
            Console.WriteLine("-----------------------------------");
            bool flag = false;

            do
            {

                Console.WriteLine("Please choose an option from the menu:");

                Console.WriteLine("Enter 1 to print the infromation for all the tickets.");
                Console.WriteLine("Enter 2 to print the ticket information for a certain ID: ");
                Console.WriteLine("Enter 3 to exit the system: ");
                var input = Convert.ToInt32(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        var tickets = operation.GetAllList();
                        foreach (var ticket in tickets)
                        {
                            Console.WriteLine("Ticket ID: " + ticket.TicketID + " \nPasseneger Name: " + ticket.PassengerName + "\nStart Destination:  " + ticket.StartDestination + "\nArrival Destination: "
                                + ticket.ArrivalDestination + "\nBus ID: " + ticket.BusID);
                        }
                        break;

                    case 2:
                        Console.WriteLine("Enter the ID of the row: ");
                        var rowID = Console.ReadLine();

                        var ticketWithID = operation.GetIDFromList(rowID);

                        foreach (var ticket in ticketWithID)
                        {
                            Console.WriteLine("Ticket ID: " + ticket.TicketID + "\nPassenger name:  " + ticket.PassengerName + "\nStart Destination: " + ticket.StartDestination + "\nArrival Destination: "
                         + ticket.ArrivalDestination + "\nBus ID: " + ticket.BusID);
                        }
                        break;

                    case 3:
                        Console.WriteLine("System successfully terminated.");
                        flag = true;
                        break;

                    default:
                        Console.WriteLine("Enter a valid number");
                        break;
                }


            } while (!flag);
        }
    }
}
