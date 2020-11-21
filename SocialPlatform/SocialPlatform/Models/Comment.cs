using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialPlatform.Models
{
    public class Comment
    {
        [Key]
        public int ID { get; set; }
        public DateTime createdAt { get; set; }

        public virtual ApplicationUser user { get; set; }
        public string text { get; set; }

        // Foreign key catre Post-ul in care este Comment-ul
        public virtual Post Post { get; }
    }
}