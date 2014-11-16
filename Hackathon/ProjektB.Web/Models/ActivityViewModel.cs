using Microsoft.AspNet.Identity.EntityFramework;
using ProjektB.Web.Models.FitnessProviderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektB.Web.Models
{
    public class ActivityViewModel
    {
        public string UserName { get; set; }
        public ActivityType ActivityType { get; set; }
        public double Score { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public IdentityUserLogin UserLogin { get; set; }

        public string UserImage
        {
            get
            {
                string image = string.Empty;
                if (UserLogin != null)
                {
                    switch (UserLogin.LoginProvider)
                    {
                        case "Facebook":
                            image = string.Format("https://graph.facebook.com/{0}/picture?height=50&width=50", UserLogin.ProviderKey);
                            break;
                    }
                }

                return image;
            }
        }
    }
}