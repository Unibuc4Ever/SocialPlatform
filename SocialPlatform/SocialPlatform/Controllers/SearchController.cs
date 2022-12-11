using SocialPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialPlatform.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        // Search the word id.
        // If id is null, then redirect to default
        public ActionResult Index(string id)
        {
            var db = new ApplicationDbContext();
            if (id == null)
                return RedirectToRoute("/");

            id = id.Trim();

            if (id.Length == 0)
                return RedirectToRoute("/");

            var users = db.Users.Where(user =>
                (user.FirstName + " " + user.LastName).Contains(id) ||
                (user.LastName + " " + user.FirstName).Contains(id));

            var groups = db.Groups.Where(group =>
                group.Name.Contains(id));

            var posts = db.Posts.Where(post =>
                post.Title.Contains(id) ||
                post.Content.Contains(id));

            ViewBag.Users = users;
            ViewBag.Groups = groups;
            ViewBag.Posts = posts;

            return View();
        }
    }
}