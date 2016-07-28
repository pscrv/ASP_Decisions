using ASP_Decisions.Formatters;
using ASP_Decisions.Search;
using System;
using System.ComponentModel.DataAnnotations;

namespace ASP_Decisions.Models.ViewModels
{
    public class SearchViewModel
    {
        #region explicit backing fields
        private string _caseNumber;
        private string _citedCase;
        #endregion


        #region searchable db fields
        [Display(Name = "Case Number")]
        public string CaseNumber
        {
            get { return _caseNumber; }
            set { _caseNumber = Formatter.FormatCaseNumber(value); }
        }

        public string Ecli { get; set; }
        public string Board { get; set; }
        public string Party { get; set; } //applicant, opponent, appellant, respondent
        public Generic.Languages DecisionLanguage { get; set; }
        public Generic.Languages ProceedingsLanguage { get; set; }
        public string Article { get; set; }
        public string Rule { get; set; }

        [Display(Name = "Cited Case")]
        public string CitedCase
        {
            get { return _citedCase; }
            set { _citedCase = Formatter.FormatCaseNumber(value); }
        }

        [DataType(DataType.Date)]
        public DateTime DecisionDate { get; set; }
        #endregion
    }
}