using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripSafe.Models;
using TripSafe.Repositories;

namespace TripSafe.Controllers
{
    public class RouteController : Controller
    {
        private RouteRepository routeRepository;
        private ScheduleRepository scheduleRepository;
        public RouteController()
        {
            routeRepository = new RouteRepository();
            scheduleRepository = new ScheduleRepository();
        }
        // GET: Route
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult viewDetails(int routeId)
        {
            Route routeInfo = routeRepository.findRoute(routeId);
            ViewBag.routeId = routeId;
            ViewBag.busId = routeInfo.busId;
            ViewBag.routeInfo = routeInfo.name;
            return View();
        }
        public object getRoutes()
        {
            var datas = routeRepository.getRoutes();
            return Json(datas, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult createRoute()
        {
            return View();
        }
        [HttpPost]
        public Object insert(Route newRoute)
        {
            Route data = routeRepository.create(newRoute);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public Object getRouteInfo(int routeId)
        {
            return Json(routeRepository.findRoute(routeId), JsonRequestBehavior.AllowGet);
        }
    }
}