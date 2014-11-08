using ProjektB.Web.Infrastructure;
using ProjektB.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Logging;

namespace ProjektB.Web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Logger
        /// </summary>
        private ILogger logger = NullLogger.Instance;
        
        protected ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        /// <summary>
        /// do not use dirrectly.
        /// </summary>
        private Lazy<UnitOfWork> _lazyUoW;

        protected UnitOfWork UoW { get { return _lazyUoW.Value; } }

        public Repository Repository { get; set; }

        public HomeController(Lazy<UnitOfWork> lazyUoW)
        {
            _lazyUoW = lazyUoW;
        }

        public ActionResult Index()
        {
            Logger.Debug("IndexPage.");
            var todos = Repository.ToDos.ToList();
            

            Repository.ToDos.Add(new ToDo { Payload = "tralala" });

            //throw new Exception();

            return View();
        }

        public ActionResult About()
        {
            Logger.Debug("AboutPage.");
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            Logger.Debug("ContactPage.");
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}