using ProjektB.Web.Models.FitnessProviderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektB.Web.Models
{
    public class ActivityViewModel
    {
        public string UserName { get; set; }
        public ActivityType ActivityType{ get; set; }
        public double Score { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
    }
}