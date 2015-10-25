using System;
using System.IO;
using Newtonsoft.Json;
using ReVersion.Models;

namespace ReVersion.Utilities.Helpers
{
    public class AppDataHelper
    {
        public static void SaveJson<TType>(string fileName, TType data) where TType : class
        {
            var filePath = FilePath(fileName);

            var json = JsonConvert.SerializeObject(data);

            File.WriteAllText(filePath, json);
        }

        public static TType LoadJson<TType>(string fileName) where TType : class
        {
            var filePath = FilePath(fileName);

            if (!File.Exists(filePath))
                return null;

            var json = File.ReadAllText(filePath);

            if (string.IsNullOrEmpty(json))
                return null;
            try
            {
                return JsonConvert.DeserializeObject<TType>(json);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string FilePath(string fileName)
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            if (!Directory.Exists(appDataPath + "\\ReVersion\\"))
            {
                Directory.CreateDirectory(appDataPath + "\\ReVersion\\");
            }

            return appDataPath + $"\\ReVersion\\{fileName}.json";
        }
    }
}
