using System.Net;

namespace ReVersion.Services.SvnClient.Requests
{
    internal class CheckoutRepositoryRequest
    {
        public string ProjectName { get; set; }
        public string SvnServerUrl { get; set; }
        public string Branch { get; set; }

        public NetworkCredential Credentials { get; set; }
    }
}