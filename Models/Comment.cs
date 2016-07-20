using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASP_Decisions.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public Decision Decision { get; set; }
        public ApplicationUser Author { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsChecked { get; set; }


        [DataType(DataType.Date)]
        public DateTime DateSubmitted { get; set; }

        [DataType(DataType.Date)]
        public DateTime DatePublished { get; set; }
    }
}