using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripSafe.Models
{
    public class Terminal
    {
        public int Id { get; set; }
        public String name { get; set; }
        public int districtId { get; set; }
    }
}