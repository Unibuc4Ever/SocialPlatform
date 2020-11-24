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
    public class CommentsController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new
             UserStore<ApplicationUser>(db));

        [HttpPost]
        [Authorize]
        public ActionResult New(int post_id, Comment comment)
        {
            comment.createdAt = DateTime.Now;
            comment.author = db.Users.Find(User.Identity.GetUserId());

            try
            {
                if (ModelState.IsValid)
                {
                    comment.Post = db.Posts.Find(post_id);
                    db.Comments.Add(comment);
                    db.SaveChanges();
                    TempData["message"] = "Commentul a fost adaugat!";
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Bad things happen in Philadelphia");
            }
            return RedirectToRoute("/Posts/Show/" + post_id);
        }

    }
}