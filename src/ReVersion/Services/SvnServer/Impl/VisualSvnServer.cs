using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;
using HtmlAgilityPack;
using ReVersion.Models;
using ReVersion.Services.SvnServer.Response;
using ReVersion.Utilities.Helpers;

namespace ReVersion.Services.SvnServer.Impl
{
    public class VisualSvnServerServer : ISvnServer
    {
        public SvnServerType ServerType { get; } = SvnServerType.VisualSvn;

        public ListRepositoriesResponse ListRepositories(SvnServerModel request)
        {
            var result = new ListRepositoriesResponse { Status = true };

            //Login

            using (var wb = new WebClientSession())
            {
                //Load the login page to grab a viewstate
                var homePageResponse = wb.Authenticate(request.BaseUrl + "/svn/", request.Username, request.Password);
                
                var serializer = new XmlSerializer(typeof(Response));
                var response = (Response)serializer.Deserialize(homePageResponse.GetResponseStream());


                foreach (var repoNode in response.Index.Dir)
                {
                    result.Repositories.Add(new RepositoryResult
                    {
                        Name = repoNode.Name,
                        Url = $"{request.BaseUrl}/{repoNode.Href}"
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
