using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjektB.Web.Models.FitnessProviderModels;
using Fitbit.Models;

namespace ProjektB.Web.Models
{
    public class UserStatisticsViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public List<ProjektB.Web.Models.FitnessProviderModels.Activity> UserActivities { get; set; }
        public double Score { get; set; }
        public int Team{ get; set; }
        public string TeamName { get; set; }
        public Dictionary<ProviderType, string> FitnessProviderLinks { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string ImagePath { get; set; }
    }
}