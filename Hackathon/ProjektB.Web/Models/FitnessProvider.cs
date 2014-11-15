using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektB.Web.Models
{
    public class FitnessProvider
    {
        public int Id { get; set; }

        public string ConnectionDetails { get; set; }

        public ProviderType Type { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }

    public enum ProviderType
    {
        FitBit = 1,
        MapMyFitness = 2,
    }
}