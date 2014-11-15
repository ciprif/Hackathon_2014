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
    }
}