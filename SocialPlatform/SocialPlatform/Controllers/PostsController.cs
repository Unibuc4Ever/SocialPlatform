using Microsoft.AspNet.Identity;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using SocialPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace SocialPlatform.Controllers
{
    public class PostsController : Controller
    {
        // Returns the list of posts visible which are:
        // 1. Made by me
        // 2. Made by a friend
        // 3. Posted on my wall
        // 4. Posted on a friend's wall
        // 5. Posted in a group I am part of
        // GET: Post
        [Authorize]
        public ActionResult Index(int? frommaybe)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            int from = frommaybe ?? 0;

            try
            {
                // trebuie sa returneam numai posturile
                // din baza de date care sunt pe un wall
                // la care avem acces (suntem prieteni).
                var user = db.Users.Find(User.Identity.GetUserId());
                var my_groups = user.Groups.ToList();
                var my_friends = user.Friends.ToList();

                var posts = db.Posts.ToList().Where(post =>
                    post.WallId == user.WallId ||
                    post.UserId == user.Id ||
                    my_friends.Any(fr => fr.Id == post.UserId) ||
                    my_groups.Where(gr => gr.WallId == post.WallId).Count() > 0 ||
                    my_friends.Where(fr => fr.WallId == post.WallId).Count() > 0).OrderByDescending(post => post.CreatedAt);

                if (from < 0 || from > posts.Count())
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);

                Post post_ret = null;
                if (from != 0)
                    post_ret = posts.ElementAt(from - 1);

                return View(post_ret);
            }
            catch(Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
        }

        // No authorize => public for not logged in users.
        // Returns all the posts
        public ActionResult Explore(int? frommaybe)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            int from = frommaybe ?? 0;
            var me = db.Users.Find(User.Identity.GetUserId());

            try
            {
                // trebuie sa returnam posturile publice
                var posts = db.Posts.Where(delegate (Post post) {
                    var users = db.Users.Where(usr => usr.WallId == post.WallId);
                    // Posted on a group -> free to access.
                    if (users.Count() == 0)
                        return true;
                    // User detaining the wall it was posted on.
                    var user = users.First();

                    // we are friends
                    if (user.Friends.Count(usr => usr.Id == User.Identity.GetUserId()) == 1)
                        return true;

                    // my own post
                    if (me != null && me.WallId == post.WallId)
                        return true;

                    // It is public
                    return user.WallIsVisible;
                }).ToList().OrderByDescending(post => post.CreatedAt);

                if (from < 0 || from > posts.Count())
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);

                Post post_ret = null;
                if (from != 0)
                    post_ret = posts.ElementAt(from - 1);

                return View(post_ret);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
        }

        // HttpGet
        [Authorize]
        public ActionResult New(int WallId)
        {
            Post post = new Post();
            post.WallId = WallId;

            return View(post);
        }

        [HttpPost]
        [Authorize]
        public ActionResult New(Post post)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var user = db.Users.Find(User.Identity.GetUserId());
            post.UserId = user.Id;
            post.CreatedAt = DateTime.Now;
			post.Comments = new HashSet<Comment>();
         
            try
            {
                post.Wall = db.Walls.Find(post.WallId);
                if (ModelState.IsValid)
                {
                    db.Posts.Add(post);
                    db.SaveChanges();
                    post.Wall.Posts.Add(post);

                    db.SaveChanges();

                    TempData["message"] = "Postul a fost adaugat!";

                    return RedirectToAction("Show", new { id = post.PostId });
                }
            }
            catch (Exception e) {
                var a = e.Message;
            }
            return View(post);
        }

        // Frommaybe is used for the comments
        // It indicated the nr of the comment (0 -> none, 1 -> first, 2 -> second etc)
        [Authorize]
        public ActionResult Show(int id, int? frommaybe)
        {
            var db = new ApplicationDbContext();
            int from = frommaybe ?? 0;
            
            try
            {
                Post post = db.Posts.Find(id);
                
                if (post.Comments.Count() < from || from < 0)
                    throw new Exception();

                ViewBag.CommentNr = from;
                return View(post);
            } catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            } 
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try {
                Post post = db.Posts.Find(id);
                if (post.User.Id != User.Identity.GetUserId())
                    throw new Exception();

                return View(post);
            } catch (Exception)
			{
                throw new HttpException(403, "Post does not exist or you don't have the required permissons!");
            }   
        }

        [HttpPut]
        [Authorize]
        public ActionResult Edit(int id, Post new_post)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                if (ModelState.IsValid)
                {
                    Post old_post = db.Posts.Find(id);

                    if (old_post.User.Id != User.Identity.GetUserId())
                        throw new Exception();
                    old_post.Content = new_post.Content;
                    old_post.Title = new_post.Title;

                    db.SaveChanges();
                    TempData["message"] = "Articolul a fost modificat!";
                    return RedirectToAction("Show", new { id = new_post.PostId });
                }
                else
                    return View(new_post);
            }
            catch (Exception) { }
            return View(new_post);
        }

        [HttpDelete]
        [Authorize]
        public ActionResult Delete(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try {
                Post post = db.Posts.Find(id);
                if (post.User.Id != User.Identity.GetUserId() && !User.IsInRole("Administrator"))
                    throw new Exception();
                db.Likes.RemoveRange(db.Likes.Where(like => like.PostId == post.PostId ||
                                                    (like.Comment != null && like.Comment.PostId == post.PostId)));
                db.Comments.RemoveRange(db.Comments.Where(comm => comm.PostId == post.PostId));
                db.Posts.Remove(post);
                db.SaveChanges();
                TempData["message"] = "Articolul a fost sters!";
                return RedirectToAction("Index");
            } catch (Exception e)
			{
                throw new HttpException(403, "Post does not exist or you don't have the required permissons!");
            }
        }

    }
}
