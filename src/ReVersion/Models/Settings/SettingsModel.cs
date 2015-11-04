using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;
using ReVersion.Utilities.Extensions;

namespace ReVersion.Models.Settings
{
    public class SettingsModel : BaseModel
    {
        public SettingsModel()
        {
            defaultSvnPath = "trunk";
            Servers = new ObservableCollection<SvnServerModel>();
        }

        #region Properties
        private string checkoutFolder;
        private string defaultSvnPath;
        private SvnNamingConvention namingConvention;
        private ObservableCollection<SvnServerModel> servers;


        public ObservableCollection<SvnServerModel> Servers
        {
            get { return servers; }
            set { SetField(ref servers, value); }
        }

        [JsonIgnore]
        public string NamingConventionDescription => NamingConvention.GetAttributeOfType<DescriptionAttribute>().Description;

        public string CheckoutFolder
        {
            get { return checkoutFolder; }
            set { SetField(ref checkoutFolder, value); }
        }

        public string DefaultSvnPath
        {
            get { return defaultSvnPath; }
            set { SetField(ref defaultSvnPath, value); }
        }

        public SvnNamingConvention NamingConvention
        {
            get { return namingConvention; }
            set { SetField(ref namingConvention, value); OnPropertyChanged(nameof(NamingConventionDescription)); }
        }
        
        #endregion
    }
}
