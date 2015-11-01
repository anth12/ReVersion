
namespace ReVersion.Services.SvnClient
{
    internal class SvnErrorHandling
    {
        public static string FormatError(string errorMessage)
        {
            if (errorMessage.Contains("E731001"))
                return "Server cannot be reached. Please check your network connection and try again";

            return errorMessage;
        }
    }
}
