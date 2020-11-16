using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SocialPlatform.Models;

namespace SocialPlatform.Controllers
{
    public class UsersController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new
            UserStore<ApplicationUser>(db));
        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users);
        }

        // GET: MakeFriendship
        [Authorize]
        public ActionResult FriendRequest()
        {
            return View(new FriendRequest());
        }

        // POST: MakeFriendship
        [HttpPost]
        [Authorize]
        public ActionResult FriendRequest(FriendRequest fr)
        {
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                if (ModelState.IsValid) {
                    // ApplicationUser other = userManager.FindById(fr.otherID);
                    
                    user.friendRequests.Add(db.Users.Find(fr.otherID));
                    
                    TempData["message"] = "Now I have " + user.friendRequests.Count + " friend requests";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(fr);
                }
            }
            catch (Exception)
            {
                return View(fr);
            }
        }
    }
}