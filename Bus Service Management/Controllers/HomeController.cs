using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using TripSafe.Models;
 
using TripSafe.Reposotories;

namespace TripSafe.Controllers
{
    public class Query
    {
        public int start_terminal { get; set; }
        public int arrivalTime { get; set; }
        public int date { get; set; }
        public int end_terminal { get; set; }
        public int vacancy { get; set; }

    }
    public class HomeController : Controller
    {
        private TicketRepository ticketRepository;
        public HomeController()
        {
            ticketRepository = new TicketRepository();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public Object search(Query query)
        {
            return Json(ticketRepository.searchBus(query.start_terminal, query.end_terminal, query.arrivalTime, query.vacancy, query.date), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public Object purchaseTicket(SearchQuery searchQuery)
        {
            return Json(ticketRepository.createTicket(searchQuery));
        }
        [HttpGet]
        public Object getTickets(String phone)
        {
            return Json(ticketRepository.getTickets(phone),JsonRequestBehavior.AllowGet);

        }
    }
}