using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProjektB.Web.Models.FitnessProviderModels;
using ProjektB.Web.FitnessProviders;
using System.Threading.Tasks;
using ProjektB.Web.FitnessProviders.MapMyFitness.Models;

namespace ProjektB.Web.Controllers
{
    public class IntegrationController : ApiController
    {
        // GET: api/Integration/5
        public static async Task Sync()
        {
            MapMyFitnessIntegration MapMyFitness = new MapMyFitnessIntegration();
            MapMyFitnessUser user = (MapMyFitnessUser)(await MapMyFitness.GetAuthenticatedUser("f34nz6t9h3unxp4s46bs2jg8py7kvq3e", "9b4c2047b76225b13274edd7ea2412e1cae03ff1"));
            List<IActivity> activities = await MapMyFitness.GetWorkoutByUserId("f34nz6t9h3unxp4s46bs2jg8py7kvq3e", "9b4c2047b76225b13274edd7ea2412e1cae03ff1", user.UserId);
        }
    }
}
