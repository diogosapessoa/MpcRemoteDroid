using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using MpcRemoteDroid.Src.Enums;

namespace MpcRemoteDroid.Src.Communication
{
    public class HttpComm
    {
        public string Url { get; private set; }
        public HttpContentType ContentType { get; private set; }
        public HttpMethod Method { get; private set; }

        private HttpComm() { }

        private Task<WebResponse> GetWebResponseAsync()
        {
            if (string.IsNullOrEmpty(Url))
            {
                throw new ArgumentNullException(nameof(Url), "Is null or empty");
            }
            if (Method == HttpMethod.Undefined)
            {
                throw new ArgumentException("Undefined HttpMethod", nameof(Method));
            }
            if (ContentType == HttpContentType.Undefined)
            {
                throw new ArgumentException("Undefined ContentType", nameof(ContentType));
            }
            var request = WebRequest.Create(Url) as HttpWebRequest;
            request.Method = Method.ToString().ToUpper();
            request.ContentType = Content.GetTypeMime(ContentType);
            return request.GetResponseAsync();
        }

        public static HttpComm Request()
        {
            return new HttpComm();
        }

        public HttpComm WithUrl(string url)
        {
            Url = url;
            return this;
        }

        public HttpComm WithContentType(HttpContentType contentType)
        {
            ContentType = contentType;
            return this;
        }

        public HttpComm WithMethod(HttpMethod method)
        {
            Method = method;
            return this;
        }

        public async Task<string> GetResponseStringAsync()
        {
            using (var response = await GetWebResponseAsync())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        public async Task<byte[]> GetResponseBytesAsync()
        {
            using (var response = await GetWebResponseAsync())
            {
                using (var ms = new MemoryStream())
                {
                    await response.GetResponseStream().CopyToAsync(ms);
                    return ms.ToArray();
                }
            }
        }

        public async Task<HttpStatusCode> ExecuteAsync()
        {
            using (var response = await GetWebResponseAsync())
            {
                if (response is HttpWebResponse httpResponse)
                {
                    return httpResponse.StatusCode;
                }
                return HttpStatusCode.NotImplemented;
            }
        }
    }
}