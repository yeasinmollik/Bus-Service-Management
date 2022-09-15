using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripSafe.Models;
 
using TripSafe.Reposotories;

namespace TripSafe.Controllers
{
    public class TicketController : Controller
    {
        private TicketRepository ticketRepository;
        public TicketController()
        {
            ticketRepository = new TicketRepository();
        }
        // GET: Ticket
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult dailyBoardingInfo()
        {
            return View();
        }


        [HttpGet]
        public Object getDailyBoardingInfo(int day)
        {
            return Json(ticketRepository.getDailyBoardingUnboardingInfo(day),JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DailyTickets(int day)
        {
           return Json(ticketRepository.getDailyTickets(day), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public Object getDailyTicketPurchases()
        {
             return View();
        }
    }
}