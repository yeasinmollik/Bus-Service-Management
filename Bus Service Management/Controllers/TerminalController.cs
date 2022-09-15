using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripSafe.Repositories;
using TripSafe.Models;
namespace TripSafe.Controllers
{
    public class TerminalController : Controller
    {
        TerminalRepository terminalRepository;
        public TerminalController()
        {
            terminalRepository = new TerminalRepository();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public Object addNew(Terminal terminal)
        {
            return Json(terminalRepository.addNew(terminal), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public Object delete(Terminal terminal)
        {
            terminalRepository.delete(terminal);
            return Json(new { data = 1 }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public Object getDistricts()
        {
            return Json(terminalRepository.getDistricts(), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public Object getTerminals()
        {
            var data = terminalRepository.getTerminals();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}