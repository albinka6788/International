using International.Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace International.Web.Controllers
{
    public class DashboardController : BaseController
    {
        //
        // GET: /Dashboard/
        [Public]
        public ActionResult Welcome()
        {
            return View();
        }

    }
}
