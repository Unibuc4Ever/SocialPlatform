using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SocialPlatform.Models;

namespace SocialPlatform.Controllers
{
    public class UsersController : Controller
    {
        [Authorize]
        public ActionResult Settings()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var user = db.Users.Find(User.Identity.GetUserId());
            var reg_view = new RegisterViewModel();

            reg_view.FirstName = user.FirstName;
            reg_view.LastName = user.LastName;
            reg_view.WallIsVisible = user.WallIsVisible;
            reg_view.Email = user.Email;
            reg_view.Password = "123123@#$#@$#@32423sdfdsFSDFsd";
            reg_view.ConfirmPassword = "123123@#$#@$#@32423sdfdsFSDFsd";

            return View(reg_view);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Settings(RegisterViewModel user)
        {

            ApplicationDbContext db = new ApplicationDbContext();
            var real_user = db.Users.Find(User.Identity.GetUserId());

            try
            {
                real_user.FirstName = user.FirstName;
                real_user.LastName = user.LastName;
                real_user.WallIsVisible = user.WallIsVisible;
                if (ModelState.IsValid)
                    db.SaveChanges();
                else
                    return View(user);
            }
            catch (Exception e)
            {
                return View(user);
            }
            return RedirectToAction("Settings");
        }

        // Returns the profile of a user
        [Authorize]
        public ActionResult Show(string id, int? frommaybe)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            int from = frommaybe ?? 0;

            try
			{
                var user = db.Users.Find(id);
                
                if (user.Posts.Count() < from || from < 0)
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);

                if (from > 0 && user.Friends.Count(usr => usr.Id == User.Identity.GetUserId()) == 0
                        && user.WallIsVisible == false)
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);

                ViewBag.PostNr = from;

                return View(user);
			}
            catch(Exception e)
			{

			}
            ViewBag.Message = "Unable to find user!";
            return RedirectToAction("Index");
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
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(db));
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser other = db.Users.Find(otherID);
                    if (other == null)
                        throw new Exception();

                    if (user.SentFriendRequests.Contains(other) || user.Friends.Contains(other))
                        return RedirectToAction("Index");
                    
                    if (user.ReceivedFriendRequests.Contains(other)) {
                        return AcceptFriendRequest(otherID);
					}

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

        [AcceptVerbs(HttpVerbs.Put | HttpVerbs.Post)]
        [Authorize]
        public ActionResult AcceptFriendRequest(string otherID)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(db));
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

        [AcceptVerbs(HttpVerbs.Put | HttpVerbs.Post)]
        [Authorize]
        public ActionResult DeclineFriendRequest(string otherID)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(db));
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

        [AcceptVerbs(HttpVerbs.Put | HttpVerbs.Post)]
        [Authorize]
        public ActionResult CancelFriendRequest(string otherID)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(db));
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

        [AcceptVerbs(HttpVerbs.Put | HttpVerbs.Post)]
        [Authorize]
        public ActionResult Unfriend(string otherID)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(db));
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
