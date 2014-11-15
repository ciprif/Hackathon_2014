using ProjektB.Web.Models.FitnessProviderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektB.Web.Models
{
    public class UserActivity
    {
        public int Id { get; set; }

        public ProviderType ProviderType { get; set; }

        public string ApplicationUserId { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public ActivityType ActivityType { get; set; }

        public double Score { get; set; }
    }
}