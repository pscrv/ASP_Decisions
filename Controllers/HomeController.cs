using ASP_Decisions.Models;
using ASP_Decisions.Search;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace ASP_Decisions.Controllers
{
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            string searchTerm = Request["caseNumber"];

            if (!string.IsNullOrEmpty(searchTerm))
            {
                Decision decision = LocalAndRemoteSearch.SearchCaseNumber(searchTerm);
                if (decision == null)
                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction("Details", "Decision", new { id = decision.Id });
            }


            await Epo_facade.DailyUpdate.TryUpdate();

            List<Decision> decisions = _db.Decisions
                .OrderByDescending(d => d.OnlineDate)
                .Take(10)
                .ToList();
            
            return View(decisions);
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            List<string> types = new List<string> {"T", "G", "R", "J", "D", "W", "All" };
            Dictionary<string, _Counts_> countDictionary = new Dictionary<string, _Counts_>();

            foreach (string str in types)
                countDictionary[str] = _countType(str);

            _Counts_ all = new _Counts_();
            all.Total = _db.Decisions.Count();
            all.WithMeta = _db.Decisions.Count(dec => dec.MetaDownloaded);
            all.WithText = _db.Decisions.Count(dec => dec.TextDownloaded);
            countDictionary["All"] = all;

            ViewBag.CountDictionary = countDictionary;
            ViewBag.Types = types;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }


        #region private helper methods
        private _Counts_ _countType(string start)
        {
            _Counts_ result = new _Counts_();
            
            result.Total = _db.Decisions.Count(dec => dec.CaseNumber.StartsWith(start));
            result.WithMeta = _db.Decisions.Count(
                dec => dec.CaseNumber.StartsWith(start)
                && dec.MetaDownloaded);
            result.WithText = _db.Decisions.Count(
                dec => dec.CaseNumber.StartsWith(start)
                && dec.TextDownloaded);

            return result;
        }
        #endregion

        #region helper struct
        public struct _Counts_
        {
            public int Total;
            public int WithMeta;
            public int WithText;
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