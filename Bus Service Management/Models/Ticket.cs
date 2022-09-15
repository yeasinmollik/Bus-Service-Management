using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripSafe.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int passengerId { get; set; }
        public int  passengerCount { get; set; }
        public int  startTerminal { get; set; }
        public int  endTerminal { get; set; }
        public int  tripId { get; set; }

    }
}