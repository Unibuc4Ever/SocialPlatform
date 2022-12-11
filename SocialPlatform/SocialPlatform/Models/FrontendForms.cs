using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialPlatform.Models
{
    public class FriendRequest
    {
        [Required(ErrorMessage = "Cu cine vrei sa fii priete? Pune ID")]
        public string otherID { get; set; }
    }

    public class PostForm
    {

        [Required(ErrorMessage = "Trebuie sa aveti un titlu!")]
        public string title { get; set; }

        [Required(ErrorMessage = "Trebuie sa aveti un continut!")]
        public string text { get; set; }
    }
}