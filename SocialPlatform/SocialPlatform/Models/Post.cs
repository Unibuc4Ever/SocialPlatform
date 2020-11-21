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
        
        public DateTime createdAt { get; set; }

        // Should  be foreign key? Check
        public virtual ApplicationUser author { get; set; }

        [Required(ErrorMessage = "Trebuie sa aveti un titlu!")]
        public string title { get; set; }
        
        [Required(ErrorMessage = "Trebuie sa aveti un continut!")]
        public string text { get; set; }

        // Foreign key catre wall-ul in care este Post-ul
        public virtual Wall wall { get; set; }
    }
}