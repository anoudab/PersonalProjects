using BMS;
using BMS.Entities;
using System;
using System.Collections.Generic;

namespace BMS
{
    public class Program
    {
        public static readonly CRUD_Operation operation = CRUD_Operation.Instance;

        static void Main(string[] args)
        {

            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Welcome to the Bus Management System");
            Console.WriteLine("-----------------------------------");
            
            Console.WriteLine("Please choose to log in as an Admin or a Client:");
            User user = new User();
            user.Role= Console.ReadLine().ToLower();

            List<Ticket> selectedTicket;
            int TicketId, userId, input =9;
            while (input != 0) 
            {
               
                Console.WriteLine("Please choose an option from the menu:");
                if (user.Role == "admin")
                {
                    Console.WriteLine("\nEnter 1 to view all the tickets."
                    + "\nEnter 2 to view all Buses."
                    + "\nEnter 3 to Modify the ticket information."
                    + "\nEnter 4 to Modify the bus information."
                    + "\nEnter 5 to Create new ticket."
                    + "\nEnter 6 to Delete ticket."
                    + "\nEnter 7 to Change your log in status."
                    + "\nEnter 0 to exit the system:");
                 
                    input = Convert.ToInt32(Console.ReadLine());

                    switch (input)
                    {
                        case 1:
                            var tickets = operation.GetAllTicket();
                            foreach (var ticket in tickets)
                            {
                                Console.WriteLine("Ticket ID: " + ticket.TicketId + " \nAvailablity: " + ticket.Available + "\nStart Destination:  " + ticket.StartDestination + "\nArrival Destination: "
                                    + ticket.ArrivalDestination + "\nBus ID: " + ticket.BusId + "\n\n");
                            }
                            break;

                        case 2:
                            List<Bus> buses = operation.GetAllBuses();
                            foreach (var bus in buses)
                            {
                                Console.WriteLine("Bus Id:" + bus.BusId + "\nnumber of seat: " + bus.NoOfSeats + "\nDiver Name:  " + bus.DriverName +
                                    "\nRegion: " + bus.Region + "\nBus Palte: " + bus.Plate + "\n\n");
                            }

                            break;
                        case 3:
                            var updateTicket = new Ticket();

                            Console.WriteLine("Enter the ID of the ticket you would like to modify: ");
                            updateTicket.TicketId = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine("Enter the Price: ");
                            updateTicket.Price = Convert.ToDouble(Console.ReadLine());
                            Console.WriteLine("Enter the start destination: ");
                            updateTicket.StartDestination = Console.ReadLine();
                            Console.WriteLine("Enter the arrival destination: ");
                            updateTicket.ArrivalDestination = Console.ReadLine();
                            Console.WriteLine("Enter the Bus ID: ");
                            updateTicket.BusId = Convert.ToInt32(Console.ReadLine());
                            //Console.WriteLine("Enter 1 if Ticket has UserID or enter 0 to skip");
                            //var Id = Convert.ToInt32(Console.ReadLine());
                            //if (Id == 1)
                            //{
                                Console.WriteLine("Enter the User ID: ");
                                updateTicket.UserId = Convert.ToInt32(Console.ReadLine());
                                updateTicket.Available = "false";
                            //}
                            //else
                            //{
                            //    updateTicket.UserId = 0; 
                            //    updateTicket.Available = "true";
                            //}
                            operation.updateTicket(updateTicket);
                            Console.WriteLine("Successfully modified Ticket information");

                            break;

                        case 4:
                            Bus updateBus = new Bus();

                            Console.WriteLine("Enter the ID of the Bus you would like to modify: ");
                            updateBus.BusId = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine("Enter the Number of Seats ");
                            updateBus.NoOfSeats = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter the Region: ");
                            updateBus.Region = Console.ReadLine();
                            Console.WriteLine("Enter the driver's name: ");
                            updateBus.DriverName = Console.ReadLine();
                            Console.WriteLine("Enter the Plate: ");
                            updateBus.Plate = Console.ReadLine();
                            operation.updateBus(updateBus);
                           
                            Console.WriteLine("Successfully modified Bus information");

                            break;


                        case 5:

                            Ticket newTicket = new Ticket();

                            Console.WriteLine("Enter the Price: ");
                            newTicket.Price = Convert.ToDouble(Console.ReadLine());
                            Console.WriteLine("Enter the start destination: ");
                            newTicket.StartDestination = Console.ReadLine();
                            Console.WriteLine("Enter the arrival destination: ");
                            newTicket.ArrivalDestination = Console.ReadLine();
                            Console.WriteLine("Enter the BusID: ");
                            newTicket.BusId = Convert.ToInt32(Console.ReadLine());
                            newTicket.Available = "true";
                            operation.InsertTicket(newTicket);
                            Console.WriteLine("Successfully added ticket");
                            break;
                        case 6: 
                            Console.WriteLine("Please enter the ticket ID you wish to delete:");
                            var id = Convert.ToInt32(Console.ReadLine());
                            operation.DeleteTicket(id);
                            Console.WriteLine("Ticket has been deleted successfuly\n");
                            break;
                        case 7:
                            Console.WriteLine("Please choose to log in as an Admin or Client:");
                            user.Role = Console.ReadLine().ToLower();
                            break;
                            
                        case 0:
                            Console.WriteLine("System successfully terminated.\n");

                            break;

                        default:
                            Console.WriteLine("Enter a valid number\n");
                            break;
                    }

                }
                else if (user.Role == "client")
                {

                    Console.WriteLine("\nEnter 1 to view all availeable the tickets." +
                    "\nEnter 2 to view your own tickits." +
                    "\nEnter 3 to book available ticket." +
                    "\nEnter 4 to cancel booked ticket." + 
                    "\nEnter 7 to change Your log." +
                     "\nEnter 0 to exit the system:"); 
                    input = Convert.ToInt32(Console.ReadLine());

                    switch (input)
                    {
                        case 1:
                             selectedTicket = operation.GetallAvailableTicket("true");
                            //if (selectedTicket[0] == null)
                            //{
                            //    Console.WriteLine("No Available Ticke"); 
                            //}else
                            foreach (var ticket in selectedTicket)
                            {
                                Console.WriteLine("Ticket ID: " + ticket.TicketId + " \nAvailablity: " + ticket.Available + "\nStart Destination:  " + ticket.StartDestination + "\nArrival Destination: "
                                    + ticket.ArrivalDestination + "\nBus ID: " + ticket.BusId+"\n\n");
                            }
                            break;

                        case 2:
                            Console.WriteLine("Enter your ID: ");
                            userId = Convert.ToInt32(Console.ReadLine());

                             selectedTicket = operation.GetClintTicket(userId);
                            //if (selectedTicket[0]==null)
                            //{
                            //    Console.WriteLine("No Available Ticke");
                            //}else
                            foreach (var ticket in selectedTicket)
                            {
                                Console.WriteLine("Ticket ID: " + ticket.TicketId + "\nAvailablity:  " + ticket.Available + "\nStart Destination: " + ticket.StartDestination + "\nArrival Destination: "
                             + ticket.ArrivalDestination + "\nBus ID: " + ticket.BusId + "\n\n");
                            }
                            break;
                        case 3:
                            Console.WriteLine("Please enter your ID:");
                            userId = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Please enter Ticket ID:");
                            TicketId = Convert.ToInt32(Console.ReadLine());

                            Ticket tick = operation.ReadOneicket(TicketId);

                            if (operation.RemoveWhitespace(tick.Available).ToLower() == "true")
                            {
                               
                                    operation.BookNewTicket(TicketId, userId);
                                    Console.WriteLine("Ticket has been booked successfuly\n");


                            }
                            else if (tick.UserId == userId && operation.RemoveWhitespace(tick.Available).ToLower() == "false")
                            {
                                Console.WriteLine("This ticket is already booked by you");

                            }

                            else
                                Console.WriteLine("The ticket is not available ");

                            break;

                        case 4:
                            Console.WriteLine("Please enter Ticket ID:");
                            TicketId = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Please enter your ID:");
                            userId = Convert.ToInt32(Console.ReadLine());
                            selectedTicket = operation.GetClintTicket(userId);

                            if (selectedTicket[0].UserId== userId)
                            {
                                operation.CancelTicket(TicketId);
                                Console.WriteLine("Ticket booking has been cancelled\n");

                            }
                            else
                                Console.WriteLine("This ticket does not belong to you!!");
                           
                            break;
                        case 7:
                            Console.WriteLine("Please choose to log in as an Admin or a Client:");

                            user.Role = Console.ReadLine().ToLower();
                            break;
                        case 0:
                            Console.WriteLine("System successfully terminated.\n");

                            break;

                        default:
                            Console.WriteLine("Enter a valid number\n");
                            break;
                    }

                }
                else
                {
                    Console.WriteLine("Your chose is not correct \nPlease enter agian");
                    user.Role = Console.ReadLine().ToLower();

                }




            } 
            Console.ReadLine();
        }
    }
}
