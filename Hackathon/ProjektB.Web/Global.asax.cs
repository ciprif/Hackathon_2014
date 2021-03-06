﻿using Castle.Core.Logging;
using Castle.Windsor;
using ProjektB.Web.App_Start;
using ProjektB.Web.Infrastructure;
using ProjektB.Web.SyncModules;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ProjektB.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {

        public static WindsorContainer Container { get; set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public void Application_Error(Object sender, EventArgs e)
        {
            UnitOfWork uoc = Container.Resolve<UnitOfWork>();
            uoc.Rollback();

            Exception ext = Server.GetLastError();

            //TODO: don't forget to treat the error here

            (Container.Resolve<ILogger>()??NullLogger.Instance).Debug("An error has occured: " + ext);
        }
    }
}
