using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialPlatform.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Comentul nu poate fii gol!")]
        public string Content { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}