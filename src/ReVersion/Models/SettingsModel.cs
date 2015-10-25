using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;
using ReVersion.Services.SvnServer;
using ReVersion.Utilities.Extensions;
using ReVersion.Utilities.Helpers;

namespace ReVersion.Models
{
    public class SettingsModel : BaseModel
    {
        public SettingsModel()
        {
            Servers = new ObservableCollection<SvnServerModel>();
        }

        public ObservableCollection<SvnServerModel> Servers { get; set; }

        private string _CheckoutFolder;
        public string CheckoutFolder { get { return _CheckoutFolder; } set { _CheckoutFolder = value; OnPropertyChanged(); } }


        private SvnNamingConvention _NamingConvention;
        public SvnNamingConvention NamingConvention { get { return _NamingConvention; } set { _NamingConvention = value; OnPropertyChanged(); OnPropertyChanged(nameof(NamingConventionDescription)); } }

        [JsonIgnore]
        public string NamingConventionDescription => NamingConvention.GetAttributeOfType<DescriptionAttribute>().Description;
        
    }

    public class SvnServerModel : BaseModel
    {
        public SvnServerModel()
        {
            _Id = Guid.NewGuid();
        }

        private Guid _Id;
        public Guid Id { get { return _Id; } set { _Id = value; OnPropertyChanged(); } }

        private string _BaseUrl;
        public string BaseUrl { get { return _BaseUrl; } set { _BaseUrl = value; OnPropertyChanged(); } }

        private SvnServerType _Type;
        public SvnServerType Type { get { return _Type; } set { _Type = value; OnPropertyChanged(); } }

        private string _Username;
        public string Username { get { return _Username; } set { _Username = value; OnPropertyChanged(); } }
        
        public string Password { get; set; }

        protected byte[] Key { get; set; }


        private DateTime _RepoUpdateDate;
        public DateTime RepoUpdateDate { get { return _RepoUpdateDate; } set { _RepoUpdateDate = value; OnPropertyChanged(); } }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public void SetPassword(string password)
        {
            Password = AuthenticationHelper.Encrypt(password);
        }

        public string GetPassword()
        {
            return AuthenticationHelper.Decrypt(Password);
        }
    }

    public enum SvnNamingConvention
    {
        [Description("The name of all repositories is persevered in the Original format")]
        PreserveOriginal,

        [Description("The name of all Repositories are converted to upperCamelCase")]
        UpperCamelCase,
        
        [Description("The name of all Repositories are converted to lowerCamelCase")]
        LowerCamelCase,
        
        [Description("The name of all Repositories are converted to lower-hyphen-case")]
        LowerHyphenCase,
        
        [Description("The name of all Repositories are converted to Upper-Hyphen-Case")]
        UpperHyphenCase,
        
        [Description("The name of all Repositories are converted to lower_underscore_case")]
        LowerUnderscoreCase,
        
        [Description("The name of all Repositories are converted to Upper_Underscore_Case")]
        UpperUnderscoreCase
    }

}
