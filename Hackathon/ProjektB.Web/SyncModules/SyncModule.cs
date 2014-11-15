using ProjektB.Web.FitnessProviders;
using ProjektB.Web.FitnessProviders.MapMyFitness.Models;
using ProjektB.Web.Models;
using ProjektB.Web.Models.FitnessProviderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ProjektB.Web.SyncModules
{
    public class SyncModule : ISyncModule
    {
        public async Task Sync()
        {
            try
            {
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
                        ApplicationUser = x.ApplicationUser,
                        ApplicationUserId = x.ApplicationUserId,
                        ConnectionDetails = x.ConnectionDetails,
                        Id = x.Id,
                        Type = x.Type
                    }).ToList();

                    foreach (FitnessProvider provider in providers)
                    {
                        switch(provider.Type)
                        {
                            case ProviderType.MapMyFitness:
                                MapMyFitnessUser myFitnessUser = (MapMyFitnessUser)(await MapMyFitness.GetAuthenticatedUser("f34nz6t9h3unxp4s46bs2jg8py7kvq3e", provider.ConnectionDetails));
                                myFitnessUser.Activities = await MapMyFitness.GetWorkoutByUserId("f34nz6t9h3unxp4s46bs2jg8py7kvq3e", provider.ConnectionDetails, myFitnessUser.UserId);
                                break;
                        }
                    }
                }
            }
            catch(Exception e)
            {

            }
        }
    }
}