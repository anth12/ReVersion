using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace ReVersion.Helpers
{
    public class WebClientSession : WebClient
    {
        public CookieContainer CookieContainer { get; set; }

        public WebClientSession()
            : base()
        {
            CookieContainer = new CookieContainer();
        }

        public WebResponse Get(string address, CookieContainer cookies = null)
        {
            var request = base.GetWebRequest(new Uri(address));

            var webRequest = request as HttpWebRequest;
            webRequest.CookieContainer = CookieContainer;

            if (cookies != null)
            {
                CookieContainer = cookies;
            }

            request.Method = "GET";

            return request.GetResponse();
        }

        public WebResponse Post(string address, NameValueCollection data = null, CookieContainer cookies = null)
        {
            var request = base.GetWebRequest(new Uri(address));

            var webRequest = request as HttpWebRequest;
            webRequest.CookieContainer = CookieContainer;

            if (cookies != null)
            {
                CookieContainer = cookies;
            }

            request.Method = "POST";

            request.ContentType = "application/x-www-form-urlencoded";
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] postBytes = ascii.GetBytes(DataToString(data));
            // add post data to request
            Stream postStream = request.GetRequestStream();
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
