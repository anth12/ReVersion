using System;
using System.IO;
using System.Threading.Tasks;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnClient.Requests;
using ReVersion.Utilities.Extensions;
using ReVersion.Utilities.Helpers;
using SharpSvn;

namespace ReVersion.Services.SvnClient
{
    public class SvnClientService
    {
        public bool CheckoutRepository(CheckoutRepositoryRequest request)
        {
            var projectFolder = SettingsService.Current.CheckoutFolder;

            if (!projectFolder.EndsWith("\\"))
                projectFolder += "\\";

            projectFolder += request.ProjectName.ToConventionCase(SettingsService.Current.NamingConvention) +
                                $"\\{SettingsService.Current.DefaultSvnPath}";

            if (!Directory.Exists(projectFolder))
            {
                Directory.CreateDirectory(projectFolder);
            }

            request.SvnServerUrl = request.SvnServerUrl.RemoveTrailing('/');

            using (var client = new SharpSvn.SvnClient())
            {
                try
                {
                    client.Authentication.ForceCredentials(request.SvnUsername, request.SvnPassword);
                    var result =
                        client.CheckOut(
                            new SvnUriTarget($"{request.SvnServerUrl}/{SettingsService.Current.DefaultSvnPath}"),
                            projectFolder);
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
    }
}