
namespace ReVersion.Utilities.Helpers
{
    public class AuthenticationHelper
    {
        public static string Encrypt(string value)
        {
            //TODO
            return value + "5";
        }

        public static string Decrypt(string value)
        {
            return value.Substring(0, value.Length - 1);
        }
    }
}
