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

        [Authorize]
        public ActionResult Index()
		{
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            return View(user);
        }

        // GET: MakeFriendship
        // Should be deleted this controller
        [Authorize]
        public ActionResult AddFriendRequest()
        {
            return View();
        }

        // POST: MakeFriendship
        [HttpPost]
        [Authorize]
        public ActionResult AddFriendRequest(string otherID)
        {
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser other = db.Users.Find(otherID);
                    if (other == null)
                        throw new Exception();

                    user.SentFriendRequests.Add(other);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                    throw new Exception();
            }
            catch (Exception)
            {
                ModelState.AddModelError("otherID", "ID of the friend is not valid");
                return View();
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult SentFriendRequests()
        {
            return View(userManager.FindById(User.Identity.GetUserId()));
        }

        [HttpGet]
        [Authorize]
        public ActionResult ReceivedFriendRequests()
        {
            return View(userManager.FindById(User.Identity.GetUserId()));
        }

        [HttpGet]
        [Authorize]
        public ActionResult Friends()
        {
            return View(userManager.FindById(User.Identity.GetUserId()));
        }

        [HttpPut]
        [Authorize]
        public ActionResult AcceptFriendRequest(string otherID)
        {
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                ApplicationUser other = db.Users.Find(otherID);
                if (user.ReceivedFriendRequests.Contains(other))
                {
                    user.Friends.Add(other);
                    other.Friends.Add(user);
                    user.ReceivedFriendRequests.Remove(other);
                    db.SaveChanges();
                }
            }
            catch (Exception) { }
            return RedirectToAction("ReceivedFriendRequests");
        }

        [HttpPut]
        [Authorize]
        public ActionResult DeclineFriendRequest(string otherID)
        {
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                ApplicationUser other = db.Users.Find(otherID);
                if (user.ReceivedFriendRequests.Contains(other))
                {
                    user.ReceivedFriendRequests.Remove(other);
                    db.SaveChanges();
                }
            }
            catch (Exception) { }
            return RedirectToAction("ReceivedFriendRequests");
        }

        [HttpPut]
        [Authorize]
        public ActionResult CancelFriendRequest(string otherID)
        {
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                ApplicationUser other = db.Users.Find(otherID);
                if (user.SentFriendRequests.Contains(other))
                {
                    user.SentFriendRequests.Remove(other);
                    db.SaveChanges();
                }
            }
            catch (Exception) { }
            return RedirectToAction("SentFriendRequests");
        }

        [HttpPut]
        [Authorize]
        public ActionResult Unfriend(string otherID)
        {
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                ApplicationUser other = db.Users.Find(otherID);
                if (user.Friends.Contains(other))
                {
                    user.Friends.Remove(other);
                    other.Friends.Remove(user);
                    db.SaveChanges();
                }
            }
            catch (Exception) { }
            return RedirectToAction("Friends");

        }
    }
}
