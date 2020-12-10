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
        // GET: Users
        public ActionResult Home()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            return View(user);
        }

        [Authorize]
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
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
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(db));
        ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser other = db.Users.Find(fr.OtherId);
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
                return View(fr);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult SentFriendRequests()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(db));
            return View(userManager.FindById(User.Identity.GetUserId()));
        }

        [HttpGet]
        [Authorize]
        public ActionResult ReceivedFriendRequests()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(db));
            return View(userManager.FindById(User.Identity.GetUserId()));
        }

        [HttpGet]
        [Authorize]
        public ActionResult Friends()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(db));
            return View(userManager.FindById(User.Identity.GetUserId()));
        }

        [HttpPut]
        [Authorize]
        public ActionResult AcceptFriendRequest(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(db));
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                ApplicationUser other = db.Users.Find(id);
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
        public ActionResult DeclineFriendRequest(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(db));
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                ApplicationUser other = db.Users.Find(id);
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
        public ActionResult CancelFriendRequest(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(db));
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                ApplicationUser other = db.Users.Find(id);
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
        public ActionResult Unfriend(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(db));
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                ApplicationUser other = db.Users.Find(id);
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
