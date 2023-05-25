using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using BusManagementSystem.Entities;

namespace BusManagementSystem
{
    public class BaseRepository
    {
        private readonly string ConnnectionString;

        protected BaseRepository()
        {
            ConnnectionString = ConfigurationManager.ConnectionStrings["BMS"].ConnectionString;

        }
        protected SqlConnection GetConnection() 
        {
            var connection = new SqlConnection(ConnnectionString);
            return connection;  
        }
        public void OpenConnection(SqlConnection conn)
        {
            conn.Open();
        }

        public List<Ticket> ExecuteReader(String query)
        {
            var connection = GetConnection();   
            OpenConnection(connection);

            var commamd = new SqlCommand(query, connection);
            return ReadEntity(commamd.ExecuteReader());
        }
        public List<Ticket> ReadEntity(SqlDataReader rdr)
        {
            var list = new List<Ticket>();

            while (rdr.Read())
            {
                var result = new Ticket
                {
                    TicketID = rdr.IsDBNull(0) ? 0 : Convert.ToInt32(rdr[0]),
                    PassengerName = rdr.IsDBNull(1) ? "Not Found" : rdr[1].ToString(),
                    Price = rdr.IsDBNull(2) ? 2 : Convert.ToInt32(rdr[2]),
                    StartDestination = rdr.IsDBNull(3) ? "Not Found" : rdr[3].ToString(),
                    ArrivalDestination = rdr.IsDBNull(4) ? "Not Found" : rdr[4].ToString(),
                    BusID = rdr.IsDBNull(5) ? 5 : Convert.ToInt32(rdr[5])
                };

                list.Add(result);
            }
            return list;
        }


    }
    
}
