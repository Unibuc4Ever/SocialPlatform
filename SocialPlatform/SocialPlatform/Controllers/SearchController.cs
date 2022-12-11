using SocialPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SocialPlatform.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult Posts(string query, int? frommaybe)
        {
            var db = new ApplicationDbContext();
            int from = frommaybe ?? 0;

            if (query == null || query == "")
                return Redirect("/");
            query = query.Trim();

            try
            {
                var posts = db.Posts.Where(post =>
                    post.Title.Contains(query) ||
                    post.Content.Contains(query)).OrderByDescending(post => post.CreatedAt);

                if (from < 0 || from > posts.Count())
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);

                Post post_ret = null;
                if (from != 0)
                    post_ret = posts.ToList().ElementAt(from - 1);

                ViewBag.Query = query;
                return View(post_ret);
            }
            catch (Exception e)
            {
                return Redirect("/");
            }
        }

        public ActionResult Groups(string query, int? frommaybe)
        {
            var db = new ApplicationDbContext();
            int from = frommaybe ?? 0;

            if (query == null || query == "")
                return Redirect("/");
            query = query.Trim();

            try
            {
                var groups = db.Groups.Where(group =>
                    group.Name.Contains(query)).OrderByDescending(gr => gr.Members.Count());

                if (from < 0 || from > groups.Count())
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);

                Group group_ret = null;
                if (from != 0)
                    group_ret = groups.ToList().ElementAt(from - 1);

                ViewBag.Query = query;
                return View(group_ret);
            }
            catch (Exception e)
            {
                return Redirect("/");
            }
        }
        public ActionResult Users(string query, int? frommaybe)
        {
            var db = new ApplicationDbContext();
            int from = frommaybe ?? 0;

            if (query == null || query == "")
                return Redirect("/");
            query = query.Trim();

            try
            {
                var users = db.Users.Where(user =>
                    (user.FirstName + " " + user.LastName).Contains(query) ||
                    (user.LastName + " " + user.FirstName).Contains(query)).OrderByDescending(usr => usr.WallId);


                if (from < 0 || from > users.ToList().Count())
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);

                ApplicationUser user_ret = null;
                if (from != 0)
                    user_ret = users.ToList().ElementAt(from - 1);

                ViewBag.Query = query;
                return View(user_ret);
            }
            catch (Exception e)
            {
                return Redirect("/");
            }
        }
    }
}
