using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialPlatform.Annotations;

namespace SocialPlatform.Models
{
    [ExactlyOneProp("PostId", "CommentId", ErrorMessage = "Like has to be for either Post or Comment")]
    public class Like
    {
        public int LikeId { get; set; }

        public int? PostId { get; set; }
        virtual public Post Post { get; set; }

        public int? CommentId { get; set; }
        virtual public Comment Comment { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    
        public string Type { get; set;  }
    }
}