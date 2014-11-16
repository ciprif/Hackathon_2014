using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjektB.Web.FitnessProviders.Interfaces;

namespace ProjektB.Web.FitnessProviders.MapMyFitness.Models
{
    public class MapMyFitnessUser : IUserDetails
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime Birthday { get; set; }

        public Gender Gender { get; set; }

        public List<Web.Models.FitnessProviderModels.Activity> Activities { get; set; }

        public string ImagePath { get; set; }

        public IUserStats UserStats { get; set; }

        public IUserLocation Location { get; set; }
    }
}