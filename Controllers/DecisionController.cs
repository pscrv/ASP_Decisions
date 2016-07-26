using ASP_Decisions.Epo_facade;
using ASP_Decisions.Models;
using ASP_Decisions.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ASP_Decisions.Controllers
{
    public class DecisionController : BaseController
    {
        // GET: Decisions
        public ActionResult Index()
        {
            return View(_db.Decisions.ToList());
        }

        // GET: Decisions/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
           if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Decision decision = _db.Decisions.Find(id);
            if (decision == null)
            {
                return HttpNotFound();
            }

            List<Decision> citedDecisions = new List<Decision>();
            if (decision.CitedCases != null && decision.CitedCases != "")
                citedDecisions = await _getCited(decision);
            //ViewBag.CitedDecisions = citedDecisions;

            if (!decision.TextDownloaded)
            {
                EpoSearch.GetDecisionText(decision);
                _db.SaveChanges();
            }            
            
            List<Comment> comments = _db.Comments.ToList()
                .Where
                    (c => c.CaseNumber == decision.CaseNumber
                        &&  ( c.IsAccepted || c.Author == _applicationUser )
                    )
                .OrderByDescending(c => c.DateSubmitted)
                .ToList();

            DecisionDetailsViewModel ddvm = new DecisionDetailsViewModel
            {
                Decision = decision,
                CitedDecisions = citedDecisions,
                Facts = decision.Facts.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries),
                Reasons = decision.Reasons.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries),
                Order = decision.Order.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries),
                Comments = comments,
            };

            return View(ddvm);
        }
        
        // GET: Decisions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Decision decision = _db.Decisions.Find(id);
            if (decision == null)
            {
                return HttpNotFound();
            }
            return View(decision);
        }

        // POST: Decisions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CaseNumber,MetaDownloaded,DecisionDate,OnlineDate,Applicant,Opponents,Appellants,Respondents,ApplicationNumber,Ipc,Title,ProcedureLanguage,Board,Keywords,Articles,Rules,Ecli,CitedCases,Distribution,Headword,Catchwords,DecisionLanguage,Link,PdfLink,TextDownloaded,HasSplitText,FactHeader,ReasonsHeader,OrderHeader,Facts,Reasons,Order")] Decision decision)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(decision).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(decision);
        }

        // GET: Decisions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Decision decision = _db.Decisions.Find(id);
            if (decision == null)
            {
                return HttpNotFound();
            }
            return View(decision);
        }

        // POST: Decisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Decision decision = _db.Decisions.Find(id);
            _db.Decisions.Remove(decision);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        #region helper methods
        private async Task<List<Decision>> _getCited(Decision decision)
        {
            List<Decision> citedDecisions = new List<Decision>();

            foreach (string cited in decision.CitedCases.Split(','))
            {
                string ctd = cited.Trim();
                Decision inDB = _db.Decisions.FirstOrDefault(
                    dec => dec.CaseNumber == ctd
                            && dec.DecisionLanguage == dec.ProcedureLanguage);

                if (inDB != null)
                {
                    citedDecisions.Add(inDB);
                }
                else
                {
                    // we don't have this decision yet
                    // or decision has a typo?
                    // try to find it on the EPO site; if we do,
                    // add it to the DB

                    List<Decision> fromEPO = await Epo_facade.EpoSearch.SearchCaseNumberAsync(ctd);
                    if (fromEPO == null || fromEPO.Count == 0)
                    {
                        citedDecisions.Add(new Decision() { CaseNumber = ctd, Title = "Not found" });
                        continue;
                    }
                    
                    bool added = false;
                    foreach (Decision dec in fromEPO)
                    {
                        _db.AddOrUpdate(dec);
                        if (dec.DecisionLanguage == dec.ProcedureLanguage && !added)
                        {
                            citedDecisions.Add(dec);
                            added = true;
                        }
                    }
                    if (!added)
                        citedDecisions.Add(fromEPO.First());
                }
            }

            return citedDecisions;
        }
        #endregion

        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        #endregion

    }
}
