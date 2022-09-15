using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripSafe.Models
{
    public class SearchQuery
    {
        public int userId { get; set; }
        public String phone { get; set; }
        public int start_terminal { get; set; }
        public int end_terminal { get; set; }
        public int routeId { get; set; }
        public int seats { get; set; }
        public int date { get; set; }
        public string name { get; set; }
    }
}