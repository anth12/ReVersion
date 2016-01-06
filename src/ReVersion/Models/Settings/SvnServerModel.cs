﻿using System;
using Newtonsoft.Json;
using ReVersion.Services.SvnServer;
using ReVersion.Utilities.Helpers;
using ReVersion.Utilities.Extensions;

namespace ReVersion.Models.Settings
{
    internal class SvnServerModel : BaseModel
    {
        public SvnServerModel()
        {
            _id = Guid.NewGuid();
        }

        #region Properties
        private string _baseUrl;
        private Guid _id;
        private DateTime _repoUpdateDate;
        private SvnServerType _type;
        private string _username;

        public Guid Id
        {
            get { return _id; }
            set { SetField(ref _id, value); }
        }

        public string BaseUrl
        {
            get { return _baseUrl; }
            set { SetField(ref _baseUrl, value); }
        }

        public SvnServerType Type
        {
            get { return _type; }
            set { SetField(ref _type, value); }
        }

        public string Username
        {
            get { return _username; }
            set { SetField(ref _username, value); }
        }

        
        public string Password { get; set; }

        public DateTime RepoUpdateDate
        {
            get { return _repoUpdateDate; }
            set { SetField(ref _repoUpdateDate, value); }
        }

        [JsonIgnore]
        public string RawPassword
        {
            get { return Password.IsNotBlank() ? new AuthenticationHelper().Decrypt(Password) : ""; }
            set { Password = value.IsNotBlank() ? new AuthenticationHelper().Encrypt(value) : ""; }
        }

        #endregion
    }
}
