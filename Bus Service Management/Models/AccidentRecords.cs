using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripSafe.Models
{
    public class AccidentRecords
    {
        public int time { get; set; }
        public int roadId { get; set; }
        public String reason { get; set; }
        public int busId { get; set; }
        public int fatalities { get; set; }
    }
}