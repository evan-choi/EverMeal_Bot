using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Net
{
    public class HttpHelper
    {
        private static HttpRequestHeader[] knownHeaders =
            new HttpRequestHeader[]
            {
                HttpRequestHeader.KeepAlive,
                HttpRequestHeader.ContentLength,
                HttpRequestHeader.Host,
                HttpRequestHeader.ContentType,
                HttpRequestHeader.TransferEncoding,
                HttpRequestHeader.Connection,
                HttpRequestHeader.Accept,
                HttpRequestHeader.Referer,
                HttpRequestHeader.UserAgent,
                HttpRequestHeader.Expect,
                HttpRequestHeader.IfModifiedSince,
                HttpRequestHeader.Date
            };

        private static Dictionary<HttpRequestHeader, PropertyInfo> headerProps =
            typeof(HttpWebRequest)
                .GetProperties()
                .Where(pi => pi.Name.ToEnum<HttpRequestHeader>().HasValue)
                .ToDictionary(pi => pi.Name.ToEnum<HttpRequestHeader>().Value);

        private CookieContainer cookie;

        public bool CookieMaintain { get; }

        public HttpHelper(bool cookieMaintain)
        {
            cookie = new CookieContainer();

            this.CookieMaintain = cookieMaintain;
        }

        public HttpHelper() : this(true)
        {
        }

        public async Task<Tuple<string, HttpStatusCode>> POST(string uri, byte[] postData, WebHeaderCollection whc = null, Encoding encoding = null)
        {
            return await Request(uri, "POST", postData, whc, encoding);
        }

        public async Task<Tuple<string, HttpStatusCode>> GET(string uri, WebHeaderCollection whc = null, Encoding encoding = null)
        {
            return await Request(uri, "GET", null, whc, encoding);
        }

        private async Task<Tuple<string, HttpStatusCode>> Request(string uri, string method, byte[] postData = null, WebHeaderCollection whc = null, Encoding encoding = null)
        {
            string result = null;
            HttpStatusCode status = HttpStatusCode.RequestTimeout;

            try
            {
                encoding = encoding ?? Encoding.Default;

                var req = WebRequest.Create(uri);

                req.Method = method;

                if (CookieMaintain)
                    (req as HttpWebRequest).CookieContainer = cookie;

                if (whc != null)
                    SetHeaderCollection(req, whc);

                if (method.AnyEquals("post") && postData != null)
                {
                    req.ContentLength = postData.Length;

                    var req_s = await req.GetRequestStreamAsync();
                    await req_s.WriteAsync(postData, 0, postData.Length);
                }

                var res = (HttpWebResponse)await req.GetResponseAsync();

                if (CookieMaintain)
                {
                    foreach (Cookie c in (res as HttpWebResponse).Cookies)
                    {
                        cookie.Add(c);
                    }
                }

                if (res.StatusCode == HttpStatusCode.OK)
                {
                    using (var sr = new StreamReader(res.GetResponseStream(), encoding))
                    {
                        result = await sr.ReadToEndAsync();
                        status = res.StatusCode;
                    }
                }
            }
            catch (Exception ex)
            {
                status = HttpStatusCode.InternalServerError;
            }

            return new Tuple<string, HttpStatusCode>(result, status);
        }

        private void SetHeaderCollection(WebRequest req, WebHeaderCollection whc)
        {
            foreach (var header in whc.AllKeys)
            {
                if (HttpKnownHeaderNames.ValueNames.ContainsKey(header))
                {
                    var reqHeader = HttpKnownHeaderNames.ValueNames[header];

                    if (knownHeaders.Contains(reqHeader) && headerProps.ContainsKey(reqHeader))
                    {
                        headerProps[reqHeader].SetValue(req, whc[header]);
                    }
                    else
                    {
                        req.Headers.Add(whc[header]);
                    }
                }
                else
                {
                    req.Headers.Add(whc[header]);
                }
            }
        }
    }
}