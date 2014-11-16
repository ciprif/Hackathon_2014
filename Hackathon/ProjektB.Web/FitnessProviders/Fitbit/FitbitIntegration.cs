using Fitbit.Api;
using ProjektB.Web.FitnessProviders.Interfaces;
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
            string fitbitAuthTokenSecret, ref DateTime lastCheck, ref int lastSteps)
        {
            FitbitClient client = new FitbitClient(fitbitConsumerKey,
                fitbitConsumerSecret,
                fitbitAuthToken,
                fitbitAuthTokenSecret);

            global::Fitbit.Models.Activity activity =  client.GetDayActivity(DateTime.Now);

            int apiSteps = activity.Summary.Steps;

            int steps = apiSteps;
            if (lastCheck == DateTime.Now.Date)
            {
                steps = apiSteps - lastSteps;
            }

            lastSteps = apiSteps;
            lastCheck = DateTime.Now.Date;

            if (steps == 0) return null;

            return new Activity
            {
                ActivityType = ActivityType.Walking,
                Values = new List<ActivityValue> 
                { 
                    new ActivityValue 
                    { 
                        Unit = ActivityUnit.Steps,
                        Value = steps
                    },
                    new ActivityValue
                    {
                        Unit = ActivityUnit.Calories,
                        Value = steps / 5.035
                    }
                },
                Timestamp = DateTimeOffset.Now
            };
        }

        public UserDetails GetAuthenticatedUser(string fitbitConsumerKey,
            string fitbitConsumerSecret, string fitbitAuthToken, 
            string fitbitAuthTokenSecret)
        {
            FitbitClient client = new FitbitClient(fitbitConsumerKey,
                fitbitConsumerSecret,
                fitbitAuthToken,
                fitbitAuthTokenSecret);

            global::Fitbit.Models.UserProfile profile = client.GetUserProfile();
            var nameSplit = profile.FullName.Split(' ');
            return new UserDetails
            {
                UserId = profile.EncodedId,
                Birthday = profile.DateOfBirth,
                DisplayName = profile.DisplayName,
                Gender = profile.Gender == global::Fitbit.Models.Gender.FEMALE ? Interfaces.Gender.Female : Gender.Male,
                ImagePath = profile.Avatar,
                FirstName = nameSplit[0],
                LastName = nameSplit[1]
            };
        }
    }
}