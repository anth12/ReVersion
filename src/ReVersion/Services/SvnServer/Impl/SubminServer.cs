using ReVersion.Models.Settings;
using ReVersion.Services.SvnServer.Response;
using ReVersion.Utilities.Helpers;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ReVersion.Services.SvnServer.Impl
{
    internal class SubminServer : ISvnServer
    {
        public SvnServerType ServerType { get; } = SvnServerType.Submin;

        public async Task<ListRepositoriesResponse> ListRepositories(SvnServerModel request)
        {
            var result = new ListRepositoriesResponse {Status = true};

            //Login
            //svn url = requet.Url + "svn/";

            using (var wb = new WebClientSession())
            {
                var loginData = new NameValueCollection
                {
                    ["username"] = request.Username,
                    ["password"] = request.RawPassword
                };

                //Do the login
                await wb.PostAsync(request.BaseUrl + "/submin/login", loginData);

                var repoListData = new NameValueCollection
                {
                    ["ajax"] = "",
                    ["listRepositories"] = ""
                };

                var repoListResponse = await wb.PostAsync(request.BaseUrl + "/submin/x", repoListData);

                if (repoListResponse == null)
                {
                    result.Status = false;
                    return result;
                }

                var serializer = new XmlSerializer(typeof (Response));
                var response = (Response) serializer.Deserialize(repoListResponse.GetResponseStream());

                result.Repositories.AddRange(response.Command.Repositories.Repository.Select(r => new RepositoryResult
                {
                    SvnServerId = request.Id,
                    Name = r.Name,
                    Url = request.BaseUrl + "/svn/" + r.Name
                }));
            }

            return result;
        }

        #region XML response objects (for mapping)

        [XmlRoot(ElementName = "repository")]
        internal class Repository
        {
            [XmlAttribute(AttributeName = "valid")]
            public string Valid { get; set; }

            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "repositories")]
        internal class Repositories
        {
            [XmlElement(ElementName = "repository")]
            public List<Repository> Repository { get; set; }
        }

        [XmlRoot(ElementName = "command")]
        internal class Command
        {
            [XmlElement(ElementName = "repositories")]
            public Repositories Repositories { get; set; }

            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }

            [XmlAttribute(AttributeName = "success")]
            public string Success { get; set; }
        }

        [XmlRoot(ElementName = "response")]
        internal class Response
        {
            [XmlElement(ElementName = "command")]
            public Command Command { get; set; }
        }

        #endregion
    }
}