using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialPlatform.Models
{
    public class SocialWorker
    {
        public void DeleteComment(int comment_id, ref ApplicationDbContext db)
        {
            foreach (var like in db.Comments.Find(comment_id).Likes.ToList())
                db.Likes.Remove(like);
            db.Comments.Remove(db.Comments.Find(comment_id));
            db.SaveChanges();
        }

        public void DeletePost(int post_id, ref ApplicationDbContext db)
        {
            var post = db.Posts.Find(post_id);
            foreach (var comment in post.Comments.ToList())
                DeleteComment(comment.CommentId, ref db);

            foreach (var like in post.Likes.ToList())
                db.Likes.Remove(like);

            db.Posts.Remove(post);
            db.SaveChanges();
        }

        public void DeleteWall(int wall_id, ref ApplicationDbContext db)
        {
            var wall = db.Walls.Find(wall_id);

            foreach (var post in wall.Posts.ToList())
                DeletePost(post.PostId, ref db);

            db.Walls.Remove(wall);
            db.SaveChanges();
        }

        public void DeleteGroup(int group_id, ref ApplicationDbContext db)
        {
            var group = db.Groups.Find(group_id);
            DeleteWall(group.WallId, ref db);
            // No need to delete group, was deleted from wall
            db.SaveChanges();
        }

        public void DeleteUser(string user_id, ref ApplicationDbContext db)
        {
            var user = db.Users.Find(user_id);

            var user_groups = db.Groups.Where(group => group.UserId == user_id).ToList();

            foreach (var group in user_groups.ToList())
                DeleteGroup(group.GroupId, ref db);

            foreach (var post in user.Posts.ToList())
                DeletePost(post.PostId, ref db);

            if (user.WallId != null)
                DeleteWall(user.WallId ?? 0, ref db);

            user.Friends.Clear();
            user.ReceivedFriendRequests.Clear();
            user.SentFriendRequests.Clear();
            user.FriendOf.Clear();

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            UserManager.Delete(user);
            db.SaveChanges();
        }
    }
}