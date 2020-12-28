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
        public ActionResult Index(string query)
        {
            var db = new ApplicationDbContext();
            

            return View();
        }

        // GET: Search
        // Search the word id.
        // If id is null, then redirect to default
        public ActionResult Old (string query)
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

        public ActionResult PagedGroups(string query, int cnt, int lastWallID)
        {
            var db = new ApplicationDbContext();
            //db.Configuration.LazyLoadingEnabled = false;
            //db.Configuration.ProxyCreationEnabled = false;

            var groups = db.Groups.Where(group =>
                group.Name.Contains(query)).ToList();

            groups.Sort(delegate (Group a, Group b) {
                return (a.GroupId < b.GroupId) ? -1 : 1;
            });

            // If not found, returns -1, but we want to anyway not return the old
            // Element, so we add 1
            var index = groups.FindIndex(gr => gr.WallId == lastWallID) + 1;
            cnt = Math.Min(cnt, groups.Count() - index);

            var selectedGroups = groups.GetRange(index, cnt);

            int nextWallID = -1;
            if (selectedGroups.Count() > 0)
                nextWallID = selectedGroups.Last().WallId;

            var transformedGroups = new List<dynamic>();
            selectedGroups.ForEach(delegate (Group gr) {
                transformedGroups.Add( new {
                    Name = gr.Name,
                    GroupID = gr.GroupId,
                    WallID = gr.WallId,
                    MemberCount = gr.Members.Count()
                });
            });

            return Json(
                new { 
                    groups = transformedGroups,
                    lastWallID = nextWallID
                }, 
                JsonRequestBehavior.AllowGet
            );
        }

        public ActionResult PagedUsers(string query, int cnt, int lastWallID)
        {
            var db = new ApplicationDbContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            var users = db.Users.Where(user =>
                (user.FirstName + " " + user.LastName).Contains(query) ||
                (user.LastName + " " + user.FirstName).Contains(query)).ToList();

            users.Sort(delegate (ApplicationUser a, ApplicationUser b)
            {
                return (a.WallId < b.WallId) ? -1 : 1;
            });

            // If not found, returns -1, but we want to anyway not return the old
            // Element, so we add 1
            int index = users.FindIndex(usr => usr.WallId == lastWallID) + 1;
            cnt = Math.Min(cnt, users.Count() - index);

            return Json(
                new { 
                    users = users.GetRange(index, cnt) 
                },
                JsonRequestBehavior.AllowGet
            );
        }

        public ActionResult PagedPosts(string query, int cnt, int lastWallID)
        {
            var db = new ApplicationDbContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            var posts = db.Posts.Where(post =>
                post.Title.Contains(query) ||
                post.Content.Contains(query)).ToList();

            posts.Sort(delegate (Post a, Post b)
            {
                return (a.WallId < b.WallId) ? -1 : 1;
            });

            // If not found, returns -1, but we want to anyway not return the old
            // Element, so we add 1
            int index = posts.FindIndex(usr => usr.WallId == lastWallID) + 1;
            cnt = Math.Min(cnt, posts.Count() - index);

            return Json(
                new { 
                    users = posts.GetRange(index, cnt) 
                },
                JsonRequestBehavior.AllowGet
            );
           
        }
    }
}