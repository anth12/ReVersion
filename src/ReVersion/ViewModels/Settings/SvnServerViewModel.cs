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
            RemoveCommand = CommandFromFunction(x => Remove());

        }

        private SettingsViewModel parent { get; set; }

        #region Commands
        public ICommand RemoveCommand { get; set; }
        #endregion

        #region Events
        

        private void Remove()
        {
            parent.Servers.Remove(this);
        }
        
        #endregion

    }
}
