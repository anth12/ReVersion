namespace ReVersion.Services.SvnServer.Requests
{
    internal class ListRepositoriesRequest
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}