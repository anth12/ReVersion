using System.Linq;
using ReVersion.ViewModels.Home;
using ReVersion.ViewModels.Settings;

namespace ReVersion.Models.Home
{
    public class HomeModel : BaseModel
    {
        public HomeModel(HomeViewModel viewModel)
        {
            search = "";
            this.viewModel = viewModel;
        }

        private HomeViewModel viewModel;


        #region Properties

        private bool loading;
        private string search;
        private bool settingActive;

        public string CountSummary => $"Showing {viewModel.Repositories.Count(r=> r.Model.IsEnabled)} of {viewModel.Repositories.Count()}";
        public string CheckoutSummary => $"Checkout {viewModel.Repositories.Count(r=> r.Model.IsChecked)} " + (viewModel.Repositories.Count(r => r.Model.IsChecked) > 1 ? "repositories" : "repository");
        public int SelectedRepositories => viewModel.Repositories.Count(r=> r.Model.IsChecked);

        public bool Loading
        {
            get { return loading; }
            set { SetField(ref loading, value); }
        }

        public string Search
        {
            get { return search; }
            set { SetField(ref search, value); }
        }
        
        public bool SettingsActive
        {
            get { return settingActive; }
            set { SetField(ref settingActive, value); }
        }

        public bool IsBulkCheckoutButtonActive => viewModel.Repositories.Any(r => r.Model.BulkCheckoutActive && r.Model.IsChecked);
        public bool IsBulkCheckoutActive => viewModel.Repositories.Any(r => r.Model.BulkCheckoutActive);

        #endregion

        #region Business Logic

        public void FilterUpdated()
        {
            OnPropertyChanged(nameof(CountSummary));
        }

        internal void BulkUpdateUpdated()
        {
            OnPropertyChanged(nameof(CheckoutSummary));
            OnPropertyChanged(nameof(SelectedRepositories));
            OnPropertyChanged(nameof(IsBulkCheckoutButtonActive));
        }

        #endregion
    }
}