using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SocialPlatform
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Posts controller

            routes.MapRoute(
                name: "NewPost",
                url: "Posts/New",
                defaults: new { controller = "Posts", action = "New" }
            );

            routes.MapRoute(
                name: "ShowPost",
                url: "Posts/Show/{id}",
                defaults: new { controller = "Posts", action = "Show" }
            );

            routes.MapRoute(
                name: "EditPost",
                url: "Posts/Edit/{id}",
                defaults: new { controller = "Posts", action = "Edit" }
            );

            routes.MapRoute(
                name: "DeletePost",
                url: "Posts/Delete/{id}",
                defaults: new { controller = "Posts", action = "Delete" }
            );

            // Comments controller
            routes.MapRoute(
                name: "NewComment",
                url: "Comments/New/{PostID}",
                defaults: new { controller = "Comments", action = "New" }
            );

            routes.MapRoute(
                name: "EditComment",
                url: "Comments/Edit/{id}",
                defaults: new { controller = "Comments", action = "Edit" }
            );

            routes.MapRoute(
                name: "DeleteComment",
                url: "Comments/Delete/{id}",
                defaults: new { controller = "Comments", action = "Delete" }
            );

            // Users controller
            routes.MapRoute(
                name: "AddFriendRequest",
                url: "Users/FriendRequest/New/{otherID}",
                defaults: new { controller = "Users", 
                                action = "AddFriendRequest",
                                otherID = UrlParameter.Optional}
            );

            routes.MapRoute(
                name: "SentFriendRequests",
                url: "Users/FriendRequest/Sent",
                defaults: new { controller = "Users", action = "SentFriendRequests" }
            );

            routes.MapRoute(
                name: "ReceivedFriendRequests",
                url: "Users/FriendRequest/Received",
                defaults: new { controller = "Users", action = "ReceivedFriendRequests" }
            );

            routes.MapRoute(
                name: "AcceptFriendRequest",
                url: "Users/FriendRequest/Accept/{otherID}",
                defaults: new { controller = "Users", action = "AcceptFriendRequest" }
            );

            routes.MapRoute(
                name: "DeclineFriendRequest",
                url: "Users/FriendRequest/Decline/{otherID}",
                defaults: new { controller = "Users", action = "DeclineFriendRequest" }
            );

            routes.MapRoute(
                name: "CancelFriendRequest",
                url: "Users/FriendRequest/Cancel/{otherID}",
                defaults: new { controller = "Users", action = "CancelFriendRequest" }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
