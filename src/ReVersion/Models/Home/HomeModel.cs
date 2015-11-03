using System.Linq;
using ReVersion.ViewModels.Home;

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


        public string CountSummary => $"Showing {viewModel.Repositories.Count(r=> r.Model.IsEnabled)} of {viewModel.Repositories.Count()}";

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
        
        #endregion

        #region Business Logic

        public void FilterUpdated()
        {
            OnPropertyChanged(nameof(CountSummary));
        }

        #endregion
    }
}