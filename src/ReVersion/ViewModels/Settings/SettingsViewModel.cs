using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Input;
using Newtonsoft.Json;
using ReVersion.Models.Settings;
using ReVersion.Services.Settings;
using ReVersion.Utilities.Extensions;
using ReVersion.Utilities.Helpers;
using ReVersion.ViewModels;

namespace ReVersion.ViewModels.Settings
{
    public class SettingsViewModel : BaseViewModel<SettingsModel>
    {
        public SettingsViewModel()
        {

            SaveAndCloseCommand = CommandFromFunction(x => SaveAndClose());
            AddServerCommand = CommandFromFunction(x => AddServer());
            RemoveServerCommand = CommandFromFunction(x => RemoveServer());
            CheckoutFolderPickerCommand = CommandFromFunction(x => CheckoutFolderPicker());
        }

        #region Commands
        public ICommand SaveAndCloseCommand { get; set; }
        public ICommand AddServerCommand { get; set; }
        public ICommand RemoveServerCommand { get; set; }

        public ICommand CheckoutFolderPickerCommand { get; set; }


        #region Events
        

        private void SaveAndClose()
        {
            SettingsService.Save();
            NotificationHelper.Show("Settings Updated");
            
            // TODO
            //Close();
        }

        private void AddServer()
        {
            Model.Servers.Add(new SvnServerModel());
        }

        private void RemoveServer()
        {
            //TODO
        }

        private void CheckoutFolderPicker()
        {
            var folderPicker = new FolderBrowserDialog
            {
                SelectedPath = Model.CheckoutFolder
            };

            var result = folderPicker.ShowDialog();

            if (result == DialogResult.OK)
            {
                Model.CheckoutFolder = folderPicker.SelectedPath;
            }
        }

        #endregion

        #endregion
        
    }
}
