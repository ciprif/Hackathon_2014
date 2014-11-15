using ProjektB.Web.FitnessProviders;
using ProjektB.Web.FitnessProviders.MapMyFitness.Models;
using ProjektB.Web.Models;
using ProjektB.Web.Models.FitnessProviderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ProjektB.Web.SyncModule
{
    public class SyncModule
    {
        public static async Task Sync()
        {
            Repository repository = MvcApplication.Container.Resolve<Repository>();

            List<UserDetail> dbUsers = repository.UserDetails.ToList()
                .Select(x => new UserDetail
                {
                    ApplicationUser = x.ApplicationUser,
                    ApplicationUserId = x.ApplicationUserId,
                    Id = x.Id,
                    TeamId = x.TeamId
                }).ToList();
            

            MapMyFitnessIntegration MapMyFitness = new MapMyFitnessIntegration();
            List<MapMyFitnessUser> mapMyFitnessUsers = new List<MapMyFitnessUser>();

            foreach (UserDetail user in dbUsers)
            {
                //repository.
                //.Select(x => new UserDetail
                //{
                //    ApplicationUser = x.ApplicationUser,
                //    ApplicationUserId = x.ApplicationUserId,
                //    Id = x.Id,
                //    TeamId = x.TeamId
                //}).ToList();

                MapMyFitnessUser myFitnessUser = (MapMyFitnessUser)(await MapMyFitness.GetAuthenticatedUser("f34nz6t9h3unxp4s46bs2jg8py7kvq3e", "9b4c2047b76225b13274edd7ea2412e1cae03ff1"));
                List<IActivity> activities = await MapMyFitness.GetWorkoutByUserId("f34nz6t9h3unxp4s46bs2jg8py7kvq3e", "9b4c2047b76225b13274edd7ea2412e1cae03ff1", myFitnessUser.UserId);
            }
        }
    }
}