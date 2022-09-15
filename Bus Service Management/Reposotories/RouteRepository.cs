using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripSafe.Models;
using System.Configuration;
using MySql.Data.MySqlClient;
namespace TripSafe.Repositories
{
    public class RouteRepository
    {
        private string constr;
        public RouteRepository()
        {
            this.constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

        }
        public Route findRoute(int routeId)
        {
            Route route = new Route();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = $"select * from route where route.Id={routeId};";
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
                                route = (new Route
                                {
                                    Id = Convert.ToInt32(sdr["Id"]),
                                    name = sdr["name"].ToString(),

                                    start_terminal = Convert.ToInt32(sdr["start_terminal"]),
                                    end_terminal = Convert.ToInt32(sdr["end_terminal"]),
                                    busId = Convert.ToInt32(sdr["busId"])

                                }); ;
                            }
                        }
                        con.Close();
                    }

                }
            }
            return route;
        }
        public List<Object> getRoutes()
        {
            List<Object> routes = new List<Object>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = $@"select route.Id,name,busId, 
                                (select name from bus where bus.id= route.busId) as busName, 
                                (select name from terminal where terminal.Id = route.start_terminal) as startTerminalName, 
                                (select name from terminal where terminal.Id = route.end_terminal) as endTerminalName, 
                                route.start_terminal, route.end_terminal from route ;";
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
                                routes.Add(new
                                {
                                    Id = Convert.ToInt32(sdr["Id"]),
                                    name = sdr["name"].ToString(),

                                    start_terminal = Convert.ToInt32(sdr["start_terminal"].ToString()),
                                    end_terminal = Convert.ToInt32(sdr["end_terminal"].ToString()),
                                    busName = sdr["busName"].ToString(),
                                    startTerminalName = sdr["startTerminalName"].ToString(),
                                    endTerminalName = sdr["endTerminalName"].ToString(),
                                    busId = Convert.ToInt32(sdr["busId"].ToString())

                                }); ;
                            }
                        }
                        con.Close();
                    }

                }
            }
            return routes;
        }
        public Route create(Route newRoute)
        {
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "INSERT INTO route( start_terminal,end_terminal,name, busId )VALUES(?1,?2,?3,?4);";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("?1", newRoute.start_terminal);

                    cmd.Parameters.AddWithValue("?2", newRoute.end_terminal);
                    cmd.Parameters.AddWithValue("?3", newRoute.name);

                    cmd.Parameters.AddWithValue("?4", newRoute.busId);

                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    con.Close();
                    using (MySqlCommand newCommand = new MySqlCommand("select max(Id) as Id from route;"))
                    {
                        newCommand.Connection = con;
                        con.Open();
                        using (MySqlDataReader sdr = newCommand.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                newRoute.Id = Convert.ToInt32(sdr["Id"]);

                            }
                        }
                        con.Close();

                        con.Close();
                    }
                }
                return newRoute;
            }
        }
    }
}