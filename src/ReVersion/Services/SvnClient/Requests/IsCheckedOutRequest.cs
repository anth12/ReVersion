using System.Net;

namespace ReVersion.Services.SvnClient.Requests
{
    internal class IsCheckedOutRequest
    {
        public string ProjectName { get; set; }

        public NetworkCredential Credentials { get; set; }
    }
}