using BMS.Constants;
using BMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BMS
{
    public class CRUD_Operation : BaseRepository
    {
        public static readonly CRUD_Operation Instance = new CRUD_Operation();

        public List<Ticket> GetAllTicket()
        {
            Console.WriteLine("Database Connection has been Opened\n");
            return ExecuteReader(QueryConstants.SelectAllTicket,0,"","all");

        }
        public List<Ticket> GetClintTicket(int UserId)
        {
            Console.WriteLine("Database Connection has been Opened\n");


            return ExecuteReader(QueryConstants.SelectAllClintTicket, UserId,"", "UserId");

        }
        public List<Ticket> GetallAvailableTicket(string Available)
        {
            Console.WriteLine("Database Connection has been Opened\n");


            return ExecuteReader(QueryConstants.SelectAllAvailableTicket, 0, Available, "Available");

        }
        public Ticket ReadOneicket(int ticket) 
        {
            Console.WriteLine("Database Connection has been Opened\n");
            return ExecuteRedeOneTicket(QueryConstants.SelectwhereTicket, ticket);


        }
        public void BookNewTicket(int TiketId, int UserId)
        {
            Console.WriteLine("Database Connection has been Opened\n");    

               
            ExecuteBookTicket(QueryConstants.BookTicket, TiketId, UserId);
        }
        public void CancelTicket(int TicketId)
        {
            Console.WriteLine("Database Connection has been Opened\n");
            ExecuteCancelBookTicket(QueryConstants.CancelBookedTicket, TicketId);
        }
        public List<Bus> GetAllBuses()
        {
            Console.WriteLine("Database Connection has been Opened\n");
            return ExcuteReadBus(QueryConstants.SelectAllBuses);

        }
        public void DeleteTicket(int TiketId)
        {
            Console.WriteLine("Database Connection has been Opened\n");
            ExcuteDelete(QueryConstants.DeleteTicket, TiketId);
        }
        public void InsertTicket(Ticket t)
        {
            Console.WriteLine("Database Connection has been Opened\n");
             ExecuteInsertTicket(QueryConstants.CreateTicket, t);
            return;

        }
        public void updateTicket(Ticket t)
        {
            Console.WriteLine("Database Connection has been Opened\n");
             ExecuteUpdateTicket(QueryConstants.UpdateTicket, t);
            return;

        }
        public void updateBus(Bus t)
        {
            Console.WriteLine("Database Connection has been Opened\n");
             ExecuteUpdateBus(QueryConstants.UpdateBus, t);
            return;

        }

        public string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }


    }
}
