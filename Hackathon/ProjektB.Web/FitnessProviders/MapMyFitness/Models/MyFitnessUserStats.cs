using ProjektB.Web.FitnessProviders.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektB.Web.FitnessProviders.MapMyFitness.Models
{
    public class MyFitnessUserStats : IUserStats
    {
        public double Height { get; set; }
        public double Weight { get; set; }
    }
}