﻿using ProjektB.Web.Models;
using ProjektB.Web.Models.FitnessProviderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektB.Web.FitnessProviders.Interfaces
{
    public enum Gender { Male, Female }

    public class UserDetails
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public Gender Gender { get; set; }
        public List<Activity> Activities { get; set; }
        public string ImagePath { get; set; }
        public IUserStats UserStats { get; set; }
        public IUserLocation Location { get; set; }
        public ProviderType ProviderType { get; set; }
    }
}
