using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASP_Decisions.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using ASP_Decisions.Models.UserViewModels;

namespace ASP_Decisions.Controllers
{
    public class ApplicationUsersController : BaseController
    {
        #region private
        private ApplicationDbContext db;
        private UserStore<ApplicationUser> userStore;
        private RoleStore<ApplicationRole> roleStore;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<ApplicationRole> roleManager;
        #endregion

        #region constructors
        public ApplicationUsersController()
            : base()
        {
            db = new ApplicationDbContext();
            userStore = new UserStore<ApplicationUser>(db);
            roleStore = new RoleStore<ApplicationRole>(db);
            userManager = new UserManager<ApplicationUser>(userStore);
            roleManager = new RoleManager<ApplicationRole>(roleStore);
        }
        #endregion

        #region actions
        // GET: ApplicationUsers
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: ApplicationUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            ViewBag.Roles = userManager.GetRoles(id).ToList();

            return View(applicationUser);
        }

         // GET: ApplicationUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            string role = userManager.GetRoles(applicationUser.Id).First();
            SelectList possibleRoles = new SelectList(roleManager.Roles.Select(x => x.Name).ToList(), role);
            EditUserViewModel evm = new EditUserViewModel
            {
                Id                  = applicationUser.Id,
                UserName            = applicationUser.UserName,
                Email               = applicationUser.Email,
                PhoneNumber         = applicationUser.PhoneNumber,
                TwoFactorEnabled    = applicationUser.TwoFactorEnabled,
                LockoutEnabled      = applicationUser.LockoutEnabled,
                Role                = role,
                PossibleRoles       = possibleRoles
            };
            
            return View(evm);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, UserName, Role, Email, PhoneNumber, TwoFactorEnabled, LockoutEnabled")] EditUserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = db.Users.Find(userVM.Id);
                user.UserName = userVM.UserName;
                user.Email = userVM.Email;
                user.PhoneNumber = userVM.PhoneNumber;
                user.TwoFactorEnabled = userVM.TwoFactorEnabled;
                user.LockoutEnabled = userVM.LockoutEnabled;


                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new UserManager<ApplicationUser>(userStore);
                userManager.RemoveFromRoles(user.Id, userManager.GetRoles(user.Id).ToArray());
                userManager.AddToRole(user.Id, userVM.Role);
                

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = user.Id });
            }
            return View(userVM);
        }

        // GET: ApplicationUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                userManager.Dispose();
                roleManager.Dispose();
                userStore.Dispose();
                roleStore.Dispose();

            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
