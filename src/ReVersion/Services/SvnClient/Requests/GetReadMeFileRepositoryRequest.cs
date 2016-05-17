﻿using System.Net;

namespace ReVersion.Services.SvnClient.Requests
{
    internal class GetReadMeFileRepositoryRequest
    {
        public string ProjectName { get; set; }
        public string SvnServerUrl { get; set; }

        public NetworkCredential Credentials { get; set; }
    }
}