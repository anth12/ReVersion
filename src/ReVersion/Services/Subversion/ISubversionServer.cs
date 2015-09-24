using ReVersion.Models;
using ReVersion.Services.Subversion.Response;

namespace ReVersion.Services.Subversion
{
    public interface ISubversionServer
    {
        bool CanHandle(SvnServerModel serverSettings);

        ListRepositoriesResponse ListRepositories(SvnServerModel request);
    }
}
