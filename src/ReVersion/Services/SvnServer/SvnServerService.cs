using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnServer.Response;
using ReVersion.Utilities.Helpers;

namespace ReVersion.Services.SvnServer
{
    public class SvnServerService
    {
        private readonly List<ISvnServer> subversionServers;

        public SvnServerService()
        {
            var iSvnServerType = typeof (ISvnServer);

            subversionServers = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.StartsWith("ReVersion"))
                .SelectMany(a => a.GetTypes())
                .Where(t => iSvnServerType.IsAssignableFrom(t) && !t.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<ISvnServer>()
                .ToList();
        }

        public async Task<ListRepositoriesResponse> ListRepositories(bool forceReload = false)
        {
            return await Task.Run(() => LoadRepositories(forceReload));
        }

        private ListRepositoriesResponse LoadRepositories(bool forceReload)
        {
            var result = new ListRepositoriesResponse();

            foreach (var svnServerSettings in SettingsService.Current.Servers)
            {
                if (!forceReload && svnServerSettings.RepoUpdateDate > DateTime.Now.AddDays(-7))
                {
                    //Attempt to load the data
                    var repoList = AppDataHelper.LoadJson<List<RepositoryResult>>(svnServerSettings.Id.ToString(),
                        "cache");
                    if (repoList != null && repoList.Any())
                    {
                        result.Repositories.AddRange(repoList);
                        continue;
                    }
                }

                foreach (var subversionServer in subversionServers)
                {
                    if (subversionServer.ServerType == svnServerSettings.Type)
                    {
                        try
                        {
                            var response = subversionServer.ListRepositories(svnServerSettings);

                            if (!response.Status)
                            {
                                result.Messages.AddRange(response.Messages);
                            }
                            else
                            {
                                result.Repositories.AddRange(response.Repositories);

                                //Save the results
                                AppDataHelper.SaveJson(svnServerSettings.Id.ToString(), response.Repositories, "cache");
                                svnServerSettings.RepoUpdateDate = DateTime.Now;
                                SettingsService.Save();
                            }
                        }
                        catch (Exception ex)
                        {
                            result.Messages.Add(ex.Message);

#if DEBUG
                            //throw ex;
#endif
                        }
                    }
                }
            }

            //Order the repo's
            result.Repositories = result.Repositories.OrderBy(x => x.Name).ToList();

            return result;
        }
    }
}