using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SocialPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialPlatform.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.Users);
        }

        public ActionResult Dummy()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.Users);
        }
    }
}