using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TripSafe.Models;
using TripSafe.Repositories;

namespace TripSafe.Controllers
{
    public class EmployeeController : Controller
    {
        private EmployeeRepository employeeRepository;
        public EmployeeController()
        {
            employeeRepository = new EmployeeRepository();
        }
        [HttpGet]
        public Object getAll()
        {
            return Json(employeeRepository.getAll(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public Object addNew(Employee employee)
        {
            return Json(this.employeeRepository.addNew(employee), JsonRequestBehavior.AllowGet);
        }
        public Object delete(Employee employee)
        {
            this.employeeRepository.delete(employee);
            return Json(new { data = 1 }, JsonRequestBehavior.AllowGet);
        }
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }
    }
}