using System;
using System.Collections.ObjectModel;
using ReVersion.Helpers;
using ReVersion.Services.SvnServer;

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


        private SvnNamingConvension _NamingConvention;
        public SvnNamingConvension NamingConvention { get { return _NamingConvention; } set { _NamingConvention = value; OnPropertyChanged(); } }


        private DateTime _SvnUpdateDate;
        public DateTime SvnUpdateDate { get { return _SvnUpdateDate; } set { _SvnUpdateDate = value; OnPropertyChanged(); } }
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

    public enum SvnNamingConvension
    {
        /// <summary>
        /// The name of all repositories are kept 'As is'
        /// </summary>
        Asis,

        /// <summary>
        /// The name of all Repositories are converted to upperCamelCase
        /// </summary>
        UpperCamelCase,

        /// <summary>
        /// The name of all Repositories are converted to lowerCamelCase
        /// </summary>
        LowerCamelCase,

        /// <summary>
        /// The name of all Repositories are converted to lower-hyphen-case
        /// </summary>
        LowerHyphenCase,

        /// <summary>
        /// The name of all Repositories are converted to Upper-Hyphen-Case
        /// </summary>
        UpperHyphenCase,

        /// <summary>
        /// The name of all Repositories are converted to lower_underscore_case
        /// </summary>
        LowerUnderscoreCase,

        /// <summary>
        /// The name of all Repositories are converted to Upper_Underscore_Case
        /// </summary>
        UpperUnderscoreCase
    }

}
