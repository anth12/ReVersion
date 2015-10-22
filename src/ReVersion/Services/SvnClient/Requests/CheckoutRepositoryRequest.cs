
namespace ReVersion.Services.SvnClient.Requests
{
    public class CheckoutRepositoryRequest
    {
        public string ProjectName { get; set; }
        public string SvnServerUrl { get; set; }
    }
}
