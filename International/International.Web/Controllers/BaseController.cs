using International.BusinessEntities.Models;
using International.BusinessLogic.Classes;
using International.BusinessLogic.Managers;
using International.Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace International.Web.Controllers
{
    [SecuredWebController, WebControllerException]
    public class BaseController : Controller
    {
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
       

      
    }
}
