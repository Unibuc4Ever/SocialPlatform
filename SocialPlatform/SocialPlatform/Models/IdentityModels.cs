using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialPlatform.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        // Wall-ul userului
        public int? WallId { get; set; }
        public virtual Wall Wall { get; set; }

        // Adaugam atribute specifice userilor
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        // Grupurile din care fac parte
        [InverseProperty("Members")]
        public virtual ICollection<Group> Groups { get; set; }

        // Catre cine am dat friend request, si neacceptate
        [InverseProperty("ReceivedFriendRequests")]
        public virtual ICollection<ApplicationUser> SentFriendRequests { get; set; }
        // Cine mi-a cerut mie prietenia, si nu am acceptat
        [InverseProperty("SentFriendRequests")]
        public virtual ICollection<ApplicationUser> ReceivedFriendRequests { get; set; }


        // Cine sunt prietenii mei
        [InverseProperty("FriendOf")]
        public virtual ICollection<ApplicationUser> Friends { get; set; }

        [InverseProperty("Friends")]
        public virtual ICollection<ApplicationUser> FriendOf { get; set; }



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext,
                SocialPlatform.Migrations.Configuration>("DefaultConnection"));
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Wall> Walls { get; set; }
        public DbSet<Group> Groups { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}