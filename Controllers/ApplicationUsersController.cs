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
        private RoleStore<ApplicationRole> _roleStore;
        private RoleManager<ApplicationRole> _roleManager;
        #endregion

        #region constructors
        public ApplicationUsersController()
            : base()
        {
            _roleStore = new RoleStore<ApplicationRole>(_db);
            _roleManager = new RoleManager<ApplicationRole>(_roleStore);
        }
        #endregion

        #region actions
        // GET: ApplicationUsers
        public ActionResult Index()
        {
            return View(_db.Users.ToList());
        }

        // GET: ApplicationUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = _userManager.FindById(id);


            if (user == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Roles = _userManager.GetRoles(id).ToList();

            return View(user);
        }

         // GET: ApplicationUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (_applicationUser == null)
            {
                return HttpNotFound();
            }

            string role = _userManager.GetRoles(_applicationUser.Id).First();
            SelectList possibleRoles = new SelectList(_roleManager.Roles.Select(x => x.Name).ToList(), role);
            EditUserViewModel evm = new EditUserViewModel
            {
                Id                  = _applicationUser.Id,
                UserName            = _applicationUser.UserName,
                Email               = _applicationUser.Email,
                PhoneNumber         = _applicationUser.PhoneNumber,
                TwoFactorEnabled    = _applicationUser.TwoFactorEnabled,
                LockoutEnabled      = _applicationUser.LockoutEnabled,
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
                ApplicationUser user = _db.Users.Find(userVM.Id);
                user.UserName = userVM.UserName;
                user.Email = userVM.Email;
                user.PhoneNumber = userVM.PhoneNumber;
                user.TwoFactorEnabled = userVM.TwoFactorEnabled;
                user.LockoutEnabled = userVM.LockoutEnabled;
                
                _userManager.RemoveFromRoles(user.Id, _userManager.GetRoles(user.Id).ToArray());
                _userManager.AddToRole(user.Id, userVM.Role);
                

                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
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

            if (_applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(_applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = _db.Users.Find(id);
            _db.Users.Remove(applicationUser);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _roleManager.Dispose();
                _roleStore.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
