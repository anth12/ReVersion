using ReVersion.ViewModels.Home;
using System.Linq;

namespace ReVersion.Models.Home
{
    internal class HomeModel : BaseModel
    {
        public HomeModel(HomeViewModel viewModel)
        {
            _search = "";
            _viewModel = viewModel;
            _loading = true;
            Window = new WindowModel();
        }

        private readonly HomeViewModel _viewModel;


        #region Properties

        private bool _loading;
        private string _search;
        private bool _settingActive;
        private WindowModel _window;

        public string CountSummary => $"Showing {_viewModel.Repositories.Count(r=> r.Model.IsEnabled)} of {_viewModel.Repositories.Count()}";
        public string CheckoutSummary => $"Checkout {_viewModel.Repositories.Count(r=> r.Model.IsChecked)} " + (_viewModel.Repositories.Count(r => r.Model.IsChecked) > 1 ? "repositories" : "repository");
        public int SelectedRepositories => _viewModel.Repositories.Count(r=> r.Model.IsChecked);

        public bool MissingData => !_loading && !_viewModel.Repositories.Any();

        public bool Loading
        {
            get => _loading;
            set { SetField(ref _loading, value); OnPropertyChanged(nameof(MissingData)); }
        }

        public string Search
        {
            get => _search;
            set => SetField(ref _search, value);
        }
        
        public bool SettingsActive
        {
            get => _settingActive;
            set => SetField(ref _settingActive, value);
        }
        
        public bool IsBulkCheckoutButtonActive => _viewModel.Repositories.Any(r => r.Model.BulkCheckoutActive && r.Model.IsChecked);
        public bool IsBulkCheckoutActive => _viewModel.Repositories.Any(r => r.Model.BulkCheckoutActive);


        public WindowModel Window
        {
            get => _window;
            set => SetField(ref _window, value);
        }
        

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