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
using ProjektB.Web.Models.FitnessProviderModels;
using ProjektB.Web.FitnessProviders.MapMyFitness.Models;
using ProjektB.Web.FitnessProviders.Interfaces;

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
            var userActivities = (from activity in Repository.UserActivities
                                 join user in Repository.Users on activity.ApplicationUserId equals user.Id
                                 join detail in Repository.UserDetails on activity.ApplicationUserId equals detail.ApplicationUserId
                                 join team in Repository.Teams on detail.TeamId equals team.Id
                                 select new ActivityViewModel
                                 {
                                     UserName = user.UserName,
                                     ActivityType = activity.ActivityType,
                                     Score = activity.Score,
                                     TeamId = detail.TeamId,
                                     TeamName = team.Name
                                 }).GroupBy(a => a.TeamName);

            return View(userActivities);
        }

        public async Task<ActionResult> UserStatistics()
        {
           UserStatisticsViewModel userStatistics = new UserStatisticsViewModel();

           SyncModule SyncModule = new SyncModule();
           List<IUserDetails> userDetails = await SyncModule.GetUserDetailsByApplicationUserId(UserId);

           userStatistics.FirstName = userDetails.FirstOrDefault().FirstName;
           userStatistics.LastName = userDetails.FirstOrDefault().LastName;
           userStatistics.Email = userDetails.FirstOrDefault().Email;
           foreach(IUserDetails ud in userDetails)
           {
               userStatistics.UserActivities.AddRange(ud.Activities);
           }

           userStatistics.UserActivities = userStatistics.UserActivities.OrderByDescending(x => x.Timestamp).ToList();
           userStatistics.Height = userDetails.FirstOrDefault(x => x.UserStats.Height > 0).UserStats.Height;
           userStatistics.Weight = userDetails.FirstOrDefault(x => x.UserStats.Weight > 0).UserStats.Weight;

           userStatistics.UserName = userStatistics.Email;
           userStatistics.Team = Repository.UserDetails.Where(x => x.ApplicationUserId == UserId).ToList().Select(x => x.TeamId).ToList().FirstOrDefault();
           userStatistics.TeamName = Repository.Teams.Where(x => x.Id == userStatistics.Team).ToList().Select(x => x.Name).ToList().FirstOrDefault();

           List<FitnessProvider> providers = Repository.FitnessProviders.Where(x => x.ApplicationUserId == UserId).ToList()
                  .Select(x => new FitnessProvider
                  {
                      Type = x.Type
                  }).ToList();

           foreach(FitnessProvider fp in providers)
           {
               //switch (fp.Type)
               //{
               //    case ProviderType.MapMyFitness:
               //     userStatistics.FitnessProviderLinks.Add(fp.Type, string.Format(@"http://www.mapmyfitness.com/profile/{0}/", userDetails.Find(x => x.)
               //     break;
               //}
           }

           return View(userStatistics);
        }
    }
}