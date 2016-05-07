using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnClient.Requests;
using ReVersion.Utilities.Extensions;
using ReVersion.Utilities.Helpers;
using SharpSvn;


namespace ReVersion.Services.SvnClient
{
    internal class SvnClientService
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
            var projectFolder = DirectoryHelper.GetRepositoryFolder(request.ProjectName);

            if (!Directory.Exists(projectFolder))
            {
                Directory.CreateDirectory(projectFolder);
            }

            request.SvnServerUrl = request.SvnServerUrl.RemoveTrailing('/');

            using (var client = new SharpSvn.SvnClient())
            {
                client.Authentication.SslServerTrustHandlers += (b, e) =>
                {
                    e.AcceptedFailures = e.Failures;
                    e.Save = true; // Save acceptance to authentication store
                };

                try
                {
                    if(ClientProgress != null)
                        client.Progress += ClientProgress;
                    
                    var result =
                        client.CheckOut(
                            new SvnUriTarget($"{request.SvnServerUrl}/{SettingsService.Current.DefaultSvnPath}"),
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
                    }
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
                    //When the toas is clicked, open the newly checked out folder
                    Process.Start(projectFolder);
                });
            return true;
            
        }

        public void RepositorySize(GetRepositorySizeRequest request)
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

        public string GetReadMeFile(GetReadMeFileRepositoryRequest request)
        {
            var projectPath = request.SvnServerUrl + "/" + (SettingsService.Current.DefaultSvnPath.IsBlank()
                ? "trunk"
                : SettingsService.Current.DefaultSvnPath);
            
            var rootFiles = new List<string>();

            using (var client = new SharpSvn.SvnClient())
            {
                client.Authentication.SslServerTrustHandlers += (b, e) =>
                {
                    e.AcceptedFailures = e.Failures;
                    e.Save = true; // Save acceptance to authentication store
                };
                client.Authentication.ForceCredentials(request.SvnUsername, request.SvnPassword);

                Collection<SvnListEventArgs> list;
                
                if (client.GetList(projectPath, out list))
                {
                    rootFiles.AddRange(list.Select(item => item.Name));
                }
            }

            var readmeFileName = rootFiles.FirstOrDefault(file => file.ToLower().Contains("read"));

            if(readmeFileName.IsBlank())
                return null;

            var readMeFilePath = $"{projectPath}/{readmeFileName}";

            var readMeUri = new Uri(readMeFilePath);
            var stream = new MemoryStream();
            using (var client = new SharpSvn.SvnClient())
            {
                client.Authentication.SslServerTrustHandlers += (b, e) =>
                {
                    e.AcceptedFailures = e.Failures;
                    e.Save = true; // Save acceptance to authentication store
                };

                client.Authentication.ForceCredentials(request.SvnUsername, request.SvnPassword);
                var target = SvnTarget.FromUri(readMeUri);
                client.Write(target, stream);
            }
            
            byte[] bytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(bytes, 0, (int)stream.Length);
            return Encoding.ASCII.GetString(bytes);
        }
        
    }
}