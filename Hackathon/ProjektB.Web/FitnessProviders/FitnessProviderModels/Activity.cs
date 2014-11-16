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

    public enum ActivityType
    {
        Running, Walking, Flexible, Cycling
    }

    public class ActivityValue
    {
        public ActivityUnit Unit { get; set; }
        public double Value { get; set; }
    }

    public class Activity
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