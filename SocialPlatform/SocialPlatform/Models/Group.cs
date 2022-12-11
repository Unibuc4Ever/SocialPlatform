using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
// "Macros"
using AppUser = SocialPlatform.Models.ApplicationUser;

namespace SocialPlatform.Models
{
    public class Group
    {
        [Key]
        public int ID { get; set; }
        
        public string name { get; set; }
        public string about { get; set; }

        public virtual AppUser owner { get; }
        public virtual ICollection<AppUser> members { get; set; }
    }
}