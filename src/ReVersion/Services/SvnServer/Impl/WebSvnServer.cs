﻿using HtmlAgilityPack;
using ReVersion.Models.Settings;
using ReVersion.Services.SvnServer.Response;
using ReVersion.Utilities.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ReVersion.Services.SvnServer.Impl
{
    internal class WebSvnServer : ISvnServer
    {
        public SvnServerType ServerType { get; } = SvnServerType.WebSvn;

        public async Task<ListRepositoriesResponse> ListRepositories(SvnServerModel request)
        {
            var result = new ListRepositoriesResponse {Status = true};

            //Login
            //svn url = requet.Url + "svn/";

            using (var wb = new WebClientSession())
            {

                //Do the login
                var homePageResponse = await wb.AuthenticateAsync(request.BaseUrl + "/websvn/", request.Username, request.RawPassword);


                var responseDoc = new HtmlDocument();
                var responseStream = homePageResponse.GetResponseStream();
                var streamReader = new StreamReader(responseStream, Encoding.UTF8);
                responseDoc.LoadHtml(streamReader.ReadToEnd());

                foreach (var node in responseDoc.DocumentNode.SelectNodes("//div[@class=\"projectlist\"]/div[@class=\"project\"]")) {

                    var name = node.SelectSingleNode("a").InnerText;

                    result.Repositories.Add(new RepositoryResult
                    {
                        SvnServerId = request.Id,
                        Name = name,
                        Url = request.BaseUrl + "/svn/" + name
                    });

                }
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