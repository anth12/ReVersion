using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using ReVersion.Helpers;
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

        private string _RootPath;
        public string RootPath { get { return _RootPath; } set { _RootPath = value; OnPropertyChanged(); } }
    }

    public class SvnServer : BaseModel
    {

        private string _BaseUrl;
        public string BaseUrl { get { return _BaseUrl; } set { _BaseUrl = value; OnPropertyChanged(); } }

        private SubversionServerType _Type;
        public SubversionServerType Type { get { return _Type; } set { _Type = value; OnPropertyChanged(); } }

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
}
