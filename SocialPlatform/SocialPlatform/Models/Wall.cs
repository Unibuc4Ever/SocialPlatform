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
        [ForeignKey("owner")]
        public string ID { get; set; }

        public WType type { get; set; }
        public Color backgroundColor { get; set; }

        
        //[Required]
        //[ForeignKey("owner")]
        //public string owner_ID { get; set; }

        public virtual ApplicationUser owner { get; set; }
    }
}