using System;
using System.IO;
using Newtonsoft.Json;
using ReVersion.Utilities.Extensions;

namespace ReVersion.Utilities.Helpers
{
    internal class AppDataHelper
    {
        public static void WriteFile(string fileName, string fileType, string contents, string directory = "")
        {
            var filePath = FilePath(fileName, fileType, directory);
            File.WriteAllText(filePath, contents);
        }

        public static void SaveJson<TType>(string fileName, TType data, string directory = "") where TType : class
        {
            var filePath = FilePath(fileName, directory: directory);

            var json = JsonConvert.SerializeObject(data);

            File.WriteAllText(filePath, json);
        }

        public static TType LoadJson<TType>(string fileName, string directory = "") where TType : class
        {
            var filePath = FilePath(fileName, directory: directory);

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

        public static string FilePath(string fileName, string fileType = "json", string directory = "")
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            appDataPath = $"{appDataPath}\\ReVersion";

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            if (directory.IsNotBlank())
            {
                appDataPath = $"{appDataPath}\\{directory}";
                if (!Directory.Exists(appDataPath))
                {
                    Directory.CreateDirectory(appDataPath);
                }
            }

            return $"{appDataPath}\\{fileName}.{fileType}";
        }
    }
}