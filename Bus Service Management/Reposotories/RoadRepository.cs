using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripSafe.Models;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace TripSafe.Repositories
{
    public class RoadRepository
    {
        private string constr;
        public RoadRepository()
        {
            this.constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

        }
        public List<Object> GetRoads()
        {
            List<Object> roads = new List<Object>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = @"select road.Id, road.name,road.length, 
                                    T.name as terminal1Name,
                                    S.name as terminal2Name from road, 
                                    terminal as T, terminal as S, 
                                    terminal_connection where 
                                    terminal_connection.roadId= road.Id and 
                                    terminal_connection.terminal1=T.Id and 
                                    terminal_connection.terminal2=S.Id;";
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
                                roads.Add(new  
                                {
                                    Id = Convert.ToInt32(sdr["Id"]),
                                    name = sdr["name"].ToString(),
                                   
                                    terminal1Name= sdr["terminal1Name"].ToString(),
                                    terminal2Name = sdr["terminal2Name"].ToString(),

                                    length = Convert.ToDouble(sdr["length"])
                                }); ; 
                            }
                        }
                        con.Close();
                    }

                }
            }
            return roads;
        }

        public Road insertRoad(Road road)
        {
            List<Road> roads = new List<Road>();

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "INSERT INTO road(name,length) VALUES (?1, ?2);";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("?1", road.name);

                    cmd.Parameters.AddWithValue("?2", road.length);
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    con.Close();
                    using (MySqlCommand newCommand = new MySqlCommand("select * from road  where road.Id=(select max(Id) from road)"))
                    {
                        newCommand.Connection = con;
                        con.Open();
                        using (MySqlDataReader sdr = newCommand.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                roads.Add(new Road
                                {
                                    Id = Convert.ToInt32(sdr["Id"]),
                                    name = sdr["name"].ToString(),

                                });
                            }
                        }
                        con.Close();
                    }
                    con.Close();
                }
            }
            return roads[0];
        }
    }
}