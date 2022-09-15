using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripSafe.Models
{
    public class Maintenance
    {
        public int busId { get; set; }
        public int terminalId { get; set; }
        public int startTime { get; set; }
        public int endTime { get; set; }
        public int type { get; set; }
        public int mechanicId { get; set; }
    }
}