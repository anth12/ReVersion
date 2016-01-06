using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using ReVersion.Models.Settings;
using ReVersion.Services.Settings;
using ReVersion.Utilities.Helpers;
using ReVersion.ViewModels.Home;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace ReVersion.ViewModels.Settings
{
    public class SettingsViewModel : BaseViewModel<SettingsModel>
    {
        public SettingsViewModel(HomeViewModel home)
        {
            this._home = home;
            
            Servers = new ObservableCollection<SvnServerViewModel>();
            
            Load();

            SaveAndCloseCommand = CommandFromFunction(x => SaveAndClose());
            AddServerCommand = CommandFromFunction(x => AddServer());
            RemoveServerCommand = CommandFromFunction(x => RemoveServer());
            CheckoutFolderPickerCommand = CommandFromFunction(x => CheckoutFolderPicker());

            ImportCommand = CommandFromFunction(x => Import());
            ExportCommand = CommandFromFunction(x => Export());
        }

        private HomeViewModel _home;

        private ObservableCollection<SvnServerViewModel> _servers;
        public ObservableCollection<SvnServerViewModel> Servers
        {
            get { return _servers; }
            set { SetField(ref _servers, value); }
        }

        #region Commands
        public ICommand SaveAndCloseCommand { get; set; }
        public ICommand AddServerCommand { get; set; }
        public ICommand RemoveServerCommand { get; set; }

        public ICommand ImportCommand { get; set; }
        public ICommand ExportCommand { get; set; }

        public ICommand CheckoutFolderPickerCommand { get; set; }
        #endregion

        #region Events

        private void Load()
        {
            Model = SettingsService.Current;

            _servers.Clear();

            foreach (var server in SettingsService.Current.Servers)
            {
                Servers.Add(new SvnServerViewModel(this)
                {
                    Model = server
                });
            }
        }

        private void Import()
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "Settings Files (.json)|*.json",
                FilterIndex = 1
            };

            var clicked = fileDialog.ShowDialog();

            if (clicked != null && clicked.Value)
            {
                var result = SettingsService.Import(fileDialog.FileName);
                if (result != null)
                    NotificationHelper.ShowResult(result);

                if (result.Status)
                {
                    Load();

                    //When settings are imported, trigger an update
                    _home.SvnRefreshCommand.Execute(null);
                }
            }
        }

        private void Export()
        {
            var fileDialog = new SaveFileDialog
            {
                Filter = "Settings Files (.json)|*.json"
            };

            var clicked = fileDialog.ShowDialog();

            if (clicked != null && clicked.Value)
            {
                SettingsService.Export(fileDialog.FileName);
                NotificationHelper.Show("Settings Exported");
            }
        }

        private void SaveAndClose()
        {
            SettingsService.Current = Model;
            SettingsService.Current.Servers.Clear();
            Servers.ToList().ForEach(s=> SettingsService.Current.Servers.Add(s.Model));

            SettingsService.Save();
            NotificationHelper.Show("Settings Updated");

            _home.OpenSettingsCommand.Execute(null);
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
