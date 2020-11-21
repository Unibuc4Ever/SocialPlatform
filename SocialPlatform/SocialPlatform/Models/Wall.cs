using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int ID { get; set; }

        public WType type { get; set; }
        public Color backgroundColor { get; set; }
    }
}