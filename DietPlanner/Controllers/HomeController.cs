using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DietPlanner.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            TempData["Success"] = "sukces";
            TempData["Warning"] = "achtung";

            return View();
        }

        public PartialViewResult ShowAlerts()
        {
            return PartialView("_Alerts");
        }
    }
}