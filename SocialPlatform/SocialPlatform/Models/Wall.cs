using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialPlatform.Models
{
	public class Wall
	{
		public int WallId { get; set; }

		// Posts written on the wall
		public virtual ICollection<Post> Posts { get; set; }
	}
}