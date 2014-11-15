using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ProjektB.Web.Controllers
{
    public class BaseController : Controller
    {
        public string UserId { get { return User.Identity.GetUserId(); } }
    }
}