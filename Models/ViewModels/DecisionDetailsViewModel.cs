using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_Decisions.Models.ViewModels
{
    public class DecisionDetailsViewModel
    {
        public Decision Decision { get; set; }
        public List<Decision> CitedDecisions { get; set; }
        public string[] Facts { get; set; }
        public string[] Reasons { get; set; }
        public string[] Order { get; set; }
        public List<Comment> Comments { get; set; }
    }
}