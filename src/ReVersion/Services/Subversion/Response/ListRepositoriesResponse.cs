using System.Collections.Generic;

namespace ReVersion.Services.Subversion.Response
{
    public class ListRepositoriesResponse : Result
    {
        public List<RepositoryResult> Repositories { get; set; } = new List<RepositoryResult>();
        
    }

    public class RepositoryResult
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
