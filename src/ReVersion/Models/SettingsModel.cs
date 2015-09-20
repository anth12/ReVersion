using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReVersion.Services.Subversion;

namespace ReVersion.Models
{
    public class SettingsModel : BaseModel
    {
        public SettingsModel()
        {
            Servers = new ObservableCollection<SvnServer>();
        }

        public ObservableCollection<SvnServer> Servers { get; set; }

    }

    public class SvnServer : BaseModel
    {

        private string _BaseUrl;
        public string BaseUrl { get { return _BaseUrl; } set { _BaseUrl = value; OnPropertyChanged(); } }

        private SubversionServerType _Type;
        public SubversionServerType Type { get { return _Type; } set { _Type = value; OnPropertyChanged(); } }
        
    }
}
