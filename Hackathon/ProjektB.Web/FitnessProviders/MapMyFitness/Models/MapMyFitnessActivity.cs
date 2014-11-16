using ProjektB.Web.Models;
using ProjektB.Web.Models.FitnessProviderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektB.Web.FitnessProviders.MapMyFitness.Models
{
    public class MapMyFitnessActivity : IActivity
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public List<ActivityValue> Values { get; set; }

        public ProviderType Provider { get; set; }

        public ActivityType ActivityType { get; set; }
    }
}