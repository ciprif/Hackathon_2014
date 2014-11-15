using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektB.Web.Models
{
    public class FitnessProviderFBPayload
    {
        public string AuthToken { get; set; }

        public string AuthTokenSecret { get; set; }

        public string UserId { get; set; }
    }
}