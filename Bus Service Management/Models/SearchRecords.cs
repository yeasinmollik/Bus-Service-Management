using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripSafe.Models
{
    public class SearchRecords
    {
        public int start_terminal { get; set; }
        public int end_terminal { get; set; }
        public int time { get; set; }
        public int passengerCount { get; set; }
        public int isFound { get; set; }
    }
}