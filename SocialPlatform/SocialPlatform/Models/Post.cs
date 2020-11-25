using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialPlatform.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Trebuie sa aveti un titlu!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Trebuie sa aveti un continut!")]
        public string Content { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}