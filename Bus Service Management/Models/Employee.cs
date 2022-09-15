using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripSafe.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public String name { get; set; }
        public String phone { get; set; }
        public String password { get; set; }
        public int employeeType { get; set; }
        public String image { get; set; }
    }
}