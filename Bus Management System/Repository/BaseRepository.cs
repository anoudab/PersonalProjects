using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using BMS.Entities;

namespace BMS
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
        #region Select Bus
        public List<Bus> ExcuteReadBus(string Query)
        {
            try
            {
                var connction = GetConnection();
                OpenConnection(connction);

                var command = new SqlCommand(Query, connction);
                return ReadBus(command.ExecuteReader());
            }
            catch (Exception ex)
            {
                 Console.WriteLine(ex.Message);
            }
            return null;
        }
        public List<Bus> ReadBus(SqlDataReader rdr)
        {
            var resultList = new List<Bus>();
            while (rdr.Read())
            {
                var bus = new Bus
                {
                    BusId = rdr.IsDBNull(1) ? 0 : Convert.ToInt32(rdr[0]),
                    NoOfSeats = rdr.IsDBNull(1) ? 0 : Convert.ToInt32(rdr[0]),
                    Region = rdr.IsDBNull(2) ? "" : rdr[2].ToString(),
                    DriverName = rdr.IsDBNull(3) ? "" : rdr[3].ToString(),
                    Palte = rdr.IsDBNull(4) ? "" : rdr[4].ToString(),
                };
                resultList.Add(bus);

            }
            return resultList;
        }
        #endregion

        #region Select All types
        public List<Ticket> ExecuteReader(String query, int UserId ,string Available, string type)
        {
            SqlDataReader result;
            try
            {
                var connection = GetConnection();
                OpenConnection(connection);

                var command = new SqlCommand(query, connection);
                if (type == "Available")
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@Available";
                    param.Value = Available;
                    command.Parameters.Add(param);
                }
                else if (type == "UserId")
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@UserId";
                    param.Value = UserId;
                    command.Parameters.Add(param);
                }

                 result = command.ExecuteReader();
                return ReadTicket(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ReadTicket(null);
        }

        public List<Ticket> ReadTicket(SqlDataReader rdr)
        {
            var list = new List<Ticket>();

            try
            {
                while (rdr.Read())
                {
                    var result = new Ticket
                    {
                        TicketId = rdr.IsDBNull(0) ? 0 : Convert.ToInt32(rdr[0]),
                        Available = rdr.IsDBNull(1) ? "Not Found" : rdr[1].ToString(),
                        Price = rdr.IsDBNull(2) ? 2 : Convert.ToInt32(rdr[2]),
                        StartDestination = rdr.IsDBNull(3) ? "Not Found" : rdr[3].ToString(),
                        ArrivalDestination = rdr.IsDBNull(4) ? "Not Found" : rdr[4].ToString(),
                        BusId = rdr.IsDBNull(5) ? 5 : Convert.ToInt32(rdr[5]),
                        UserId = rdr.IsDBNull(6) ? 5 : Convert.ToInt32(rdr[6])
                    };

                    list.Add(result);
                }
               
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return list;
        }

        public Ticket ExecuteRedeOneTicket(string query, int ticketId)
        {
            
            try
            {
                var connection = GetConnection();
                OpenConnection(connection);

                var command = new SqlCommand(query, connection);

                Ticket result =null;
                command.Parameters.AddWithValue("@TicketId", ticketId);

               var rdr = command.ExecuteReader();
                try
                {
                    if (rdr.Read())
                    {
                         result = new Ticket
                        {
                            TicketId = rdr.IsDBNull(0) ? 0 : Convert.ToInt32(rdr[0]),
                            Available = rdr.IsDBNull(1) ? "Not Found" : rdr[1].ToString(),
                            Price = rdr.IsDBNull(2) ? 2 : Convert.ToInt32(rdr[2]),
                            StartDestination = rdr.IsDBNull(3) ? "Not Found" : rdr[3].ToString(),
                            ArrivalDestination = rdr.IsDBNull(4) ? "Not Found" : rdr[4].ToString(),
                            BusId = rdr.IsDBNull(5) ? 0 : Convert.ToInt32(rdr[5]),
                            UserId = rdr.IsDBNull(6) ? 0 : Convert.ToInt32(rdr[6])
                        };


                    }
                    return result;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return new Ticket();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        //public Ticket Read(SqlDataReader rdr)
        //{
        //    Ticket result =null;

            
        //    return result;
        //}
        #endregion

        #region Delete Tickit
        public void ExcuteDelete(string Query, int TicketId)
        {
            try
            {
                var connction = GetConnection();
                OpenConnection(connction);

                // 1. declare command object with parameter
                var command = new SqlCommand(Query, connction);
                // 2. define parameters used in command object
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@TicketId";
                param.Value = TicketId;
                // 3. add new parameter to command object
                command.Parameters.Add(param);
                command.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

        #region Book ticket
        public void ExecuteBookTicket(string Query, int TeckitId , int userId)
        {
            try
            {
                var connection=GetConnection();
                OpenConnection(connection);
                                
                var command = new SqlCommand(Query, connection);
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@TicketId";
                param.Value = TeckitId;
                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@UserId";
                param2.Value = userId;
                command.Parameters.Add(param);
                command.Parameters.Add(param2);
                command.ExecuteNonQuery();
            }
            catch (Exception ex) 
            { 
                Console.WriteLine (ex.Message);
            }
        }

        #endregion

        #region cancel ticket Book
        public void ExecuteCancelBookTicket(string Query, int TicketId)
        {
            try
            {
                var connection = GetConnection();
                OpenConnection(connection);

                var command = new SqlCommand(Query, connection);
                
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@TicketId";
                param.Value = TicketId;
                command.Parameters.Add(param);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

        #region insert Ticket
        public void ExecuteInsertTicket(string query, Ticket t)
        {
            try
            {
                var connection = GetConnection();
                OpenConnection(connection);

                var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Available", t.Available);
                command.Parameters.AddWithValue("@Price", t.Price);
                command.Parameters.AddWithValue("@StartDestination", t.StartDestination);
                command.Parameters.AddWithValue("@ArrivalDestination", t.ArrivalDestination);
                command.Parameters.AddWithValue("@BusId", t.BusId);
                command.Parameters.AddWithValue("@UserId", t.UserId);
                command.ExecuteNonQuery();

            }catch(Exception ex) 
            { 
                Console.WriteLine(ex.Message);
            }
        }

        #endregion
        
        #region Update Ticket
        public void ExecuteUpdateTicket(string query, Ticket t)
        {
            try
            {
                var connection = GetConnection();
                OpenConnection(connection);

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TicketId", t.TicketId);
                command.Parameters.AddWithValue("@Price", t.Price);
                command.Parameters.AddWithValue("@StartDestination", t.StartDestination);
                command.Parameters.AddWithValue("@ArrivalDestination", t.ArrivalDestination);
                command.Parameters.AddWithValue("@BusId", t.BusId);
                command.Parameters.AddWithValue("@UserId", t.UserId);

                command.ExecuteNonQuery();
            } catch(Exception ex) 
            { 
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
        
        #region Update busb
        public void ExecuteUpdateBus(string query, Bus t)
        {
            try { 
            var connection = GetConnection();
            OpenConnection(connection);

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@BusId", t.BusId);
            command.Parameters.AddWithValue("@NoOfSeats", t.NoOfSeats);
            command.Parameters.AddWithValue("@Region", t.Region);
            command.Parameters.AddWithValue("@DriverName", t.DriverName);
            command.Parameters.AddWithValue("@Plate", t.Plate);
             
                command.ExecuteNonQuery ();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }


}
