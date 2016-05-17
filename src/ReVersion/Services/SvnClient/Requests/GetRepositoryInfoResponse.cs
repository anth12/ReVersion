using System;

namespace ReVersion.Services.SvnClient.Requests
{
    internal class GetRepositoryInfoResponse
    {
        public string LastChangeAuthor { get; set; }
        public long LastChangeRevision { get; set; }
        public DateTime LastChangeTime { get; set; }
    }
}