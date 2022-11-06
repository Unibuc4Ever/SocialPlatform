using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialPlatform.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Ar trebui sa verifice daca user-ul este logat
            // Daca este logat, atunci ar trebui sa faca redirect la "/Posts"
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}