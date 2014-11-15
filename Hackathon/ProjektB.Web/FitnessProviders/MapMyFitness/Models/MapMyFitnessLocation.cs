using ProjektB.Web.FitnessProviders.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektB.Web.FitnessProviders.MapMyFitness.Models
{
    public class MapMyFitnessLocation : IUserLocation
    {
        public string Country { get; set; }
        public string City { get; set; }
    }
}