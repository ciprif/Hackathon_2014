using ProjektB.Web.Infrastructure;
using ProjektB.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Logging;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using ProjektB.Web.SyncModules;

namespace ProjektB.Web.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// Logger
        /// </summary>
        private ILogger logger = NullLogger.Instance;

        public ILogger Logger
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
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.HasProviders = Repository.FitnessProviders.Where(p => p.ApplicationUserId == UserId).Count() > 0;
                ViewBag.HasTeam = Repository.UserDetails.Where(u => u.ApplicationUserId == UserId).Count() > 0;
            }

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

        public ActionResult ToDoS()
        {
            Logger.Debug("ToDos");

            ToDosViewModel toDos = new ToDosViewModel();

            toDos.Items = Repository.ToDos.ToList()
                .Select(x => new ToDoViewModel
                {
                    ItemNumber = x.ToDoId,
                    ItemValue = x.Payload
                }).ToList();

            return View(toDos);
        }

        public ActionResult AddToDo()
        {
            Logger.Debug("AddToDo");
            return View();
        }

        [HttpPost]
        public ActionResult AddToDo(ToDoViewModel model)
        {
            Logger.Debug("AddToDo");

            Repository.ToDos.Add(new ToDo { Payload = model.ItemValue });

            return RedirectToAction("ToDoS");
        }

        public ActionResult LeaderBoard()
        {
            return View();
        }
    }
}