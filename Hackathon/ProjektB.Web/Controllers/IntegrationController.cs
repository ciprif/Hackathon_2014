using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProjektB.Web.Models.FitnessProviderModels;
using System.Threading.Tasks;
using ProjektB.Web.SyncModules;
using ProjektB.Web.Models;

namespace ProjektB.Web.Controllers
{
    public class IntegrationController : ApiController
    {
        [HttpGet]
        [ActionName("Activity")]
        public async Task<IHttpActionResult> GetActivityById(string id)
        {
            //TODO: finish this method!
            Repository repo = MvcApplication.Container.Resolve<Repository>();
            
            var startTime = DateTimeOffset.Now.AddMinutes(-10);
            var recentActivities = repo.UserActivities.Where(x => x.ApplicationUserId == id && x.Timestamp > startTime);
            double score = recentActivities.ToList().Sum(x => x.Score);

            Object result = new
            {
                needsMovement = score < 0.9
            };

            return Json(result);
        }

        [HttpGet]
        [ActionName("Activity")]
        public async Task<IHttpActionResult> GetActivity()
        {
            SyncModule syncModule = new SyncModule();
            await syncModule.Sync();

            return Ok();
        }
    }
}
