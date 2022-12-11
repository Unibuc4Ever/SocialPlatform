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
        public ActionResult Index(string query)
        {
            var db = new ApplicationDbContext();
            if (query == null)
                return RedirectToRoute("/");

            query = query.Trim();

            if (query.Length == 0)
                return RedirectToRoute("/");

            var users = db.Users.Where(user =>
                (user.FirstName + " " + user.LastName).Contains(query) ||
                (user.LastName + " " + user.FirstName).Contains(query));

            var groups = db.Groups.Where(group =>
                group.Name.Contains(query));

            var posts = db.Posts.Where(post =>
                post.Title.Contains(query) ||
                post.Content.Contains(query));

            ViewBag.Users = users;
            ViewBag.Groups = groups;
            ViewBag.Posts = posts;

            return View();
        }
    }
}