using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripSafe.Models;
using System.Configuration;
using MySql.Data.MySqlClient;
namespace TripSafe.Repositories
{
    public class EmployeeRepository
    {
        private string constr;
        public EmployeeRepository()
        {
            this.constr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

        }
        public List<Employee> getAll()
        {
            List<Employee> employees = new List<Employee>();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "SELECT * FROM  employee;";
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
                                employees.Add(new Employee
                                {
                                    Id = Convert.ToInt32(sdr["Id"]),
                                    name = sdr["name"].ToString(),
                                    phone = (sdr["phone"].ToString()),
                                    employeeType = Convert.ToInt32(sdr["employeeType"]),
                                    image = sdr["image"].ToString()
                                });
                            }
                        }
                        con.Close();
                    }

                }
            }
            return employees;
        }
        public void delete(Employee employee)
        {
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = $"delete  FROM  employee where employee.Id={employee.Id} ;";

                using (MySqlCommand newCommand = new MySqlCommand(query))
                {
                    newCommand.Connection = con;
                    con.Open();
                    con.Close();
                }
            }

        }

        public Employee addNew(Employee employee)
        {
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = $@"INSERT INTO `bus_management_system`.`employee`
                                ( name,
                                phone,
                                password,
                                employeeType,
                                image)
                                VALUES
                                (  {employee.name },
                                {employee.phone },
                                {employee.password },
                                {employee.employeeType },
                                {employee.image });";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    using (MySqlCommand newCommand = new MySqlCommand("select max(Id) from employee;"))
                    {
                        newCommand.Connection = con;
                        con.Open();
                        using (MySqlDataReader sdr = newCommand.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                employee.Id = Convert.ToInt32(sdr["Id"]);
                            }
                        }
                        con.Close();
                    }

                }
            }
            return employee;
        }
    }
}