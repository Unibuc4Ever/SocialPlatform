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
    public class PostsController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new
             UserStore<ApplicationUser>(db));

        // GET: Post
        [Authorize]
        public ActionResult Index()
        {
            // trebuie sa returneam numai posturile
            // din baza de date care sunt pe un wall
            // la care avem acces (suntem prieteni).
            ViewBag.Posts = db.Posts;
            return View();
        }

        // HttpGet
        [Authorize]
        public ActionResult New()
        {
            Post post = new Post();
            return View(post);
        }

        [HttpPost]
        [Authorize]
        public ActionResult New(Post post)
        {
            post.createdAt = DateTime.Now;
            post.author = db.Users.Find(User.Identity.GetUserId());
            post.wall = post.author.wall;

            try
            {
                if (ModelState.IsValid)
                {
                    db.Posts.Add(post);
                    db.SaveChanges();
                    TempData["message"] = "Postul a fost adaugat!";

                    return RedirectToAction("Index");
                }
                else
                    return View(post);
            }
            catch (Exception)
            {
                return View(post);
            }
        }

        [Authorize]
        public ActionResult Show(int id)
        {
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
            try {
                Post post = db.Posts.Find(id);
                if (post.author.Id != User.Identity.GetUserId())
                    throw new Exception();

                return View(post);
            } catch (Exception)
			{
                throw new HttpException(403, "Post does not exist or you don't have the required permissons!");
            }   
        }

        [HttpPut]
        [Authorize]
        public ActionResult Edit(Post new_post)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Post old_post = db.Posts.Find(new_post.ID);

                    if (TryUpdateModel(old_post))
                    {
                        if (old_post.author.Id != User.Identity.GetUserId())
                            throw new Exception();
                        old_post.text = new_post.text;
                        old_post.title = new_post.title;

                        db.SaveChanges();
                        TempData["message"] = "Articolul a fost modificat!";
                        return RedirectToAction("Index");
                    }
                }
                return View(new_post);
            }
            catch (Exception)
            {
                return View(new_post);
            }
        }

        [HttpDelete]
        [Authorize]
        public ActionResult Delete(int id)
        {
            try {
                Post post = db.Posts.Find(id);
                if (post.author.Id != User.Identity.GetUserId())
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
