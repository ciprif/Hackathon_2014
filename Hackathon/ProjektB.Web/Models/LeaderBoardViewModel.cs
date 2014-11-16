using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektB.Web.Models
{
    public class LeaderBoardViewModel
    {
        public IEnumerable<IGrouping<string,ActivityViewModel>> TeamActivities{ get; set; }
    }
}