using Microsoft.AspNet.Identity;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using SocialPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
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
                my_friends.Where(fr => fr.WallId == post.WallId).Count() > 0);

            return View(posts.ToList());
        }

        // Returns all the posts
        // TODO: Return only public posts.
        [Authorize]
        public ActionResult Explore()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            // trebuie sa returneam numai posturile
            // din baza de date care sunt pe un wall
            // la care avem acces (suntem prieteni).
            return View(db.Posts.ToList());
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

        [Authorize]
        public ActionResult Show(int id)
        {
            var db = new ApplicationDbContext();
            try
            {
                Post post = db.Posts.Find(id);
                return View(post);
            } catch (Exception)
			{
                throw new HttpException(404, "Post does not exist!");
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
                if (post.User.Id != User.Identity.GetUserId())
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
