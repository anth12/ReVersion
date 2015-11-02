using System.Collections.ObjectModel;
using ReVersion.Services.SvnServer.Response;

namespace ReVersion.Models.Home
{
    public class HomeModel : BaseModel
    {
        public HomeModel()
        {
            search = "";
            repositories = new ObservableCollection<RepositoryResult>();
            filteredRepositories = new ObservableCollection<RepositoryResult>();
        }

        #region Properties

        private bool loading;
        private string search;
        private ObservableCollection<RepositoryResult> filteredRepositories;
        private ObservableCollection<RepositoryResult> repositories;


        public string CountSummary => $"Showing {FilteredRepositories.Count} of {Repositories.Count}";

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

        public ObservableCollection<RepositoryResult> FilteredRepositories
        {
            get { return filteredRepositories; }
            set { SetField(ref filteredRepositories, value); OnPropertyChanged(nameof(CountSummary)); }
        }

        public ObservableCollection<RepositoryResult> Repositories
        {
            get { return repositories; }
            set { SetField(ref repositories, value); }
        }

        #endregion
        
    }
}