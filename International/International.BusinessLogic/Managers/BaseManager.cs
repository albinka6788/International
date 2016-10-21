using International.BusinessLogic.Classes;
using International.Entities;
using International.Entities.MDMEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace International.BusinessLogic.Managers
{
    public class BaseManager
    {
        #region Config

        private InternationalSubmissionEntities _context { get; set; }
        protected InternationalSubmissionEntities Context { get { return _context; } }

        private MDMEntities _mdmContext { get; set; }
        protected MDMEntities mdmContext { get { return _mdmContext; } }

        public BaseManager(){}

        public BaseManager(InternationalSubmissionEntities applicationContext, MDMEntities mdmContext)
        {
            if (this._context == null)
                this._context = applicationContext;
            if (this._mdmContext == null)
                this._mdmContext = mdmContext;
        }



        private UnitOfWork _applicationContext;
        public UnitOfWork ApplicationContext
        {
            get
            {
                if (this._applicationContext == null)
                    this._applicationContext = new UnitOfWork();

                return this._applicationContext;
            }
        }
        #endregion

    }
}
