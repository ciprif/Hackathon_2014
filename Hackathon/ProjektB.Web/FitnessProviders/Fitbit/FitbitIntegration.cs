﻿using Fitbit.Api;
using ProjektB.Web.Models.FitnessProviderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektB.Web.FitnessProviders.Fitbit
{
    public class FitbitIntegration
    {
        public Activity GetWorkoutByUserId(string fitbitConsumerKey,
            string fitbitConsumerSecret, string fitbitAuthToken, 
            string fitbitAuthTokenSecret)
        {
            FitbitClient client = new FitbitClient(fitbitConsumerKey,
                fitbitConsumerSecret,
                fitbitAuthToken,
                fitbitAuthTokenSecret);

            global::Fitbit.Models.Activity activity =  client.GetDayActivity(DateTime.Now);

            return new Activity
            {
                ActivityType = ActivityType.Walking,
                Values = new List<ActivityValue> 
                { 
                    new ActivityValue 
                    { 
                        Unit = ActivityUnit.Steps,
                        Value = activity.Summary.Steps
                    },
                    new ActivityValue
                    {
                        Unit = ActivityUnit.Calories,
                        Value = activity.Summary.Steps / 5.035
                    }
                }
            };
        }
    }
}