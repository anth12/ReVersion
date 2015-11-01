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
        private string _CheckoutFolder;
        private SvnNamingConvention _NamingConvention;

        public SettingsModel()
        {
            Servers = new ObservableCollection<SvnServerModel>();
        }

        public ObservableCollection<SvnServerModel> Servers { get; set; }

        public string CheckoutFolder
        {
            get { return _CheckoutFolder; }
            set
            {
                _CheckoutFolder = value;
                OnPropertyChanged();
            }
        }

        public SvnNamingConvention NamingConvention
        {
            get { return _NamingConvention; }
            set
            {
                _NamingConvention = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NamingConventionDescription));
            }
        }

        [JsonIgnore]
        public string NamingConventionDescription
            => NamingConvention.GetAttributeOfType<DescriptionAttribute>().Description;
    }

    public class SvnServerModel : BaseModel
    {
        private string _BaseUrl;
        private Guid _Id;
        private DateTime _RepoUpdateDate;
        private SvnServerType _Type;
        private string _Username;

        public SvnServerModel()
        {
            _Id = Guid.NewGuid();
        }

        public Guid Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged();
            }
        }

        public string BaseUrl
        {
            get { return _BaseUrl; }
            set
            {
                _BaseUrl = value;
                OnPropertyChanged();
            }
        }

        public SvnServerType Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get { return _Username; }
            set
            {
                _Username = value;
                OnPropertyChanged();
            }
        }

        public string Password { get; set; }
        protected byte[] Key { get; set; }

        public DateTime RepoUpdateDate
        {
            get { return _RepoUpdateDate; }
            set
            {
                _RepoUpdateDate = value;
                OnPropertyChanged();
            }
        }

        private static string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length/sizeof (char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public void SetPassword(string password)
        {
            Password = (new AuthenticationHelper()).Encrypt(password);
        }

        public string GetPassword()
        {
            return (new AuthenticationHelper()).Decrypt(Password);
        }
    }

    public enum SvnNamingConvention
    {
        [Description("The name of all repositories is persevered in the Original format")] PreserveOriginal,

        [Description("The name of all Repositories are converted to upperCamelCase")] UpperCamelCase,

        [Description("The name of all Repositories are converted to lowerCamelCase")] LowerCamelCase,

        [Description("The name of all Repositories are converted to lower-hyphen-case")] LowerHyphenCase,

        [Description("The name of all Repositories are converted to Upper-Hyphen-Case")] UpperHyphenCase,

        [Description("The name of all Repositories are converted to lower_underscore_case")] LowerUnderscoreCase,

        [Description("The name of all Repositories are converted to Upper_Underscore_Case")] UpperUnderscoreCase
    }
}