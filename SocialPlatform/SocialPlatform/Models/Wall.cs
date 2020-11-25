using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;

namespace SocialPlatform.Models
{
    public class Wall
    {
        // Wall-ul este pentru un utilizator, sau o comunitate? (Poate vrem si business)
        // Este read only deoarece un wall de un tip, nu se poate transforma in altul
        public enum WType
        {
            UserW,
            CommunityW
        }

        [Key]
        [ForeignKey("ApplicationUser")]
        public string ID { get; set; }

        // Wall type
        [Required]
        public WType type { get; set; }

        // Color of the wall
        [Required]
        public Int32 colorArgb
        {
            get { return backgroundColor.ToArgb(); }
            set { backgroundColor = Color.FromArgb(value); }
        }

        [NotMapped]
        public Color backgroundColor { get; set; } = Color.Blue;

        
        // Owner of the wall

        // [Required]   /// De ce daca pun asta crapa la creearea Wall-ului ?
        // Pai daca depinde de primary key, clar exista mereu....
        public ApplicationUser ApplicationUser { get; set; }
    }
}