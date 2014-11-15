using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProjektB.Web.Models.FitnessProviderModels;
using System.Threading.Tasks;

namespace ProjektB.Web.Controllers
{
    public class IntegrationController : ApiController
    {
        // GET: api/Integration/5
        [HttpGet]
        [ActionName("Activity")]
        public async Task<IHttpActionResult> GetActivityById(string id)
        {
            //TODO: finish this method!

            Object result = new
            {
                needsMovement = true
            };

            return Json(result);
        }

        [HttpGet]
        [ActionName("Activity")]
        public async Task<IHttpActionResult> GetActivity()
        {
            // method to be called from scheduler
            return Ok();
        }
    }
}
