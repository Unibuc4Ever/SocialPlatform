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
}