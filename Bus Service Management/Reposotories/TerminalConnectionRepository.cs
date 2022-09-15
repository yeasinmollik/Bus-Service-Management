using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripSafe.Models;
using System.Configuration;
using MySql.Data.MySqlClient;
namespace TripSafe.Repositories
{
    public class TerminalConnectionRepository
    {
        private string constr;
        public TerminalConnectionRepository()
        {
            this.constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

        }
        public void create(TerminalConnection connection)
        {
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "INSERT INTO terminal_connection(terminal1,terminal2,roadId) VALUES (?1,?2,?3);";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("?1", connection.terminal1);
                    cmd.Parameters.AddWithValue("?2", connection.terminal2);
                    cmd.Parameters.AddWithValue("?3", connection.roadId);
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
   
        public Object getAll()
        {
            List<Object> connections = new List<Object>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "select terminal1,terminal2,roadId,(select name from terminal where terminal.Id=terminal_connection.terminal1) as terminal1Name, (select name from terminal where terminal.Id=terminal_connection.terminal2) as terminal2Name,(select name from road where road.Id=terminal_connection.roadId) as roadName from terminal_connection;";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    using (MySqlCommand newCommand = new MySqlCommand(query))
                    {
                        newCommand.Connection = con;
                        con.Open();
                        using (MySqlDataReader sdr = newCommand.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                connections.Add(new
                                {
                                    terminal1 = Convert.ToInt32(sdr["terminal1"]),
                                    terminal2 = Convert.ToInt32(sdr["terminal2"]),
                                    roadId = Convert.ToInt32(sdr["roadId"]),
                                     

                                    terminal1Name = sdr["terminal1Name"].ToString(),
                                    terminal2Name = sdr["terminal2Name"].ToString(),

                                    roadName = sdr["roadName"].ToString()
                                }); ;
                            }
                        }
                        con.Close();
                    }

                }
            }
            return connections;
        }
    
    }
}