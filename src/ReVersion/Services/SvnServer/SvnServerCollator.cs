using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnServer.Response;
using ReVersion.Services.SvnServer;
using ReVersion.Services.SvnServer.Impl;

namespace ReVersion.Services.SvnServer
{
    internal class SvnServerCollator
    {
        public SvnServerCollator()
        {
            subversionServers = new List<ISvnServer>
            {
                new SubminServer(),
                new SvenServer()
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

            //Load from disk- update weekly
            if (SettingsService.Current.SvnUpdateDate > DateTime.Now.AddDays(-7))
            {
                if (File.Exists(RepositoriesPath))
                {
                    var savedJson = File.ReadAllText(RepositoriesPath);

                    try
                    {
                        result.Repositories = JsonConvert.DeserializeObject<List<RepositoryResult>>(savedJson);
                        result.Status = true;
                        return result;
                    }
                    catch (Exception)
                    {
                        //Swallow silently for now...
                    }
                }
            }

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

            result.Status = !result.Messages.Any();
            result.Repositories = result.Repositories.OrderBy(x => x.Name).ToList();

            //Order the repo's
            if (result.Status)
            {
                //Save & update the update date
                var json = JsonConvert.SerializeObject(result.Repositories);
                File.WriteAllText(RepositoriesPath, json);

                SettingsService.Current.SvnUpdateDate = DateTime.Now;
                SettingsService.Save();
            }

            return result;
        }

        private static string RepositoriesPath
        {
            get
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                if (!Directory.Exists(appDataPath + "\\ReVersion\\"))
                {
                    Directory.CreateDirectory(appDataPath + "\\ReVersion\\");
                }
                return appDataPath + "\\ReVersion\\repositories.json";
            }
        }
    }
}
