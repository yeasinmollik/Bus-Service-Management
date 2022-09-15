using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripSafe.Models
{
    public class Route
    {
        public int Id { get; set; }
        public String name { get; set; }
        public int start_terminal { get; set; }
        public int end_terminal { get; set; }
        public int busId { get; set; }
    }
}