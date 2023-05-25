using BusManagementSystem.Constants;
using BusManagementSystem.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace BusManagementSystem
{
    public class CRUD_Operation : BaseRepository
    {
        public static readonly CRUD_Operation Instance = new CRUD_Operation();

        public List<Ticket> GetAllList()
        {
            Console.WriteLine("Database Connection has been Opened");
            return ExecuteReader(QueryConstants.SelectAll);

        }

        public List<Ticket> GetIDFromList(string rowID)
        {
            Console.WriteLine("Database Connection has been Opened");
            var stringBuilder = new StringBuilder(QueryConstants.SelectAllWhere);
            stringBuilder.Append(rowID);
            var query = stringBuilder.ToString();

            return ExecuteReader(query);

        }
    }
}
