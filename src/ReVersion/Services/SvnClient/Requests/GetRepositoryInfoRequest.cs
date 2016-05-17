using System.Net;

namespace ReVersion.Services.SvnClient.Requests
{
    internal class GetRepositoryInfoRequest
    {
        public string ProjectName { get; set; }
        public string SvnServerUrl { get; set; }

        public NetworkCredential Credentials { get; set; }
    }
}