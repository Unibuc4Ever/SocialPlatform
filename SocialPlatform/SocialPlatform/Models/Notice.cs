using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialPlatform.Models
{
    public class Notice
    {
        [Key]
        public int NoticeId { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Should have a content!")]
        public string Content { get; set; }

        // User the notice is meant to
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
