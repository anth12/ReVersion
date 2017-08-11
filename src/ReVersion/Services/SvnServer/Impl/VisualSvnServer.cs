using ReVersion.Models.Settings;
using ReVersion.Services.SvnServer.Response;
using ReVersion.Utilities.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ReVersion.Services.SvnServer.Impl
{
    public class VisualSvnServerServer : ISvnServer
    {
        public SvnServerType ServerType { get; } = SvnServerType.VisualSvn;

        public async Task<ListRepositoriesResponse> ListRepositories(SvnServerModel request)
        {
            var result = new ListRepositoriesResponse {Status = true};

            //Login

            using (var wb = new WebClientSession())
            {
                //Load the login page to grab a viewstate
                var homePageResponse = await wb.AuthenticateAsync(request.BaseUrl + "/svn/", request.Username, request.RawPassword);

                var serializer = new XmlSerializer(typeof (Response));
                var response = (Response) serializer.Deserialize(homePageResponse.GetResponseStream());


                foreach (var repoNode in response.Index.Dir)
                {
                    result.Repositories.Add(new RepositoryResult
                    {
                        SvnServerId = request.Id,
                        Name = repoNode.Name,
                        Url = $"{request.BaseUrl}/svn/{repoNode.Href}"
                    });
                }
            }

            return result;
        }

        #region XML response objects (for mapping)

        [XmlRoot(ElementName = "dir")]
        public class Dir
        {
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }

            [XmlAttribute(AttributeName = "href")]
            public string Href { get; set; }
        }

        [XmlRoot(ElementName = "index")]
        public class Index
        {
            [XmlElement(ElementName = "dir")]
            public List<Dir> Dir { get; set; }

            [XmlAttribute(AttributeName = "rev")]
            public string Rev { get; set; }

            [XmlAttribute(AttributeName = "path")]
            public string Path { get; set; }
        }

        [XmlRoot(ElementName = "svn")]
        public class Response
        {
            [XmlElement(ElementName = "index")]
            public Index Index { get; set; }

            [XmlAttribute(AttributeName = "version")]
            public string Version { get; set; }

            [XmlAttribute(AttributeName = "href")]
            public string Href { get; set; }
        }

        #endregion
    }
}