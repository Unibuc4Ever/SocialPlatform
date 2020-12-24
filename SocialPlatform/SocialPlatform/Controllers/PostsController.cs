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
        // GET: Post
        [Authorize]
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            // trebuie sa returneam numai posturile
            // din baza de date care sunt pe un wall
            // la care avem acces (suntem prieteni).
            ViewBag.Posts = db.Posts;
            return View(db.Users.Find(User.Identity.GetUserId()));
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
                db.Posts.Remove(post);
                db.SaveChanges();
                TempData["message"] = "Articolul a fost sters!";
                return RedirectToAction("Index");
            } catch (Exception)
			{
                throw new HttpException(403, "Post does not exist or you don't have the required permissons!");
            }
        }

    }
}
