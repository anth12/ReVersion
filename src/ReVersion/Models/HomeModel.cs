using System.Collections.ObjectModel;
using ReVersion.Services.Subversion.Response;

namespace ReVersion.Models
{
    public class HomeModel : BaseModel
    {

        private string _Search = "";
        public string Search { get { return _Search; } set { _Search = value; OnPropertyChanged(); } }

        private bool _Loaded;
        public bool Loaded { get { return _Loaded; } set { _Loaded = value; OnPropertyChanged(); } }


        private ObservableCollection<RepositoryResult> _Repositories = new ObservableCollection<RepositoryResult>();
        public ObservableCollection<RepositoryResult> Repositories { get { return _Repositories; } set { _Repositories = value; OnPropertyChanged(); } }


        private ObservableCollection<RepositoryResult> _FilteredRepositories = new ObservableCollection<RepositoryResult>();
        public ObservableCollection<RepositoryResult> FilteredRepositories { get { return _FilteredRepositories; } set { _FilteredRepositories = value; OnPropertyChanged(); } }

    }
}
