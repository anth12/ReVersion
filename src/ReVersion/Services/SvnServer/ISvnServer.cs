using ReVersion.Models;
using ReVersion.Services.SvnServer.Response;

namespace ReVersion.Services.SvnServer
{
    public interface ISvnServer
    {
        bool CanHandle(SvnServerModel serverSettings);

        ListRepositoriesResponse ListRepositories(SvnServerModel request);
    }
}
