using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripSafe.Models;
using System.Configuration;
using MySql.Data.MySqlClient;


namespace TripSafe.Repositories
{
    public class BusRepository
    {
        private string constr;
        public BusRepository()
        {
            this.constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
        }
        public Bus findBus(int Id)
        {
            Bus currentBus = new Bus();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = $"select * from bus where bus.Id={Id}";
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
                                currentBus = new Bus
                                {
                                    Id = Convert.ToInt32(sdr["Id"]),
                                    name = sdr["name"].ToString(),
                                    status = sdr["status"].ToString(),
                                    capacity = Convert.ToInt32(sdr["capacity"]),
                                    isActive = Convert.ToInt32(sdr["isActive"]),
                                    rem_vacancy = Convert.ToInt32(sdr["rem_vacancy"])
                                }; ;
                            }
                        }
                        con.Close();
                    }

                }
            }
            return currentBus;
        }

        public Bus insert(Bus newBus)
        {


            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "INSERT INTO bus(name,capacity,status,isActive,rem_vacancy) VALUES(?1,?2,?3,?4,?5);";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("?1", newBus.name);
                    cmd.Parameters.AddWithValue("?2", newBus.capacity);
                    cmd.Parameters.AddWithValue("?3", newBus.status);
                    cmd.Parameters.AddWithValue("?4", newBus.isActive);
                    cmd.Parameters.AddWithValue("?5", newBus.rem_vacancy);
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    con.Close();
                    using (MySqlCommand newCommand = new MySqlCommand("select max(Id) from road"))
                    {
                        newCommand.Connection = con;
                        con.Open();
                        using (MySqlDataReader sdr = newCommand.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                newBus.Id = Convert.ToInt32(sdr["Id"]);
                            }
                        }
                        con.Close();
                    }
                    con.Close();
                }
            }
            return newBus;
        }

        public void update(Bus bus)
        {
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand newCommand = new MySqlCommand($@"update bus set status=?1
                                    where bus.id = ?2; "))
                {
                    newCommand.Parameters.AddWithValue("?1", bus.status);
                    newCommand.Parameters.AddWithValue("?2", bus.Id);
                    newCommand.Connection = con;
                    con.Open();
                    newCommand.ExecuteNonQuery();
                    con.Close();
                }

            }
        }


        public List<Bus> findAllBus(String query)
        {
            List<Bus> buses = new List<Bus>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand newCommand = new MySqlCommand(query))
                {
                    newCommand.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = newCommand.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            buses.Add(new Bus
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                name = sdr["name"].ToString(),
                                status = sdr["status"].ToString(),
                                capacity = Convert.ToInt32(sdr["capacity"]),
                                isActive = Convert.ToInt32(sdr["isActive"]),
                                rem_vacancy = Convert.ToInt32(sdr["rem_vacancy"])
                            });
                        }
                    }
                    con.Close();
                }

            }
            return buses;
        }


    }
}