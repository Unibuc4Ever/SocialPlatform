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
        [Key]
        public int ID { get; set; }

        [Required]
        public DateTime createdAt { get; set; }

        [Required(ErrorMessage = "Comentul nu poate fii gol")]
        public string text { get; set; }

        [Required]
        public virtual ApplicationUser author { get; set; }
        

        // Foreign key catre Post-ul in care este Comment-ul
        [Required]
        public virtual Post post { get; set; }
    }
}