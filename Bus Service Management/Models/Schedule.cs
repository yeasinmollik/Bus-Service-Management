using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripSafe.Models
{
    public class Schedule
    {
        public int terminalId { get; set; }
        public int routeId { get; set; }
        public int departureTime { get; set; }
        public int arrivalTime { get; set; }
        public int stoppageIndex { get; set; }
    }
}