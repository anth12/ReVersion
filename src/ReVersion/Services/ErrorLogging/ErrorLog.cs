using System;
using ReVersion.Utilities.Helpers;

namespace ReVersion.Services.ErrorLogging
{
    public static class ErrorLog
    {
        public static void Log(string title)
        {
         Log(title, "");   
        }

        public static void Log(Exception ex)
        {
            Log(ex.Message, ex.ToString());
        }

        public static void Log(string title, Exception ex)
        {
            Log(title, ex.ToString());
        }


        public static void Log(string title, string message)
        {
            var log = $"Title: {title} \nMessage: {message}";

            AppDataHelper.WriteFile(DateTime.Now.ToString("yyyy-MM-dd_mm-ss"), "txt", log, "log");
        }
    }
}
