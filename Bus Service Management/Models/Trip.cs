using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripSafe.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public int driverId { get; set; }
        public int routeId { get; set; }
        public int date { get; set; }
    }
}