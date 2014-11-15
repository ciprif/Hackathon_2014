using ProjektB.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektB.Web.SyncModules
{
    public interface ISyncModule
    {
        Task Sync();
    }
}
