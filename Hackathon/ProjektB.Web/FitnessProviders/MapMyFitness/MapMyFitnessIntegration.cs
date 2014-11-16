using ProjektB.Web.FitnessProviders.MapMyFitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using ProjektB.Web.FitnessProviders.Interfaces;
using Castle.Core.Logging;
using ProjektB.Web.Models.FitnessProviderModels;
using ProjektB.Web.Models;

namespace ProjektB.Web.FitnessProviders
{
    public class MapMyFitnessIntegration
    {
        /// <summary>
        /// Logger
        /// </summary>
        private ILogger logger = NullLogger.Instance;

        public ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        public async Task<IUserDetails> GetAuthenticatedUser(string apiKey, string authorization)
        {
            MapMyFitnessUser UserInformation = new MapMyFitnessUser();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://oauth2-api.mapmyapi.com/v7.0/");

                    HttpRequestHeaders headers = client.DefaultRequestHeaders;

                    headers.Add("Api-Key", apiKey);
                    headers.TryAddWithoutValidation("Content-Type", "application/json");
                    headers.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    HttpResponseMessage response = await client.GetAsync("user/self/");

                    if (response.IsSuccessStatusCode)
                    {
                        JObject Content = await response.Content.ReadAsAsync<JObject>();

                        UserInformation.FirstName = (string)Content["first_name"];
                        UserInformation.LastName = (string)Content["last_name"];
                        UserInformation.Birthday = DateTime.Parse((string)Content["birthdate"]);
                        UserInformation.DisplayName = (string)Content["display_name"];
                        UserInformation.Email = (string)Content["email"];
                        UserInformation.Gender = (string)Content["gender"] == "M" ? Gender.Male : Gender.Female;
                        UserInformation.ImagePath = (string)Content["_links"]["image"][0]["href"];
                        UserInformation.UserId = (int)Content["id"];
                        
                        UserInformation.Location = new MapMyFitnessLocation
                        {
                            City = (string)Content["location"]["locality"],
                            Country = (string)Content["location"]["country"]
                        };

                        UserInformation.UserStats = new MyFitnessUserStats
                        {
                            Height = (double)((string)Content["height"] == null ? 0 : Content["height"]),
                            Weight = (double)((string)Content["weight"] == null ? 0 : Content["weight"])
                        };
                    }
                }
            }
            catch(Exception e)
            {
                Logger.Error("An error has occured: " + " " + e.InnerException.Message);
            }
            
            return UserInformation;
        }

        public async Task<List<IActivity>> GetWorkoutByUserId(string apiKey, string authorization, int userId, DateTimeOffset? startedAfter = null, DateTimeOffset? startedBefore = null, string activityType = null)
        {
            List<IActivity> activities = new List<IActivity>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://oauth2-api.mapmyapi.com/v7.0/");

                    HttpRequestHeaders headers = client.DefaultRequestHeaders;

                    headers.Add("Api-Key", apiKey);
                    headers.TryAddWithoutValidation("Content-Type", "application/json");
                    headers.Authorization = new AuthenticationHeaderValue("Bearer", authorization);

                    string requestUri = "workout/?";

                    if (activityType != null)
                    {
                        requestUri += "activity_type=" + activityType + "&";
                    }

                    if (startedAfter != null)
                    {
                        requestUri += "started_after=" + startedAfter.ToString() + "&";
                    }

                    if (startedBefore != null)
                    {
                        requestUri += "started_before=" + startedBefore.ToString() + "&";
                    }

                    requestUri += "user=" + userId.ToString();

                    HttpResponseMessage response = await client.GetAsync(requestUri);

                    if (response.IsSuccessStatusCode)
                    {
                        JObject Content = await response.Content.ReadAsAsync<JObject>();
                        JArray workouts = Content["_embedded"]["workouts"] as JArray;

                        foreach(JToken workout in workouts)
                        {
                            MapMyFitnessActivity activity =  new MapMyFitnessActivity();
                            activity.Description = (string)workout["notes"];
                            activity.Name = (string)workout["name"];
                            activity.ID = (int)workout["_links"]["self"][0]["id"];
                            activity.Duration = (int)workout["aggregates"]["active_time_total"];
                            activity.Provider = ProviderType.MapMyFitness;
                            activity.Timestamp = DateTimeOffset.Parse((string)workout["start_datetime"]);
                            activity.Values = new List<ActivityValue>
                            {
                                new ActivityValue{
                                    Unit = ActivityUnit.Distance,
                                    Value = (double)workout["aggregates"]["distance_total"]
                                },
                                new ActivityValue{
                                    Unit = ActivityUnit.Calories,
                                    Value = (double)workout["aggregates"]["metabolic_energy_total"]
                                }
                            };

                            int activityTypeValue = (int)workout["_links"]["activity_type"][0]["id"];
                            switch (activityTypeValue)
                            { 
                                case 9:
                                    activity.ActivityType = ActivityType.Walking;
                                    break;
                                case 16:
                                    activity.ActivityType = ActivityType.Running;
                                    break;
                                case 36:
                                    activity.ActivityType = ActivityType.Cycling;
                                    break;
                                case 44:
                                    activity.ActivityType = ActivityType.Cycling;
                                    break;
                                case 47:
                                    activity.ActivityType = ActivityType.Cycling;
                                    break;
                                case 53:
                                    activity.ActivityType = ActivityType.Cycling;
                                    break;
                                case 64:
                                    activity.ActivityType = ActivityType.Cycling;
                                    break;
                                default:
                                    activity.ActivityType = ActivityType.Flexible;
                                    break;
                            }

                            activities.Add(activity);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Debug("An error has occured: " + " " + e.InnerException.Message);
            }

            return activities;
        }
    }
}