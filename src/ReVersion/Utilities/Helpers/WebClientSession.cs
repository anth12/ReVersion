using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace ReVersion.Utilities.Helpers
{
    public class WebClientSession : WebClient
    {
        public WebClientSession()
        {
            CookieContainer = new CookieContainer();
        }

        public CookieContainer CookieContainer { get; set; }

        public WebResponse Get(string address)
        {
            var request = GetWebRequest(new Uri(address));

            var webRequest = request as HttpWebRequest;
            webRequest.CookieContainer = CookieContainer;

            request.Method = "GET";

            return request.GetResponse();
        }

        public WebResponse Authenticate(string address, string username, string password)
        {
            try
            {
                //Force the server to return a 401
                Get(address);
            }
            catch (Exception)
            {
                //Swallow the 401
            }

            var request = GetWebRequest(new Uri(address));

            var webRequest = request as HttpWebRequest;
            webRequest.CookieContainer = CookieContainer;

            request.Method = "GET";

            //this.UseDefaultCredentials = true;
            //this.Credentials = new NetworkCredential(username, password);
            //this.Encoding = System.Text.Encoding.UTF8;

            var authInfo = username + ":" + password;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;

            return request.GetResponse();
        }

        public WebResponse Post(string address, NameValueCollection data = null)
        {
            var request = GetWebRequest(new Uri(address));

            var webRequest = request as HttpWebRequest;
            webRequest.CookieContainer = CookieContainer;

            request.Method = "POST";

            request.ContentType = "application/x-www-form-urlencoded";
            var ascii = new ASCIIEncoding();
            var postBytes = ascii.GetBytes(DataToString(data));
            // add post data to request
            var postStream = request.GetRequestStream();
            postStream.Write(postBytes, 0, postBytes.Length);

            return request.GetResponse();
        }

        private string DataToString(NameValueCollection data)
        {
            var sb = new StringBuilder();

            var index = 0;
            foreach (var key in data.AllKeys)
            {
                if (index > 0)
                    sb.Append("&");

                sb.Append(string.Format("{0}={1}", key, data[key]));

                index++;
            }

            return sb.ToString();
        }
    }
}