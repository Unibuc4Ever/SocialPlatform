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
        // Adaugam atribute specifice userilor
        public string firstName { get; set; }
        public string lastName { get; set; }

        [InverseProperty("owner")]
        public virtual Wall wall { get; set; }

        [InverseProperty("author")]
        public virtual ICollection<Comment> comments { get; set; }

        // Catre cine am dat friend request, si neacceptate
        [InverseProperty("receivedFriendRequests")]
        public virtual ICollection<ApplicationUser> sentFriendRequests { get; set; }
        // Cine mi-a cerut mie prietenia, si nu am acceptat
        [InverseProperty("sentFriendRequests")]
        public virtual ICollection<ApplicationUser> receivedFriendRequests { get; set; }


        // Cine sunt prietenii mei
        [InverseProperty("friendOf")]
        public virtual ICollection<ApplicationUser> friends { get; set; }

        [InverseProperty("friends")]
        public virtual ICollection<ApplicationUser> friendOf { get; set; }

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

        public DbSet<Wall> Walls { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}