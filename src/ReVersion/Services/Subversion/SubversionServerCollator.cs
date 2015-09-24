using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReVersion.Services.Settings;
using ReVersion.Services.Subversion.Response;

namespace ReVersion.Services.Subversion
{
    public class SubversionServerCollator
    {
        public SubversionServerCollator()
        {
            subversionServers = new List<ISubversionServer>
            {
                new SubminServer(),
                new SvenServer()
            };
        }

        private readonly List<ISubversionServer> subversionServers;

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

                    if (subversionServer.CanHandle(svnServerSettings))
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
