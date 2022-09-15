using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripSafe.Models;
using System.Configuration;
using MySql.Data.MySqlClient;


namespace TripSafe.Repositories
{
    public class TerminalRepository
    {
        private string constr;
        public TerminalRepository()
        {
            this.constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

        }
        public Terminal addNew(Terminal terminal)
        {
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = $@"INSERT INTO  terminal 
                                    ( 
                                     name ,
                                     districtId )
                                    VALUES
                                    ( ?1,?2 );";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("?1", terminal.name);
                    cmd.Parameters.AddWithValue("?2", terminal.districtId);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    using (MySqlCommand newCommand = new MySqlCommand("select max(ID) from terminal;"))
                    {
                        newCommand.Connection = con;
                        con.Open();
                        using (MySqlDataReader sdr = newCommand.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                terminal.Id = Convert.ToInt32(sdr["Id"]);
                            }
                        }
                        con.Close();
                    }

                }
            }
            return terminal;
        }
        public void delete(Terminal terminal)
        {
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = $@"delete from terminal 
                                where terminal.Id={terminal.Id};";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    using (MySqlCommand newCommand = new MySqlCommand(query))
                    {
                        newCommand.Connection = con;
                        con.Open();
                        con.Close();
                    }

                }
            }

        }
        public List<District> getDistricts()
        {
            List<District> districts = new List<District>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = @" select * from district;";
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
                                districts.Add(new District
                                {
                                    Id = Convert.ToInt32(sdr["Id"]),
                                    name = sdr["name"].ToString(),

                                }); ;
                            }
                        }
                        con.Close();
                    }

                }
            }
            return districts;
        }
        public List<Object> getTerminals()
        {
            List<Object> terminals = new List<Object>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = @"SELECT Id, name, districtId,
                                (select name from district where district.Id=terminal.districtId) as districtName
                                 FROM bus_management_system.terminal;";
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
                                terminals.Add(new
                                {
                                    Id = Convert.ToInt32(sdr["Id"]),
                                    name = sdr["name"].ToString(),
                                    districtId = Convert.ToInt32(sdr["districtId"]),
                                    districtName = sdr["districtName"].ToString(),
                                }); ;
                            }
                        }
                        con.Close();
                    }

                }
            }
            return terminals;
        }

    }
}