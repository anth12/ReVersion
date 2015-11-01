using System.Collections.ObjectModel;
using ReVersion.Services.SvnServer.Response;

namespace ReVersion.Models
{
    public class HomeModel : BaseModel
    {
        private ObservableCollection<RepositoryResult> _FilteredRepositories =
            new ObservableCollection<RepositoryResult>();

        private bool _Loading;
        private ObservableCollection<RepositoryResult> _Repositories = new ObservableCollection<RepositoryResult>();
        private string _Search = "";

        public string Search
        {
            get { return _Search; }
            set
            {
                _Search = value;
                OnPropertyChanged();
            }
        }

        public bool Loading
        {
            get { return _Loading; }
            set
            {
                _Loading = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<RepositoryResult> Repositories
        {
            get { return _Repositories; }
            set
            {
                _Repositories = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<RepositoryResult> FilteredRepositories
        {
            get { return _FilteredRepositories; }
            set
            {
                _FilteredRepositories = value;
                OnPropertyChanged();
            }
        }

        public string CountSummary => $"Showing {FilteredRepositories.Count} of {Repositories.Count}";
    }
}