using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektB.Web.Models.FitnessProviderModels
{
    public enum ActivityUnit
    {
        Steps, Calories, Distance
    }

    public class ActivityValue
    {
        public ActivityUnit Unit { get; set; }
        public int Value { get; set; }
    }

    public interface Activity
    {
        int ID { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int Duration { get; set; }
        DateTimeOffset Timestamp { get; set; }
        List<ActivityValue> Values { get; set; }
        string Provider { get; set; }
    }
}