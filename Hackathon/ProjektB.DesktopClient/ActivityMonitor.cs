using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Runtime.InteropServices;
using System.Net.Http;
using System.Windows.Forms;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace ProjektB.DesktopClient
{
    /// <summary>
    /// Does the job of checking the user activity on the PC
    /// </summary>
    class ActivityMonitor
    {
        #region unmanaged code

        [DllImport("User32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        internal struct LASTINPUTINFO
        {
            public uint cbSize;

            public uint dwTime;
        }

        #endregion unmanaged code

        // we want to start a counter of the user activity on the PC
        // don't start the monitor if the user has been innactive
        // reset the timer if the user has been innactive 
        System.Timers.Timer t;

        private int activityMinutes;
        private int inactiviyMinutes;
        private int askToMoveAfterMin;
        private string apiUrl;
        private string userKey;

        public ActivityMonitor()
        {
            askToMoveAfterMin = int.Parse(ConfigurationManager.AppSettings.Get("showToMoveAfterMin"));
            apiUrl = ConfigurationManager.AppSettings.Get("apiUrl");
            userKey = ConfigurationManager.AppSettings.Get("userKey");

            t = new System.Timers.Timer();
            //set the timer interval at 5 min
            t.Interval = 5 * 60 * 1000;
            t.Elapsed += TimerElapsed;
            t.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            bool innactiveForTheLast5Min = false;

            LASTINPUTINFO lastInput = new LASTINPUTINFO();
            GetLastInputInfo(ref lastInput);

            

            if (lastInput.dwTime / 60.0 * 1000 >= 5)
            {
                inactiviyMinutes += 5;
                innactiveForTheLast5Min = true;
            }
            else
            {
                // there was some activiy in the last 5 mins, reset the
                // innactivity counter
                inactiviyMinutes = 0;
                activityMinutes += 5;
            }

            //if there were 20 minutes of inactivity, reset the activity counter
            if (inactiviyMinutes >= 20)
            {
                activityMinutes = 0;
            }

            if (!innactiveForTheLast5Min && activityMinutes >= askToMoveAfterMin)
            {
                HttpClient client = new HttpClient();
                client.GetAsync(apiUrl + userKey)
                    .ContinueWith(x => x.Result.Content.ReadAsStringAsync()
                        .ContinueWith(y=>
                        {
                            string webResponse = y.Result;
                            JObject r = JObject.Parse(webResponse);
                            bool needsMovement = r.Value<bool>("needsMovement");

                            if (needsMovement)
                            {
                                if (MessageBox.Show("Hey there! Looks like you haven't done much movement in the past hour. How about you take a break.", "KernelFit", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {
                                    //reset activity minutes
                                    activityMinutes = 0;
                                }

                            }

                        }));
            }
        }
    }
}
