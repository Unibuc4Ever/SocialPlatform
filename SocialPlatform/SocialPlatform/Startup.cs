using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using SocialPlatform.Models;
using System.Collections.Generic;

[assembly: OwinStartupAttribute(typeof(SocialPlatform.Startup))]
namespace SocialPlatform
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // Se apeleaza o metoda in care se adauga contul de administrator si rolurile aplicatiei
            createAdminUserAndApplicationRoles();
        }
        private void createAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Se adauga rolurile aplicatiei
            if (!roleManager.RoleExists("Administrator"))
            {
                // Se adauga rolul de administrator
                var role = new IdentityRole();
                role.Name = "Administrator";
                roleManager.Create(role);

                // se adauga utilizatorul administrator
                var user = new ApplicationUser();
                user.UserName = "admin@admin.com";
                user.Email = "admin@admin.com";
                var adminCreated = UserManager.Create(user, "111111");
                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Administrator");

                    Wall userWall = new Wall();
                    userWall.Posts = new HashSet<Post>();
                    context.Walls.Add(userWall);
                    context.SaveChanges();

                    user.Wall = userWall;
                    context.SaveChanges();
                    UserManager.Update(user);
                }
            }
            if (!roleManager.RoleExists("Editor"))
            {
                var role = new IdentityRole();
                role.Name = "Editor";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
        }
    }
}
