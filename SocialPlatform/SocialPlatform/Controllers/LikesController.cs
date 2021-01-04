using Microsoft.AspNet.Identity;
using SocialPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialPlatform.Controllers
{
    public class LikesController : Controller
    {
        // GET: Likes
        [Authorize]
        [HttpPost]
        public void New(Like like)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            // To verif that you can like
            try {
                var user = db.Users.Find(User.Identity.GetUserId());
                like.User = user;

                if (ModelState.IsValid)
                {
                    if (user.Likes.Count(l => (l.PostId != null && l.PostId == like.PostId) ||
                                              (l.CommentId != null && l.CommentId == like.LikeId)) == 1)
                        throw new Exception();

                    db.Likes.Add(like);
                    db.SaveChanges();
                    //return 
                }
            } catch (Exception) { }
        }

        [Authorize]
        [HttpPost]
        public void Delete(Like like)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                like = db.Likes.Find(like.LikeId);
                var user = db.Users.Find(User.Identity.GetUserId());

                if (user.Id != like.UserId)
                    throw new Exception();

                db.Likes.Remove(like);
                db.SaveChanges();
            }
            catch (Exception) { }
        }
    }
}