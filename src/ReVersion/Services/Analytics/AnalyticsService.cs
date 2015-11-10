using System;
using System.Reflection;
using Garlic;

namespace ReVersion.Services.Analytics
{
    public class AnalyticsService
    {
        static AnalyticsService()
        {
            Session = new AnalyticsSession(domain, gaCode);
            Session.SetCustomVariable(1, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        private const string domain = "ReVersion.cloudapp.net";
        private const string gaCode = "UA-69855489-1";

        public static IAnalyticsSession Session;
        
    }
}
