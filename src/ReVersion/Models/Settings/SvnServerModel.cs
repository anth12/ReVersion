using System;
using Newtonsoft.Json;
using ReVersion.Services.SvnServer;
using ReVersion.Utilities.Helpers;

namespace ReVersion.Models.Settings
{
    public class SvnServerModel : BaseModel
    {
        public SvnServerModel()
        {
            id = Guid.NewGuid();
        }

        #region Properties
        private string baseUrl;
        private Guid id;
        private DateTime repoUpdateDate;
        private SvnServerType type;
        private string username;

        public Guid Id
        {
            get { return id; }
            set { SetField(ref id, value); }
        }

        public string BaseUrl
        {
            get { return baseUrl; }
            set { SetField(ref baseUrl, value); }
        }

        public SvnServerType Type
        {
            get { return type; }
            set { SetField(ref type, value); }
        }

        public string Username
        {
            get { return username; }
            set { SetField(ref username, value); }
        }

        
        public string Password { get; set; }

        public DateTime RepoUpdateDate
        {
            get { return repoUpdateDate; }
            set { SetField(ref repoUpdateDate, value); }
        }

        [JsonIgnore]
        public string RawPassword
        {
            get { return new AuthenticationHelper().Decrypt(Password); }
            set { Password = new AuthenticationHelper().Encrypt(Password); }
        }

        #endregion
    }
}
