using System;
using System.Collections.Specialized;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using ReVersion.Models;
using ReVersion.Utilities.Helpers;

namespace ReVersion.Services.Settings
{
    public class SettingsService
    {
        static SettingsService()
        {
            Load();
        }
        
        public static SettingsModel Current { get; set; }

        public static Result Import(string importPath)
        {
            //validate the import
            if (!File.Exists(importPath))
                return Result.Error("File does not exist");

            var content = File.ReadAllText(importPath);

            try
            {
                JsonConvert.DeserializeObject<SettingsModel>(content);
            }
            catch (Exception)
            {
                return Result.Error("The file is not in the correct format");
            }

            File.Delete(AppDataHelper.FilePath("settings"));
            File.Copy(importPath, AppDataHelper.FilePath("settings"));
            Load();

            return Result.Success("Import successfull");
        }

        public static void Export(string exportPath)
        {
            File.Copy(AppDataHelper.FilePath("settings"), exportPath);
        }

        public static void Load()
        {
            Current = AppDataHelper.LoadJson<SettingsModel>("settings") ?? new SettingsModel();
        }

        public static void Save()
        {
            AppDataHelper.SaveJson("settings", Current);
        }
        
    }
}
