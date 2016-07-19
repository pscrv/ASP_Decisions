using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP_Decisions.Controllers
{
    [Authorize(Roles = "admin")]
    public abstract class BaseController : Controller
    {
        // This is basically a wrapper class for all controllers
        // It should ensure that each controller which inherits from this
        // will have to be explicit about which actions are accessible to which users
    }
}