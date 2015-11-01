using System.IO;
using ReVersion.Services.Settings;
using ReVersion.Services.SvnClient.Requests;
using ReVersion.Utilities.Extensions;
using ReVersion.Utilities.Helpers;

namespace ReVersion.Services.SvnClient
{
    public class SvnClientService
    {
        public async void CheckoutRepository(CheckoutRepositoryRequest request)
        {
            var projectFolder = SettingsService.Current.CheckoutFolder;

            if (!projectFolder.EndsWith("\\"))
                projectFolder += "\\";

            projectFolder += request.ProjectName.ToConventionCase(SettingsService.Current.NamingConvention) + "\\trunk";
            //TODO make trunk optional

            if (!Directory.Exists(projectFolder))
            {
                Directory.CreateDirectory(projectFolder);
            }

            request.SvnServerUrl = request.SvnServerUrl.RemoveTrailing('/');

            var bat =
                $"svn checkout {request.SvnServerUrl}/trunk \"{projectFolder}\" " +
                $"--username {request.SvnUsername} " +
                $"--password {request.SvnPassword}";
            AppDataHelper.WriteFile("checkout", "bat", bat);
            var filePath = AppDataHelper.FilePath("checkout", "bat");

            string errorResult;
            var successResult = CommandLineHelper.Run(filePath, "", out errorResult);

            if (errorResult.IsNotBlank())
            {
                NotificationHelper.Show(SvnErrorHandling.FormatError(errorResult));
            }
            else
            {
                NotificationHelper.Show($"{request.ProjectName} checked out");
            }
        }
    }
}