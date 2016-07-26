using ASP_Decisions.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP_Decisions.Controllers
{
    [Authorize(Roles = "admin")]
    public abstract class BaseController : Controller
    {
        // This is basically a wrapper class for all controllers
        // It should ensure that each controller which inherits from this
        // will have to be explicit about which actions are accessible to which users


        #region common controller fields
        protected ApplicationDbContext _db;
        protected UserStore<ApplicationUser> _userStore;
        protected UserManager<ApplicationUser> _userManager;
        #endregion


        #region properties
        protected ApplicationUser _applicationUser
        {
            get { return _userManager.FindById(User.Identity.GetUserId()); }
        }
        #endregion

        #region constructors
        public BaseController()
            : base()
        {
            _db = new ApplicationDbContext();
            _userStore = new UserStore<ApplicationUser>(_db);
            _userManager = new UserManager<ApplicationUser>(_userStore);
        }
        #endregion

        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
                _userManager.Dispose();
                _userStore.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}