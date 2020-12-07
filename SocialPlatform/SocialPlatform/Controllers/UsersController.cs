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
        // GET: Users  Ar trebui stearsa chestia asta
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
                    ApplicationUser other = db.Users.Find(fr.otherID);
                    user.sentFriendRequests.Add(other);
                    // other.receivedFriendRequests.Add(user);

                    db.SaveChanges();
                  
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
        public ActionResult AcceptFriendRequest(string id)
        {
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                ApplicationUser other = db.Users.Find(id);
                if (user.receivedFriendRequests.Contains(other))
                {
                    user.friends.Add(other);
                    other.friends.Add(user);
                    user.receivedFriendRequests.Remove(other);
                    //other.sentFriendRequests.Remove(user);
                    db.SaveChanges();

                    return RedirectToAction("ReceivedFriendRequests");
                }
                else
                {
                    return RedirectToAction("ReceivedFriendRequests");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("ReceivedFriendRequests");
            }
        }

        [HttpPut]
        [Authorize]
        public ActionResult DeclineFriendRequest(string id)
        {
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                ApplicationUser other = db.Users.Find(id);
                if (user.receivedFriendRequests.Contains(other))
                {
                    user.receivedFriendRequests.Remove(other);
                    //other.sentFriendRequests.Remove(user);
                    db.SaveChanges();

                    return RedirectToAction("ReceivedFriendRequests");
                }
                else
                {
                    return RedirectToAction("ReceivedFriendRequests");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("ReceivedFriendRequests");
            }
        }

        [HttpPut]
        [Authorize]
        public ActionResult CancelFriendRequest(string id)
        {
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                ApplicationUser other = db.Users.Find(id);
                if (user.sentFriendRequests.Contains(other))
                {
                    user.sentFriendRequests.Remove(other);
                    //other.receivedFriendRequests.Remove(user);
                    db.SaveChanges();

                    return RedirectToAction("SentFriendRequests");
                }
                else
                {
                    return RedirectToAction("SentFriendRequests");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("SentFriendRequests");
            }
        }

        [HttpPut]
        [Authorize]
        public ActionResult Unfriend(string id)
        {
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                ApplicationUser other = db.Users.Find(id);
                if (user.friends.Contains(other))
                {
                    user.friends.Remove(other);
                    other.friends.Remove(user);
                    db.SaveChanges();

                    return RedirectToAction("Friends");
                }
                else
                {
                    return RedirectToAction("Friends");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Friends");
            }
        }
    }
}