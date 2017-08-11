using ReVersion.Models.Settings;
using ReVersion.Services.SvnServer.Response;
using System.Threading.Tasks;

namespace ReVersion.Services.SvnServer
{
    internal interface ISvnServer
    {
        SvnServerType ServerType { get; }
        Task<ListRepositoriesResponse> ListRepositories(SvnServerModel request);
    }
}