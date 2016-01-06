using System;
using System.Reflection;
using Garlic;

namespace ReVersion.Services.Analytics
{
    public class AnalyticsService
    {
        static AnalyticsService()
        {
            Session = new AnalyticsSession(Domain, GaCode);
            Session.SetCustomVariable(1, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        private const string Domain = "ReVersion.cloudapp.net";
        private const string GaCode = "UA-69855489-1";

        public static IAnalyticsSession Session;
        
    }
}
