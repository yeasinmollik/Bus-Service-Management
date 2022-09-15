using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TripSafe.Models;
using TripSafe.Repositories;

namespace TripSafe.Controllers
{
    public class BusController : Controller
    {
        BusRepository busRepository;
        public BusController()
        {
            busRepository = new BusRepository();
        }
        // GET: Bus
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public Object update(Bus newBus)
        {
            busRepository.update(newBus);
            return Json(new { status = 1 }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public Object getAllBusList()
        {
            return Json(this.busRepository.findAllBus($"select * from bus;"), JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public Object getUnAssignedBuses()
        {
            return Json(this.busRepository.findAllBus($"select * from bus where bus.id not in (select route.busId from route);"), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public Object findBus(int busId)
        {
            var bus = busRepository.findBus(busId);
            return Json(bus, JsonRequestBehavior.AllowGet);
        }
    }
}