namespace ReVersion.Services.SvnClient.Requests
{
    internal class ListBranchesRequest
    {
        public string SvnUsername { get; set; }
        public string SvnPassword { get; set; }
        public string ProjectName { get; set; }
        public string SvnServerUrl { get; set; }
    }
}