using ReVersion.Services.Settings;
using ReVersion.Utilities.Extensions;

namespace ReVersion.Utilities.Helpers
{
    internal static class DirectoryHelper
    {
        public static string GetRepositoryFolder(string repositoryName, string branch = null)
        {
            var repositoryFolder = SettingsService.Current.CheckoutFolder;

            if (!repositoryFolder.EndsWith("\\"))
                repositoryFolder += "\\";

            repositoryFolder += repositoryName.ToConventionCase(SettingsService.Current.NamingConvention) +
                                $"\\{branch ?? SettingsService.Current.DefaultSvnPath}";
            return repositoryFolder;
        }
    }
}
