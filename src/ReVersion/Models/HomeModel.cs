using System.Collections.ObjectModel;
using ReVersion.Services.SvnServer.Response;

namespace ReVersion.Models
{
    public class HomeModel : BaseModel
    {

        private string _Search = "";
        public string Search { get { return _Search; } set { _Search = value; OnPropertyChanged(); } }

        private bool _Loading;
        public bool Loading { get { return _Loading; } set { _Loading = value; OnPropertyChanged(); } }


        private ObservableCollection<RepositoryResult> _Repositories = new ObservableCollection<RepositoryResult>();
        public ObservableCollection<RepositoryResult> Repositories { get { return _Repositories; } set { _Repositories = value; OnPropertyChanged(); } }


        private ObservableCollection<RepositoryResult> _FilteredRepositories = new ObservableCollection<RepositoryResult>();
        public ObservableCollection<RepositoryResult> FilteredRepositories { get { return _FilteredRepositories; } set { _FilteredRepositories = value; OnPropertyChanged(); } }

        public string CountSummary
        {
            get { return $"Showing {FilteredRepositories.Count} of {Repositories.Count}"; }
        }

    }
}
