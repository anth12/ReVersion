using System.Windows.Forms;
using System.Windows.Input;
using ReVersion.Models.Settings;

namespace ReVersion.ViewModels.Settings
{
    public class SvnServerViewModel : BaseViewModel<SvnServerModel>
    {
        public SvnServerViewModel(SettingsViewModel parent)
        {
            this.parent = parent;
            RemoveServerCommand = CommandFromFunction(x => RemoveServer());

        }

        private SettingsViewModel parent { get; set; }

        #region Commands
        public ICommand RemoveServerCommand { get; set; }
        #endregion

        #region Events
        

        private void RemoveServer()
        {
            parent.Servers.Remove(this);
        }
        
        #endregion

    }
}
