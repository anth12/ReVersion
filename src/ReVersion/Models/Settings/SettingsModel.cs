using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;
using ReVersion.Utilities.Extensions;

namespace ReVersion.Models.Settings
{
    internal class SettingsModel : BaseModel
    {
        public SettingsModel()
        {
            _defaultSvnPath = "trunk";
            Servers = new ObservableCollection<SvnServerModel>();
        }

        #region Properties
        private string _checkoutFolder;
        private string _defaultSvnPath;
        private SvnNamingConvention _namingConvention;
        private ObservableCollection<SvnServerModel> _servers;


        public ObservableCollection<SvnServerModel> Servers
        {
            get { return _servers; }
            set { SetField(ref _servers, value); }
        }

        [JsonIgnore]
        public string NamingConventionDescription => NamingConvention.GetAttributeOfType<DescriptionAttribute>().Description;

        public string CheckoutFolder
        {
            get { return _checkoutFolder; }
            set { SetField(ref _checkoutFolder, value); }
        }

        public string DefaultSvnPath
        {
            get { return _defaultSvnPath; }
            set { SetField(ref _defaultSvnPath, value); }
        }

        public SvnNamingConvention NamingConvention
        {
            get { return _namingConvention; }
            set { SetField(ref _namingConvention, value); OnPropertyChanged(nameof(NamingConventionDescription)); }
        }
        
        #endregion
    }
}
