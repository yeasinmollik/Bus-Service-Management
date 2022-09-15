using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripSafe.Models;
using TripSafe.Repositories;
namespace TripSafe.Controllers
{
    public class AccidentsController : Controller
    {
        private AccidentsRecordRepository accidentsRecordRepository;
        public AccidentsController()
        {
            accidentsRecordRepository = new AccidentsRecordRepository();
        }
        // GET: Accidents
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public Object getAll()
        {
            return Json(accidentsRecordRepository.getAll(),JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public Object insert(AccidentRecords record)
        {
            accidentsRecordRepository.insert(record);
            return Json(new { data = 1 }, JsonRequestBehavior.AllowGet);

        }
    }
}