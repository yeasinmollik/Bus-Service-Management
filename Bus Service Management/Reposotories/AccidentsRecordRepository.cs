using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripSafe.Models;
using System.Configuration;
using MySql.Data.MySqlClient;


namespace TripSafe.Repositories
{
    public class AccidentsRecordRepository
    {
        private string constr;
        public AccidentsRecordRepository()
        {
            this.constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
        }
        public void insert(AccidentRecords record)
        {
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = $@"INSERT INTO `bus_management_system`.`accident_records`
                                    (`time`,
                                    `roadId`,
                                    `reason`,
                                    `busId`,
                                    `fatalities`)
                                    VALUES
                                    ({record.time},{record.roadId},{record.reason},{record.busId},{record.fatalities});";

                using (MySqlCommand newCommand = new MySqlCommand(query))
                {
                    newCommand.Connection = con;
                    con.Open();
                    newCommand.ExecuteNonQuery();
                    con.Close();
                }
            }

        }
        public List<Object> getAll( )
        {
            List<Object> records = new List<Object>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = $@"SELECT time, roadId, reason, busId, fatalities,
                                (select name from road where road.Id= accident_records.roadId) as roadName,
                                (select name from bus where bus.Id= accident_records.busId) as busName
                                 FROM bus_management_system.accident_records;";

                using (MySqlCommand newCommand = new MySqlCommand(query))
                {
                    newCommand.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = newCommand.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            records.Add(new
                            {
                                time = Convert.ToInt64(sdr["time"].ToString()),
                                roadId = Convert.ToInt32(sdr["roadId"].ToString()),
                                reason = sdr["reason"].ToString(),
                                busId = Convert.ToInt32(sdr["busId"].ToString()),
                                fatalities = Convert.ToInt32(sdr["fatalities"].ToString()),
                                roadName = sdr["roadName"].ToString(),
                                busName = sdr["busName"].ToString()
                            }); ;
                        }
                    }
                    con.Close();
                }


            }
            return records;
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



    }
}