using System.Linq;
using ReVersion.ViewModels.Home;

namespace ReVersion.Models.Home
{
    public class HomeModel : BaseModel
    {
        public HomeModel(HomeViewModel viewModel)
        {
            _search = "";
            _viewModel = viewModel;
        }

        private readonly HomeViewModel _viewModel;


        #region Properties

        private bool _loading;
        private string _search;
        private bool _settingActive;

        public string CountSummary => $"Showing {_viewModel.Repositories.Count(r=> r.Model.IsEnabled)} of {_viewModel.Repositories.Count()}";
        public string CheckoutSummary => $"Checkout {_viewModel.Repositories.Count(r=> r.Model.IsChecked)} " + (_viewModel.Repositories.Count(r => r.Model.IsChecked) > 1 ? "repositories" : "repository");
        public int SelectedRepositories => _viewModel.Repositories.Count(r=> r.Model.IsChecked);

        public bool Loading
        {
            get { return _loading; }
            set { SetField(ref _loading, value); }
        }

        public string Search
        {
            get { return _search; }
            set { SetField(ref _search, value); }
        }
        
        public bool SettingsActive
        {
            get { return _settingActive; }
            set { SetField(ref _settingActive, value); }
        }

        public bool IsBulkCheckoutButtonActive => _viewModel.Repositories.Any(r => r.Model.BulkCheckoutActive && r.Model.IsChecked);
        public bool IsBulkCheckoutActive => _viewModel.Repositories.Any(r => r.Model.BulkCheckoutActive);

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