using ProjektB.Web.Models.FitnessProviderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektB.Web.FitnessProviders.Interfaces
{
    public enum Gender { Male, Female }

    public interface IUserDetails
    {
        int UserId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string DisplayName { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
        DateTime Birthday { get; set; }
        Gender Gender { get; set; }
        List<IActivity> Activities { get; set; }
        string ImagePath { get; set; }
        IUserStats UserStats { get; set; }
        IUserLocation Location { get; set; }
    }
}
