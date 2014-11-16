using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektB.Web.Models
{
    public static class ModelExtensions
    {
        public static double GetScore(this IGrouping<string, ActivityViewModel> activityGroup)
        {
            return (activityGroup.Sum(a => a.Score) / activityGroup.GroupBy(g => g.UserName).Count());
        }
    }
}