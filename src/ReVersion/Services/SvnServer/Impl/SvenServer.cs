using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using ReVersion.Helpers;
using ReVersion.Models;
using ReVersion.Services.SvnServer.Response;

namespace ReVersion.Services.SvnServer.Impl
{
    public class SvenServer : ISvnServer
    {
        public SvnServerType ServerType { get; } = SvnServerType.Sven;


        public ListRepositoriesResponse ListRepositories(SvnServerModel request)
        {
            var result = new ListRepositoriesResponse { Status = true };

            //Login

            using (var wb = new WebClientSession())
            {
                //Load the login page to grab a viewstate
                var loginGetResponse = wb.Get(request.BaseUrl + "/ubersvn/views/platform/shared/welcome.jsf");
                var viewState = GetResponseViewState(loginGetResponse);

                var loginData = new NameValueCollection
                {
                    ["mainPortalForm"] = "mainPortalForm",
                    ["username"] = request.Username,
                    ["password"] = request.GetPassword(),
                    ["loginButton"] = "",
                    ["javax.faces.ViewState"] = viewState
                };

                //Do the login
                wb.Post(request.BaseUrl + "/ubersvn/views/platform/shared/welcome.jsf", loginData);

                //Load the Repo browser to get a view state
                var repoBrowserResponse = wb.Get(request.BaseUrl + "/ubersvn/views/platform/repository/viewRepositories.jsf");
                viewState = GetResponseViewState(repoBrowserResponse);

                var repoListData = new NameValueCollection
                {
                    ["mainPortalForm"] = "mainPortalForm",
                    ["serverOS"] = "Windows",
                    ["popupField"] = "repo_PathValue",
                    ["globalFilter"] = "",
                    ["javax.faces.ViewState"] = viewState,
                    ["javax.faces.partial.ajax"] = "true",
                    ["javax.faces.source"] = "repoTableId",
                    ["javax.faces.partial.execute"] = "repoTableId",
                    ["javax.faces.partial.render"] = "repoTableId",
                    ["repoTableId"] = "repoTableId",
                    ["repoTableId_paging"] = "true",
                    ["repoTableId_first"] = "0",
                    ["repoTableId_rows"] = "150",
                    ["repoTableId_page"] = "2",
                };

                var repoListResponse = wb.Post(request.BaseUrl + "/ubersvn/views/platform/repository/viewRepositories.jsf", repoListData);

                if (repoListResponse == null)
                {
                    result.Status = false;
                    return result;
                }

                var repoNameRegex = new Regex("(?<=</a></td><td>)(.+?)(?=</td><td>)");

                var stream = repoListResponse.GetResponseStream();
                var streamReader = new StreamReader(stream, Encoding.UTF8);

                foreach (var repo in repoNameRegex.Matches(streamReader.ReadToEnd()))
                {
                    result.Repositories.Add(new RepositoryResult
                    {
                        Name = repo.ToString().Split('/').Last(),
                        Url = repo.ToString()
                    });
                }
               
            }

            return result;
        }

        private string GetResponseViewState(WebResponse response)
        {
            var responseDoc = new HtmlDocument();
            var responseStream = response.GetResponseStream();
            var streamReader = new StreamReader(responseStream, Encoding.UTF8);
            responseDoc.LoadHtml(streamReader.ReadToEnd());

            var viewState =
                responseDoc.DocumentNode.SelectNodes("//input[@name=\"javax.faces.ViewState\"]")
                .First()
                .Attributes["value"].Value;

            return HttpUtility.UrlEncode(viewState);
        }
    }
}
