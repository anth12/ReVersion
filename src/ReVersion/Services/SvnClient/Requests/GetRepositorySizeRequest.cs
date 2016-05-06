using System;

namespace ReVersion.Services.SvnClient.Requests
{
    internal class GetRepositorySizeRequest
    {
        public string ProjectName { get; set; }
        public string SvnServerUrl { get; set; }

        public Action<long> RepositorySize { get; set; }
    }
}