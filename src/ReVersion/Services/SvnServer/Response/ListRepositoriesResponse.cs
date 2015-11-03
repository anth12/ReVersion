using System;
using System.Collections.Generic;

namespace ReVersion.Services.SvnServer.Response
{
    public class ListRepositoriesResponse : Result
    {
        public List<RepositoryResult> Repositories { get; set; } = new List<RepositoryResult>();
    }

    public class RepositoryResult
    {
        public bool CheckedOut { get; set; }
        public Guid SvnServerId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}