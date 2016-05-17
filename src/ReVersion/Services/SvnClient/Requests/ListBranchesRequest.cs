using System.Net;

namespace ReVersion.Services.SvnClient.Requests
{
    internal class ListBranchesRequest
    {
        public string ProjectName { get; set; }
        public string SvnServerUrl { get; set; }

        public NetworkCredential Credentials { get; set; }
    }
}