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
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.Users);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult ChangeRole(string user_id, string new_role)
        {
            try
            {
                var db = new ApplicationDbContext();
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
                    (new UserStore<ApplicationUser>(db));

                if (!new string[3] { "Administrator", "Editor", "User" }.Contains(new_role))
                    throw new Exception();

                userManager.RemoveFromRoles(user_id, new string[] { userManager.GetRoles(user_id).First() });
                userManager.AddToRole(user_id, new_role);
            }
            catch(Exception e) { }

            string user_wall_route = "/Users/Show/" + user_id;
            return Redirect(user_wall_route);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteUser(string id)
        {
            var db = new ApplicationDbContext();
            try
            {
                SocialWorker worker = new SocialWorker();
                worker.DeleteUser(id, ref db);
            }
            catch (Exception e) {
                return Redirect("/Users/Show/" + id);
            }

            string user_wall_route = "/";
            return Redirect(user_wall_route);
        }

        public ActionResult Dummy()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.Users);
        }
    }
}