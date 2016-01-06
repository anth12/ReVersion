using ReVersion.Models.Settings;
using ReVersion.Services.SvnServer.Response;

namespace ReVersion.Services.SvnServer
{
    internal interface ISvnServer
    {
        SvnServerType ServerType { get; }
        ListRepositoriesResponse ListRepositories(SvnServerModel request);
    }
}