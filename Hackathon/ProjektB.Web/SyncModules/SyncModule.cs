using ProjektB.Web.FitnessProviders;
using ProjektB.Web.FitnessProviders.MapMyFitness.Models;
using ProjektB.Web.Models;
using ProjektB.Web.Models.FitnessProviderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Castle.Core.Logging;
using ProjektB.Web.FitnessProviders.Interfaces;


namespace ProjektB.Web.SyncModules
{
    public class SyncModule : ISyncModule
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

        public async Task Sync(DateTimeOffset? fromDate = null)
        {
            try
            {
                if (fromDate == null)
                    fromDate = DateTimeOffset.MinValue;

                Repository repository = MvcApplication.Container.Resolve<Repository>();

                List<ApplicationUser> dbUsers = repository.Users.ToList()
                    .Select(x => new ApplicationUser
                    {
                        Id = x.Id
                    }).ToList();


                MapMyFitnessIntegration MapMyFitness = new MapMyFitnessIntegration();
                List<MapMyFitnessUser> mapMyFitnessUsers = new List<MapMyFitnessUser>();

                foreach (ApplicationUser user in dbUsers)
                {
                    List<FitnessProvider> providers = repository.FitnessProviders.Where(x => x.ApplicationUserId == user.Id).ToList()
                    .Select(x => new FitnessProvider
                    {
                        ConnectionDetails = x.ConnectionDetails,
                        Type = x.Type
                    }).ToList();

                    foreach (FitnessProvider provider in providers)
                    {
                        DateTimeOffset latestTimestamp = DateTimeOffset.MinValue;
                        
                        List<UserActivity> dbUserActivities = repository.UserActivities.Where(x => x.ApplicationUserId == user.Id).ToList();
                        if (dbUserActivities.Count > 0)
                        {
                            latestTimestamp = dbUserActivities.OrderByDescending(x => x.Timestamp).ToList().FirstOrDefault().Timestamp;
                        }

                        FitnessProviderPayload payload = JsonConvert.DeserializeObject<FitnessProviderPayload>(provider.ConnectionDetails);

                        switch(provider.Type)
                        {
                            case ProviderType.MapMyFitness:
                                MapMyFitnessUser myFitnessUser = (MapMyFitnessUser)(await MapMyFitness.GetAuthenticatedUser("f34nz6t9h3unxp4s46bs2jg8py7kvq3e",  payload.key));
                                myFitnessUser.Activities = await MapMyFitness.GetWorkoutByUserId("f34nz6t9h3unxp4s46bs2jg8py7kvq3e", payload.key, myFitnessUser.UserId, fromDate);
                                
                                foreach(MapMyFitnessActivity act in myFitnessUser.Activities)
                                {
                                    if (act.Timestamp.Subtract(latestTimestamp).Minutes > 10)
                                    {
                                        //joules to calories to kilocalories / 50
                                        double score = (act.Values.Find(x => x.Unit == ActivityUnit.Calories).Value * 0.239005736) / 50000;

                                        repository.UserActivities.Add(new UserActivity
                                        {
                                            ActivityType = act.ActivityType,
                                            ApplicationUserId = user.Id,
                                            ProviderType = provider.Type,
                                            Score = score,
                                            Timestamp = act.Timestamp
                                        });
                                    }
                                }

                                break;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Logger.Error("An error has occured: " + e.InnerException.Message);
            }
        }

        public async Task<List<IUserDetails>> GetUserDetailsByApplicationUserId(string applicationUserId)
        {
            List<IUserDetails> userDetails = new List<IUserDetails>();
            Repository repository = MvcApplication.Container.Resolve<Repository>();
            MapMyFitnessIntegration MapMyFitness = new MapMyFitnessIntegration();

            List<FitnessProvider> providers = repository.FitnessProviders.Where(x => x.ApplicationUserId == applicationUserId).ToList()
                    .Select(x => new FitnessProvider
                    {
                        ConnectionDetails = x.ConnectionDetails,
                        Type = x.Type
                    }).ToList();

            foreach (FitnessProvider provider in providers)
            {
                DateTimeOffset latestTimestamp = DateTimeOffset.MinValue;

                List<UserActivity> dbUserActivities = repository.UserActivities.Where(x => x.ApplicationUserId == applicationUserId).ToList();
                if (dbUserActivities.Count > 0)
                {
                    latestTimestamp = dbUserActivities.OrderByDescending(x => x.Timestamp).ToList().FirstOrDefault().Timestamp;
                }

                FitnessProviderPayload payload = JsonConvert.DeserializeObject<FitnessProviderPayload>(provider.ConnectionDetails);

                switch (provider.Type)
                {
                    case ProviderType.MapMyFitness:
                        MapMyFitnessUser myFitnessUser = (MapMyFitnessUser)(await MapMyFitness.GetAuthenticatedUser("f34nz6t9h3unxp4s46bs2jg8py7kvq3e", payload.key));
                        myFitnessUser.Activities = await MapMyFitness.GetWorkoutByUserId("f34nz6t9h3unxp4s46bs2jg8py7kvq3e", payload.key, myFitnessUser.UserId);
                        userDetails.Add(myFitnessUser);
                        break;
                }
            }

            return userDetails;
        }
    }
}