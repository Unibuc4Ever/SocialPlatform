using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialPlatform.Models
{
	public class Group
	{
		public int GroupId { get; set; }

		// Owner of the group
		public string UserId { get; set; }
		public virtual ApplicationUser User { get; set; }

		// Members
		[InverseProperty("Groups")]
		public virtual ICollection<ApplicationUser> Members { get; set; }

		[Required(ErrorMessage = "Introduceti numele!")]
		public string Name { get; set; }

		// Wall
		public int WallId { get; set; }
		public virtual Wall Wall { get; set; }
	}
}