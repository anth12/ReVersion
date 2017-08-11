using ReVersion.Services.ErrorLogging;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnClient;
using ReVersion.Services.SvnClient.Requests;
using ReVersion.Services.SvnServer.Response;
using ReVersion.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReVersion.Services.SvnServer
{
    internal class SvnServerService
    {
        private readonly List<ISvnServer> _subversionServers;

        public SvnServerService()
        {
            var iSvnServerType = typeof (ISvnServer);

            _subversionServers = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.StartsWith("ReVersion"))
                .SelectMany(a => a.GetTypes())
                .Where(t => iSvnServerType.IsAssignableFrom(t) && !t.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<ISvnServer>()
                .ToList();
        }
        

        private readonly object _lock = new object();

        public async Task<ListRepositoriesResponse> ListRepositories(bool forceReload)
        {
            var result = new ListRepositoriesResponse();

            var tasks = SettingsService.Current.Servers.ToList().Select(async svnServerSettings =>
            {
                if (!forceReload && svnServerSettings.RepoUpdateDate > DateTime.Now.AddDays(-7))
                {
                    //Attempt to load the data
                    var repoList = AppDataHelper.LoadJson<List<RepositoryResult>>(svnServerSettings.Id.ToString(),
                        "cache");
                    if (repoList != null && repoList.Any())
                    {
                        lock (_lock)
                        {
                            result.Repositories.AddRange(repoList);
                        }
                        return;
                    }
                }

                var subversionServer = _subversionServers.FirstOrDefault(s => s.ServerType == svnServerSettings.Type);

                try
                {
                    var response = await subversionServer.ListRepositories(svnServerSettings);

                    if (!response.Status)
                    {
                        result.Messages.AddRange(response.Messages);
                    }
                    else
                    {
                        lock (_lock)
                        {
                            result.Repositories.AddRange(response.Repositories);
                        }

                        //Save the results
                        AppDataHelper.SaveJson(svnServerSettings.Id.ToString(), response.Repositories, "cache");
                        svnServerSettings.RepoUpdateDate = DateTime.Now;
                        SettingsService.Save();
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.Log($"Error updating {svnServerSettings.Type} ({svnServerSettings.BaseUrl})", ex);

                    result.Messages.Add(ex.Message);
                }
            });

            await Task.WhenAll(tasks);

            //Order the repo's
            result.Repositories = result.Repositories.OrderBy(x => x.Name).ToList();

            using (var svnClient = new SvnClientService())
            {
                foreach (var repository in result.Repositories)
                {
                    repository.CheckedOut = svnClient.IsCheckedOut(new IsCheckedOutRequest
                    {
                        ProjectName = repository.Name
                    });
                }

                return result;
            }
        }
    }
}