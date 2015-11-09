using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using ReVersion.Models.Settings;
using ReVersion.Services.Settings;
using ReVersion.Utilities.Helpers;

namespace ReVersion.ViewModels.Settings
{
    public class SettingsViewModel : BaseViewModel<SettingsModel>
    {
        public SettingsViewModel()
        {
            //TODO when closing, saettings need to be loaded from disk to override any in-memory changes bound from the UI
            Model = new SettingsModel
            {
                CheckoutFolder = SettingsService.Current.CheckoutFolder,
                DefaultSvnPath = SettingsService.Current.DefaultSvnPath,
                NamingConvention = SettingsService.Current.NamingConvention
            };

            Servers = new ObservableCollection<SvnServerViewModel>();

            foreach (var server in SettingsService.Current.Servers)
            {
                Servers.Add(new SvnServerViewModel(this)
                {
                    Model = server
                });
            }

            SaveAndCloseCommand = CommandFromFunction(x => SaveAndClose());
            AddServerCommand = CommandFromFunction(x => AddServer());
            RemoveServerCommand = CommandFromFunction(x => RemoveServer());
            CheckoutFolderPickerCommand = CommandFromFunction(x => CheckoutFolderPicker());
        }

        private ObservableCollection<SvnServerViewModel> servers;
        public ObservableCollection<SvnServerViewModel> Servers
        {
            get { return servers; }
            set { SetField(ref servers, value); }
        }

        #region Commands
        public ICommand SaveAndCloseCommand { get; set; }
        public ICommand AddServerCommand { get; set; }
        public ICommand RemoveServerCommand { get; set; }

        public ICommand CheckoutFolderPickerCommand { get; set; }
        #endregion

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
            Servers.Add(new SvnServerViewModel(this)
            {
                Model = new SvnServerModel()
            });
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
        
        
    }
}
