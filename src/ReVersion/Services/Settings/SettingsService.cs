using System;
using System.Collections.Specialized;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using ReVersion.Models;

namespace ReVersion.Services.Settings
{
    public class SettingsService
    {
        static SettingsService()
        {
            Load();

            Current.PropertyChanged += Current_PropertyChanged;
            Current.Servers.CollectionChanged += ServersOnCollectionChanged;
        }

        private static void ServersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            Save();
        }

        private static void Current_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Save();
        }

        public static SettingsModel Current { get; set; }

        public static void Import(string importPath)
        {
            //TODO validate the import
            File.Copy(importPath, SettingsPath);
            Load();
        }

        public static void Export(string exportPath)
        {
            File.Copy(SettingsPath, exportPath);
        }

        public static void Load()
        {
            if (File.Exists(SettingsPath))
            {
                var json = File.ReadAllText(SettingsPath);

                try
                {
                    Current = JsonConvert.DeserializeObject<SettingsModel>(json);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error: Your settings file appears to be corrupt");
                }
            }
            else
            {
                Current = new SettingsModel();
            }
        }

        public static void Save()
        {
            var json = JsonConvert.SerializeObject(Current);

            File.WriteAllText(SettingsPath, json);
        }

        private static string SettingsPath
        {
            get
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                if (!Directory.Exists(appDataPath + "\\ReVersion\\"))
                {
                    Directory.CreateDirectory(appDataPath + "\\ReVersion\\");
                }
                return appDataPath + "\\ReVersion\\settings.json";
            }
        }
    }
}
