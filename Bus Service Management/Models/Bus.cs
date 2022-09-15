using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripSafe.Models
{
    public class Bus
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int capacity { get; set; }
        public string status { get; set; }
        public int isActive { get; set; }
        public int rem_vacancy { get; set; }
    }
}