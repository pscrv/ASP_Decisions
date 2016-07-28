using ASP_Decisions.Models;
using ASP_Decisions.Models.ViewModels;
using ASP_Decisions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP_Decisions.Controllers
{
    public class SearchController : BaseController
    {
        [AllowAnonymous]
        public ActionResult BasicSearch(SearchViewModel svm)
        {
            return View();
        }

        // GET: Search
        [AllowAnonymous]
        public ActionResult BasicSearchResult(SearchViewModel svm)
        {
            if (svm == null)
                return RedirectToAction("BasicSearch");            

            var results =
                _db.Decisions
                    .WhereIf(!string.IsNullOrWhiteSpace(svm.CaseNumber), d => d.CaseNumber == svm.CaseNumber)
                    .WhereIf(!string.IsNullOrWhiteSpace(svm.Ecli), d => d.Ecli == svm.Ecli)
                    .WhereIf(!string.IsNullOrWhiteSpace(svm.Board), d => d.Board == svm.Board)
                    .WhereIf(!string.IsNullOrWhiteSpace(svm.Article), d => d.Articles.Contains(svm.Article))
                    .WhereIf(!string.IsNullOrWhiteSpace(svm.Rule), d => d.Rules.Contains(svm.Rule))
                    .WhereIf(!string.IsNullOrWhiteSpace(svm.CitedCase), d => d.CitedCases.Contains(svm.CitedCase))
                    .WhereIf(!string.IsNullOrWhiteSpace(svm.Party), d => 
                           d.Applicant == svm.Party
                        || d.Opponents == svm.Party
                        || d.Appellants == svm.Party
                        || d.Respondents == svm.Party);
                return View(results);
            

        }
    }
}