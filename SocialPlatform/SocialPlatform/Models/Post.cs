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
        public int ID { get; set; }

        [Required]
        public DateTime createdAt { get; set; }

        [Required(ErrorMessage = "Trebuie sa aveti un titlu!")]
        public string title { get; set; }

        [Required(ErrorMessage = "Trebuie sa aveti un continut!")]
        public string text { get; set; }
        
        // Not required as if the user gets deleted, then Required wall gets deleted
        //[Required]
        public virtual ApplicationUser author { get; set; }

        [Required] // Eu vreau acest required
        public string wall_ID { get; set; }
        
        [ForeignKey("wall_ID")]
        public virtual Wall wall { get; set; }

        [InverseProperty("post")]
        public virtual ICollection<Comment> comments { get; set; }
    }
}