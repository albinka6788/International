using International.BusinessEntities.Models;
using International.BusinessLogic.Managers;
using International.Entities;
using International.Entities.MDMEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace International.BusinessLogic.Classes
{
    public class UnitOfWork : IDisposable
    {
        private InternationalSubmissionEntities applicationContext { get; set; }
        private MDMEntities mdmContext { get; set; }

        public UnitOfWork()
        {
            this.applicationContext = new InternationalSubmissionEntities();
            this.applicationContext.Configuration.LazyLoadingEnabled = false;
            this.mdmContext = new MDMEntities();
        }

        public LoggedInUser LoggedInUser { get; set; }

        private MasterManager _master;
        public MasterManager Master
        {
            get
            {
                if (this._master == null)
                    this._master = new MasterManager(applicationContext, mdmContext);

                return this._master;
            }
        }

        private SubmissionManager _submission;
        public SubmissionManager Submission
        {
            get
            {
                if (this._submission == null)
                    this._submission = new SubmissionManager(applicationContext, mdmContext);

                return this._submission;
            }
        }
        

        private UtilityManager _utility;
        public UtilityManager Utilities
        {
            get
            {
                if (this._utility == null)
                    this._utility = new UtilityManager(this.applicationContext);

                return this._utility;
            }
        }

        public void SaveChanges()
        {
         //   applicationContext.SaveChanges();

            try
            {
                applicationContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public void Dispose()
        {
            if(this.applicationContext != null)
            {
                this.applicationContext.Dispose();
                this.applicationContext = null;
            }
        }

        private UserManager _userManager;
        public UserManager UserManager
        {
            get
            {
                if (this._userManager == null)
                    this._userManager = new UserManager(this.applicationContext, this.mdmContext);

                return this._userManager;
            }
        }

    }
}
