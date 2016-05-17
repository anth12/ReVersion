using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnClient.Requests;
using ReVersion.Utilities.Extensions;
using ReVersion.Utilities.Helpers;
using SharpSvn;
using System.Diagnostics;
using System.Net;
using ReVersion.Services.ErrorLogging;

namespace ReVersion.Services.SvnClient
{
    internal class SvnClientService : IDisposable
    {
        public event EventHandler<SvnProgressEventArgs> ClientProgress;

        public bool IsCheckedOut(IsCheckedOutRequest request)
        {
            var projectFolder = DirectoryHelper.GetRepositoryFolder(request.ProjectName);

            return Directory.Exists(projectFolder);
            // TODO we are assuming that because the folder exists the repo must be there... 
            // needs to be verified (checkout can fail or be removed)
        }

        public bool CheckoutRepository(CheckoutRepositoryRequest request)
        {
            var projectFolder = DirectoryHelper.GetRepositoryFolder(request.ProjectName, request.Branch);

            if (!Directory.Exists(projectFolder))
            {
                Directory.CreateDirectory(projectFolder);
            }

            request.SvnServerUrl = request.SvnServerUrl.RemoveTrailing('/');

            using (var client = createClient())
            {

                try
                {
                    if(ClientProgress != null)
                        client.Progress += ClientProgress;

                    var repositoryUri =
                        $"{request.SvnServerUrl}/";

                    if (request.Branch.IsNotBlank())
                    {
                        Collection<SvnListEventArgs> rootList;

                        if (!client.GetList(request.SvnServerUrl, out rootList))
                            return false;
                        
                        repositoryUri = request.SvnServerUrl + "/" +
                                          rootList.First(f => f.Name.ToLower().StartsWith("branch"))
                                          .Name + "/" + request.Branch;
                    }
                    else
                    {

                        repositoryUri += SettingsService.Current.DefaultSvnPath;
                    }

                    var result =
                        client.CheckOut(
                            new SvnUriTarget(repositoryUri),
                            projectFolder);
                }
                catch (SvnRepositoryIOException ex)
                {
                    if (ex.SvnErrorCode == SvnErrorCode.SVN_ERR_RA_ILLEGAL_URL)
                    {
                        //default path does not exist, checkout the root
                        var result =
                        client.CheckOut(
                            new SvnUriTarget($"{request.SvnServerUrl}"),
                            projectFolder);
                        return result;
                    }

                    if (ex.SvnErrorCode == SvnErrorCode.SVN_ERR_RA_CANNOT_CREATE_SESSION)
                    {
                        request.Credentials = RequestCredentials(request.SvnServerUrl);

                        if (request.Credentials != null)
                        {
                            return CheckoutRepository(request);
                        }
                    }

                    NotificationHelper.Show(SvnErrorHandling.FormatError(ex.Message));
                    return false;
                }
                catch (Exception ex)
                {
                    NotificationHelper.Show(SvnErrorHandling.FormatError(ex.Message));
                    return false;
                }
            }

            NotificationHelper.Show($"{request.ProjectName} checked out",
                onActivate: (sender, args) =>
                {
                    //When the toast is clicked, open the newly checked out folder
                    Process.Start(projectFolder);
                });
            return true;
            
        }

        internal void RepositorySize(GetRepositorySizeRequest request)
        {
            /*  --- Output:
                Count    : 2686
                Average  :
                Sum      : 131446674
                Maximum  :
                Minimum  :
                Property : size

            */

            var command = $"powershell \"([xml](svn list --xml --recursive {request.SvnServerUrl}/{SettingsService.Current.DefaultSvnPath})).lists.list.entry | measure -sum size\"";
            
            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            var process = Process.Start(processInfo);

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
            {

                if (e != null && e.Data.IsNotBlank() && e.Data.Contains("Sum"))
                {
                    var size = long.Parse(e.Data.Split(':').Last());
                    request.RepositorySize.Invoke(size);
                }
            };

            process.BeginOutputReadLine();
            
            process.BeginErrorReadLine();

            process.WaitForExit();
            
            process.Close();
        }

        public List<string> ListBranches(ListBranchesRequest request)
        {
            try
            {
                var rootFiles = new List<string>();

                using (var client = createClient())
                {
                    Collection<SvnListEventArgs> rootList;

                    if (!client.GetList(request.SvnServerUrl, out rootList))
                        return new List<string>();

                    var projectPath = request.SvnServerUrl + "/" +
                                      rootList.First(f => f.Name.ToLower().StartsWith("branch"))
                                      .Name;

                    Collection<SvnListEventArgs> list;

                    if (client.GetList(projectPath, out list))
                    {
                        rootFiles.AddRange(
                            list.Skip(1).Select(item => item.Name)
                            );
                    }
                }

                return rootFiles;
            }
            catch (SvnRepositoryIOException ex)
            {
                if (ex.SvnErrorCode == SvnErrorCode.SVN_ERR_RA_CANNOT_CREATE_SESSION)
                {
                    request.Credentials = RequestCredentials(request.SvnServerUrl);

                    if (request.Credentials != null)
                    {
                        return ListBranches(request);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog.Log("ListBranches:", ex);
            }

            return new List<string>();
        }

        public string GetReadMeFile(GetReadMeFileRepositoryRequest request)
        {
            var projectPath = request.SvnServerUrl + "/" + (SettingsService.Current.DefaultSvnPath.IsBlank()
                ? "trunk"
                : SettingsService.Current.DefaultSvnPath);
            
            var rootFiles = new List<string>();

            using (var client = createClient())
            {
                Collection<SvnListEventArgs> list;

                try
                {
                    if (client.GetList(projectPath, out list))
                    {
                        rootFiles.AddRange(list.Select(item => item.Name));
                    }
                }
                catch (SvnRepositoryIOException ex)
                {
                    if (ex.SvnErrorCode == SvnErrorCode.SVN_ERR_RA_CANNOT_CREATE_SESSION)
                    {
                        request.Credentials = RequestCredentials(request.SvnServerUrl);

                        if (request.Credentials != null)
                        {
                            return GetReadMeFile(request);
                        }
                    }

                }
                catch (Exception ex)
                {
                    ErrorLog.Log("Get Readme", ex);
                }
            }

            var readmeFileName = rootFiles.FirstOrDefault(file => file.ToLower().Contains("read"));

            if(readmeFileName.IsBlank())
                return null;

            var readMeFilePath = $"{projectPath}/{readmeFileName}";

            var readMeUri = new Uri(readMeFilePath);
            var stream = new MemoryStream();

            using (var client = createClient())
            {
                var target = SvnTarget.FromUri(readMeUri);
                client.Write(target, stream);
            }
            
            byte[] bytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(bytes, 0, (int)stream.Length);
            return Encoding.ASCII.GetString(bytes);
        }

        public GetRepositoryInfoResponse GetInfo(GetRepositoryInfoRequest request)
        {
            var projectPath = request.SvnServerUrl + "/" + (SettingsService.Current.DefaultSvnPath.IsBlank()
                ? "trunk"
                : SettingsService.Current.DefaultSvnPath);

            using (var client = createClient(request.Credentials))
            {
                try
                {
                    Collection<SvnInfoEventArgs> info;
                    client.GetInfo(
                        new SvnUriTarget(projectPath),
                        new SvnInfoArgs
                        {

                        }, out info);


                    if (info != null && info.Any())
                    {
                        return new GetRepositoryInfoResponse
                        {
                            LastChangeAuthor = info.First().LastChangeAuthor,
                            LastChangeRevision = info.First().LastChangeRevision,
                            LastChangeTime = info.First().LastChangeTime,
                        };
                    }

                }
                catch (SvnRepositoryIOException ex)
                {
                    if (ex.SvnErrorCode == SvnErrorCode.SVN_ERR_RA_CANNOT_CREATE_SESSION)
                    {
                        request.Credentials = RequestCredentials(request.SvnServerUrl);

                        if (request.Credentials != null)
                        {
                            return GetInfo(request);
                        }
                    }

                }
                catch (Exception ex)
                {
                    ErrorLog.Log("Project Info", ex);
                    return null;
                }


            }
            return null;
        }

        private SharpSvn.SvnClient createClient(NetworkCredential credentials = null)
        {
            var client = new SharpSvn.SvnClient();

            client.Authentication.SslServerTrustHandlers += (b, e) =>
            {
                e.AcceptedFailures = e.Failures;
                e.Save = true; // Save acceptance to authentication store
            };

            if (credentials != null)
            {
                client.Authentication.ForceCredentials(credentials.UserName, credentials.Password);
            }

            return client;
        }

        private NetworkCredential RequestCredentials(string serverHostName)
        {
            var serverUri = new Uri(serverHostName);
            NetworkCredential credentials;
            CredentialHelper.GetCredentialsVistaAndUp(serverUri.Host, out credentials);

            return credentials;
        }
        
        public void RemoveMasterCredentials()
        {
            var svnPath = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), "Subversion\\auth\\svn.simple\\");

            var files = Directory.GetFiles(svnPath);

            files.Where(fileName =>
            {
                var file = File.ReadAllText(fileName);

                return SettingsService.Current.Servers
                    .Where(s=> s.MasterAccount)
                    .Any(s=> file.Contains(s.Username));
            })
            .ToList()
            .ForEach(File.Delete);
        }

        public void Dispose()
        {
            RemoveMasterCredentials();
        }
    }
}