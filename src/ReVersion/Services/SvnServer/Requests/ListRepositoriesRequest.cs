namespace ReVersion.Services.SvnServer.Requests
{
    public class ListRepositoriesRequest
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}