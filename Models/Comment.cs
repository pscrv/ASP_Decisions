using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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

        [Required]
        [MinLength(10)]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Required]
        public string CaseNumber { get; set; }

        [Required]
        public virtual ApplicationUser Author { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime DateSubmitted { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DatePublished { get; set; }

        public bool IsAccepted { get; set; }
        public bool IsChecked { get; set; }

    }
}