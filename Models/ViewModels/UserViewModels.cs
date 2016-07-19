using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace ASP_Decisions.Models.UserViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
        
        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

        [Display(Name =("Possible roles"))]
        public SelectList PossibleRoles { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Two factor login?")]
        public bool TwoFactorEnabled { get; set; }

        [Display(Name = "Lockout possible?")]
        public bool LockoutEnabled { get; set; }

        #region dbcontext
        private static ApplicationDbContext __context = new ApplicationDbContext();
        private static UserStore<ApplicationUser> __userStore = new UserStore<ApplicationUser>(__context);
        private static RoleStore<ApplicationRole> __roleStore = new RoleStore<ApplicationRole>(__context);
        private static UserManager<ApplicationUser> __userManager = new UserManager<ApplicationUser>(__userStore);
        private static RoleManager<ApplicationRole> __roleManager = new RoleManager<ApplicationRole>(__roleStore);
        #endregion

        #region constructors
        #endregion



    }



}