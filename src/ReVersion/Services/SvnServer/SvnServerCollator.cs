using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnServer.Response;
using ReVersion.Services.SvnServer;
using ReVersion.Services.SvnServer.Impl;

namespace ReVersion.Services.SvnServer
{
    public class SvnServerCollator
    {
        public SvnServerCollator()
        {
            subversionServers = new List<ISvnServer>
            {
                new SubminServer(),
                new SvenServer(),
                new VisualSvnServerServer()
            };
        }

        private readonly List<ISvnServer> subversionServers;

        public async Task<ListRepositoriesResponse> ListRepositories()
        {
            return await Task.Run(() => LoadRepositories());
        }

        private ListRepositoriesResponse LoadRepositories()
        {
            var result = new ListRepositoriesResponse();

            foreach (var svnServerSettings in SettingsService.Current.Servers)
            {

                foreach (var subversionServer in subversionServers)
                {

                    if (subversionServer.ServerType == svnServerSettings.Type)
                    {
                        try
                        {
                            var response = subversionServer.ListRepositories(svnServerSettings);

                            if(!response.Status)
                                result.Messages.AddRange(response.Messages);
                            else
                                result.Repositories.AddRange(response.Repositories);

                        }
                        catch (Exception ex)
                        {
                            result.Messages.Add(ex.Message);
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
