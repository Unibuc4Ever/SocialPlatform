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
        [HttpPost]
        [Authorize]
        public ActionResult New(int PostID, Comment comment)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                if (ModelState.IsValid)
                {
                    Post post = db.Posts.Find(PostID);
                    if (post == null)
                        throw new Exception();

                    var user = db.Users.Find(User.Identity.GetUserId());
                    comment.User = user;

                    comment.CreatedAt = DateTime.Now;
                    comment.PostId = PostID;

                    db.Comments.Add(comment);
                    db.SaveChanges();
                    TempData["message"] = "Commentul a fost adaugat!";
                }
                else
                    RedirectToAction("Show", "Posts", new { id = PostID });
            }
            catch (Exception) { } 
            return RedirectToAction("Show", "Posts", new { id = PostID });
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
			{
                Comment comm = db.Comments.Find(id);
                if (comm.UserId != User.Identity.GetUserId())
                    throw new Exception();

                return View(comm);
			}
            catch(Exception)
			{
                return RedirectToAction("Show", "Posts", new { id = id });
            }
		}

        [HttpPut]
        [Authorize]
        public ActionResult Edit(Comment comm)
		{
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                if (ModelState.IsValid)
                {
                    Comment old_comm = db.Comments.Find(comm.CommentId);
                    if (old_comm.UserId != User.Identity.GetUserId())
                        throw new Exception();

                    old_comm.Content = comm.Content;
                    db.SaveChanges();

                    return RedirectToAction("Show", "Posts", new { id = old_comm.PostId });
                }
                else
                    return View(comm);
            }
            catch (Exception)
            {
                return View(comm);
            }
        }

        [HttpDelete]
        [Authorize]
        public ActionResult Delete(int id)
		{
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                Comment comm = db.Comments.Find(id);
                if (comm.UserId != User.Identity.GetUserId())
                    throw new Exception();

                int PostId = comm.PostId;
                db.Comments.Remove(comm);
                db.SaveChanges();

                return RedirectToAction("Show", "Posts", new { id = PostId });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Posts");
            }
        }
    }
}
