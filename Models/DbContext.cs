using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ASP_Decisions.Models
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        #region constructors/create
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        #endregion

        #region DbSet
        public DbSet<ASP_Decisions.Models.ApplicationRole> IdentityRoles { get; set; }

        public DbSet<Decision> Decisions { get; set; }

        public DbSet<Comment> Comments { get; set; }
        #endregion


        #region public methods
        public void AddOrUpdate(Decision decision, bool forceUpdate = false)
        {
            Decision inDB = this.Decisions.FirstOrDefault(
                       dec => dec.CaseNumber == decision.CaseNumber
                               && dec.DecisionLanguage == decision.DecisionLanguage);

            bool mustAdd = inDB == null || forceUpdate;

            if (mustAdd)
            {
                this.Decisions.Add(decision);
                if (inDB != null)
                    this.Decisions.Remove(inDB);
                this.SaveChanges();
            }
        }

        public void Save(Decision decision)
        {
            Decision inDB = this.Decisions.FirstOrDefault(dec => dec.Id == decision.Id);

            if (inDB == null)
            {
                this.Decisions.Add(decision);
            }

            this.SaveChanges();
        }
        #endregion
        
    }
}