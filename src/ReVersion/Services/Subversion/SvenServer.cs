using ReVersion.Models;
using ReVersion.Services.Subversion.Response;

namespace ReVersion.Services.Subversion
{
    public class SvenServer : ISubversionServer
    {
        public bool CanHandle(SvnServerModel serverSettings)
        {
            return serverSettings.Type == SubversionServerType.Sven;
        }

        public ListRepositoriesResponse ListRepositories(SvnServerModel request)
        {
            return new ListRepositoriesResponse { Status = false, Messages = { "Some test fail message" }};
        }
    }
}
