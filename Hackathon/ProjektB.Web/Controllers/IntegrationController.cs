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
        public string GetActivityById(int id)
        {
            return "value";
        }

        [HttpGet]
        public async Task<IHttpActionResult> Status(string userIdHash)
        {
            Object result = new
            {
                needsMovement = true
            };

            return Json(result);
        }
    }
}
