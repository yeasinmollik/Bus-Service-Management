using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripSafe.Models;
using System.Configuration;
using MySql.Data.MySqlClient;


namespace TripSafe.Controllers
{
    public class DistrictController : Controller
    {
        // GET: District
        [HttpPost]
        public object InsertDistrict(String districtName)
        {
            string constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
            List<District> districts = new List<District>();

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "insert into district(name) values(?1)";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;

                    cmd.Parameters.AddWithValue("?1", districtName);
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    con.Close();
                    using (MySqlCommand newCommand = new MySqlCommand("select * from district  where district.Id=(select max(Id) from district)"))
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
                    con.Close();
                }
            }
            return Json(districts[0], JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public int RemoveDistrict(int id)
        {
            string constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
            using(MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "DELETE FROM district where id=?1";
                using(MySqlCommand cmd=new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("?1", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return 1;
        }

        [HttpGet]
        public object GetDistrictNames()
        {
            string constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
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
            return Json(districts , JsonRequestBehavior.AllowGet);
        }
    }
}