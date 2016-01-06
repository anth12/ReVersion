namespace ReVersion.Services.SvnClient.Requests
{
    internal class CheckoutRepositoryRequest
    {
        public string SvnUsername { get; set; }
        public string SvnPassword { get; set; }
        public string ProjectName { get; set; }
        public string SvnServerUrl { get; set; }
    }
}