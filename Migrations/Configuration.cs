namespace ASP_Decisions.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ASP_Decisions.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ASP_Decisions.Models.ApplicationDbContext context)
        {
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //{
            //    System.Diagnostics.Debugger.Launch();
            //}

            var roleStore = new RoleStore<IdentityRole>(context);
            var userStore = new UserStore<ApplicationUser>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (roleManager.FindByName("admin") == null)
                roleManager.Create(new ApplicationRole("admin", ""));
            if (roleManager.FindByName("guest") == null)
                roleManager.Create(new ApplicationRole("guest", ""));
            if (roleManager.FindByName("member") == null)
                roleManager.Create(new ApplicationRole("member", ""));
            

            if (userManager.FindByName("pscrv") == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = "pscrv",
                    Email = "scrv.pscr@googlemail.com"
                };
                userManager.Create(user, "aLLiance");
                userManager.AddToRole(user.Id, "admin");
            }
            if (userManager.FindByName("someone") == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = "someone",
                    Email = "some.one@some.whr"
                };
                userManager.Create(user, "aLLiance");
                userManager.AddToRole(user.Id, "guest");
            }
            if (userManager.FindByName("somemember") == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = "somemember",
                    Email = "some.member@some.whr"
                };
                userManager.Create(user, "aLLiance");
                userManager.AddToRole(user.Id, "member");
            }
        }
    }
}
