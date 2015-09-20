using System.Collections.Generic;
using ReVersion.Services.Subversion;

namespace ReVersion.Services.Settings
{
    public class SettingsModel
    {
        public List<SvnServer> Servers { get; set; }
    }

    public class SvnServer
    {
        public SubversionServerType Type { get; set; }
        public string BaseUrl { get; set; }
    }
}
