﻿using ProjektB.Web.FitnessProviders.MapMyFitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Script.Serialization;
using System.Net.Http.Headers;

namespace ProjektB.Web.FitnessProviders
{
    public class MapMyFitnessIntegration
    {
        public MapMyFitnessUser UserInformation { get; set; }

        public async void GetAuthenticatedUser(string APIKey, string Authorization)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://oauth2-api.mapmyapi.com/v7.0/");
                HttpRequestHeaders headers = client.DefaultRequestHeaders;
                
                headers.Add("Api-Key", APIKey);
                headers.Add("Authorization", Authorization);
                headers.Add("Content-Type", "application/json");
                headers.Add("X-Originating-Ip", "193.33.93.43");

                HttpResponseMessage response = await client.GetAsync("user/self/");

                if(response.IsSuccessStatusCode)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    object Content = await response.Content.ReadAsAsync(typeof(Object));

                    UserInformation = new MapMyFitnessUser
                    {
                        
                    };
                    
                }
            }
        }
    }
}