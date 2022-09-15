using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripSafe.Models;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace TripSafe.Repositories
{
    public class ScheduleRepository
    {
        private string constr;
        public ScheduleRepository()
        {
            this.constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

        }
        public void insert(Schedule schedule)
        {


            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "INSERT INTO  schedule (terminalId,routeId,arrivalTime,departureTime,stoppageIndex)VALUES(?1,?2,?3,?4,?5);";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("?1", schedule.terminalId);

                    cmd.Parameters.AddWithValue("?2", schedule.routeId);
                    cmd.Parameters.AddWithValue("?3", schedule.arrivalTime);

                    cmd.Parameters.AddWithValue("?4", schedule.departureTime);
                    cmd.Parameters.AddWithValue("?5", schedule.stoppageIndex);

                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    con.Close();

                }
            }

        }

        // todo: add many queries

        public List<Object> getMany(string query)
        {
            List<Object> schedules = new List<Object>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {

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
                                schedules.Add(new
                                {
                                    terminalId = Convert.ToInt32(sdr["terminalId"]),
                                    routeId = Convert.ToInt32(sdr["routeId"]),
                                    arrivalTime = Convert.ToInt32(sdr["arrivalTime"]),
                                    departureTime = Convert.ToInt32(sdr["departureTime"]),
                                    stoppageIndex = Convert.ToInt32(sdr["stoppageIndex"]),
                                    terminalName = sdr["terminalName"].ToString(),

                                }); ;
                            }
                        }
                        con.Close();
                    }

                }
            }
            return schedules;
        }


    }
}