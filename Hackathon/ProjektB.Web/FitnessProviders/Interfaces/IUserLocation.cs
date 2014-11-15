using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektB.Web.FitnessProviders.Interfaces
{
    public interface IUserLocation
    {
        string Country { get; set; }
        string City { get; set; }
    }
}
