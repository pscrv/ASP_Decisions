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
using ASP_Decisions.Models.ViewModels;

namespace ASP_Decisions.Controllers
{
    public class CommentsController : BaseController
    {
        // GET: Comments
        public ActionResult Index()
        {
            return View(_db.Comments.ToList());
        }
        
        // GET: Pending
        public ActionResult Pending()
        {
            IEnumerable<Comment> pending = _db.Comments.ToList()
                .Where(c => !c.IsChecked);
            return View(pending);
        }

        // POST: Comments/Pending/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pending([Bind(Include = "Id")] Comment cmt, string SubmitButton)
        {
            if (cmt == null)
                return RedirectToAction("Pending");

            Comment comment = _db.Comments.FirstOrDefault(c => c.Id == cmt.Id);
            if (comment == null)
                return RedirectToAction("Pending");


            if (SubmitButton == "Approve")
            {
                comment.DatePublished = DateTime.Now;
                comment.IsAccepted = true;
              
            }
            else if (SubmitButton == "Reject")
            {
                comment.IsAccepted = false;
            }
            else
            {
                // eh?
                throw new ApplicationException("The request did not come from one of the Accept of Reject buttons.");
            }

            comment.IsChecked = true;
            _db.Entry(comment).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Pending");
        }



        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = _db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/AddComment
        [OverrideAuthorization]
        [Authorize]
        public ActionResult AddComment(string caseNumber)
        {
            Decision decision = _db.Decisions.FirstOrDefault(x => x.CaseNumber == caseNumber);

            AddCommentViewModel acvm = new AddCommentViewModel
            {
                Id = 0,
                Author = _applicationUser.UserName,
                CaseNumber = decision.CaseNumber,
                Text = "",
                DateSubmitted = DateTime.Now
            };
            return View(acvm);
        }

        // POST: Comments/AddComment
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OverrideAuthorization]
        [Authorize]
        public ActionResult AddComment([Bind(Include = "Id, Text, Author, CaseNumber, DateSubmitted")] AddCommentViewModel acvm, string SubmitButton)
        {
            if (SubmitButton == "Save")
            {
                if (ModelState.IsValid)
                {
                    Comment comment = new Comment
                    {
                        Text = acvm.Text,
                        Author = _applicationUser,
                        CaseNumber = acvm.CaseNumber,
                        DateSubmitted = acvm.DateSubmitted,
                        DatePublished = null,
                        IsAccepted = false,
                        IsChecked = false
                    };

                    _db.Comments.Add(comment);
                    _db.SaveChanges();
                }
            }
            else if (SubmitButton == "Cancel")
            {
                // Nothing to do
            }
            else
            {
                // How did we get here?
                throw new ApplicationException("The AddComment form was not submitted by the Add or Cancel button");
            }

            // go the the details view of the decision
            // assuming there is one (homepage, if not)
            Decision decision = _db.Decisions.FirstOrDefault(x => x.CaseNumber == acvm.CaseNumber);
            if (decision == null)
                return RedirectToAction("Index", "Home");

            return RedirectToAction("Details", "Decision", new { id = decision.Id });
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = _db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }                        

            AddCommentViewModel acvm = new AddCommentViewModel
            {
                Id = (int)id,
                Author = comment.Author.UserName,
                CaseNumber = comment.CaseNumber,
                Text = comment.Text,
                DateSubmitted = comment.DateSubmitted                
            };

            return View(acvm);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, Text, Author, CaseNumber, DateSubmitted")] AddCommentViewModel acvm, string SubmitButton)
        {
            if (SubmitButton == "Save")
            {
                if (ModelState.IsValid)
                {
                    // Only the text should have been changed
                    // but we need to update the flags
                    Comment comment = _db.Comments.FirstOrDefault(x => x.Id == acvm.Id);
                    comment.Text = acvm.Text;                  
                    comment.DatePublished = null;
                    comment.IsAccepted = false;
                    comment.IsChecked = false;

                    _db.Entry(comment).State = EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            else if (SubmitButton == "Cancel")
            {
                // Nothing to do
            }
            else
            {
                // How did we get here?
                throw new ApplicationException("The AddComment form was not submitted by the Add or Cancel button");
            }

            // go the the details view of the decision
            // assuming there is one (homepage, if not)
            Decision decision = _db.Decisions.FirstOrDefault(x => x.CaseNumber == acvm.CaseNumber);
            if (decision == null)
                return RedirectToAction("Index", "Home");

            return RedirectToAction("Details", "Decision", new { id = decision.Id });
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = _db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = _db.Comments.Find(id);
            _db.Comments.Remove(comment);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region helper methods
        #endregion


        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        #endregion
    }
}
