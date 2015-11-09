using System;
using System.IO;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnClient.Requests;
using ReVersion.Utilities.Extensions;
using ReVersion.Utilities.Helpers;
using SharpSvn;

namespace ReVersion.Services.SvnClient
{
    public class SvnClientService
    {
        public bool IsCheckedOut(IsCheckedOutRequest request)
        {
            var projectFolder = GetRepositoryFolder(request.ProjectName);

            return Directory.Exists(projectFolder);
            // TODO we are assuming that because the folder exists the repo must be there... 
            // needs to be verified (checkout can fail or be removed)
        }

        public bool CheckoutRepository(CheckoutRepositoryRequest request)
        {
            var projectFolder = GetRepositoryFolder(request.ProjectName);

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

            NotificationHelper.Show($"{request.ProjectName} checked out");
            return true;
            
        }

        private string GetRepositoryFolder(string repositoryName)
        {
            var repositoryFolder = SettingsService.Current.CheckoutFolder;

            if (!repositoryFolder.EndsWith("\\"))
                repositoryFolder += "\\";

            repositoryFolder += repositoryName.ToConventionCase(SettingsService.Current.NamingConvention) +
                                $"\\{SettingsService.Current.DefaultSvnPath}";
            return repositoryFolder;
        }
    }
}