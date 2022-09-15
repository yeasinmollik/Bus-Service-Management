using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripSafe.Models;
using System.Configuration;
using MySql.Data.MySqlClient;
namespace TripSafe.Repositories
{
    public class DistrictRepository
    {
        private string constr;
        public DistrictRepository()
        {
            this.constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

        }
        public List<District> GetDistricts()
        {
            List<District> districts = new List<District>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "SELECT * FROM district";
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

                                });
                            }
                        }
                        con.Close();
                    }

                }
            }
            return districts;
        }
    
    
    }
}