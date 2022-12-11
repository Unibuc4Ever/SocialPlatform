using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialPlatform.Models;

namespace SocialPlatform.Controllers
{
    public class UsersController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users);
        }
    }
}