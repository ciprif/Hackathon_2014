﻿using ProjektB.Web.Infrastructure;
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
using Microsoft.AspNet.Identity.EntityFramework;

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

                if (ViewBag.HasProviders && ViewBag.HasTeam)
                {
                    return View("LeaderBoard", GetUserActivities());
                }
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
            var userActivities = GetUserActivities();

            return View(userActivities);
        }

        private IEnumerable<IGrouping<string, ActivityViewModel>> GetUserActivities()
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
                                      TeamName = team.Name,
                                      TimeStamp = activity.Timestamp,
                                      UserLogin = user.Logins.FirstOrDefault()
                                  }).GroupBy(a => a.TeamName).ToList().OrderByDescending(g => g.GetScore());


            return userActivities;
        }

        public async Task<ActionResult> UserStatistics(string userId)
        {
            if (userId == null)
                userId = UserId;

            UserStatisticsViewModel userStatistics = new UserStatisticsViewModel();

            SyncModule SyncModule = new SyncModule();
            List<UserDetails> userDetails = await SyncModule.GetUserDetailsByApplicationUserId(userId);

            userStatistics.FirstName = userDetails.FirstOrDefault().FirstName;
            userStatistics.LastName = userDetails.FirstOrDefault().LastName;
            userStatistics.Email = Repository.Users.Single(x => x.Id == userId).UserName;

            userStatistics.UserActivities = new List<Activity>();

            userStatistics.FitnessProviderLinks = new Dictionary<ProviderType, string>();

            foreach (UserDetails ud in userDetails)
            {
                userStatistics.UserActivities.AddRange(ud.Activities);

                switch (ud.ProviderType)
                {
                    case ProviderType.MapMyFitness:
                        string mapMyFitnessUserId = ud.UserId;
                        userStatistics.FitnessProviderLinks.Add(ud.ProviderType, string.Format(@"http://www.mapmyfitness.com/profile/{0}/", mapMyFitnessUserId));
                        break;
                    case ProviderType.FitBit:
                        string fitBitUserId = ud.UserId;
                        userStatistics.FitnessProviderLinks.Add(ud.ProviderType, string.Format(@"https://www.fitbit.com/user/{0}", fitBitUserId));
                        break;
                }
            }

            userStatistics.UserActivities = userStatistics.UserActivities.OrderByDescending(x => x.Timestamp).ToList();

            userStatistics.Height = Math.Round(userDetails.FirstOrDefault(x => x.UserStats != null && x.UserStats.Height > 0) != null ? userDetails.FirstOrDefault(x => x.UserStats.Height > 0).UserStats.Height : 0, 2);
            userStatistics.Weight = Math.Round(userDetails.FirstOrDefault(x => x.UserStats != null && x.UserStats.Weight > 0) != null ? userDetails.FirstOrDefault(x => x.UserStats.Weight > 0).UserStats.Weight : 0, 2);

            userStatistics.UserName = userStatistics.Email;
            userStatistics.Team = Repository.UserDetails.Where(x => x.ApplicationUserId == userId).ToList().Select(x => x.TeamId).ToList().FirstOrDefault();
            userStatistics.TeamName = Repository.Teams.Where(x => x.Id == userStatistics.Team).ToList().Select(x => x.Name).ToList().FirstOrDefault();

            List<UserActivity> dbActivities = Repository.UserActivities.Where(x => x.ApplicationUserId == userId).ToList();

            userStatistics.Score = 0;
            foreach (UserActivity act in dbActivities)
            {
                userStatistics.Score += act.Score;
            }

            userStatistics.Score = Math.Round(userStatistics.Score, 2);

            userStatistics.ImagePath = string.Format("http://graph.facebook.com/{0}/picture?height=300&width=300", Repository.Users.Where(x => x.Id == userId).FirstOrDefault().Logins.FirstOrDefault().ProviderKey);
            userStatistics.Gender = userDetails.FirstOrDefault().Gender == Gender.Male ? "Male" : "Female";
            return View(userStatistics);
        }
    }
}