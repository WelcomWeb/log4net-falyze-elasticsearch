using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using log4net.Falyze.ElasticSearch.Models;

namespace log4net.Falyze.ElasticSearch.Services
{
    public class HttpService
    {
        public Uri ServiceURI { get; set; }

        private string AuthHeader { get; set; }

        public HttpService(string credentials = null)
        {
            if (!string.IsNullOrWhiteSpace(credentials) && credentials.Contains(":"))
            {
                CreateAuthHeader(credentials);
            }
        }

        public void Post(Entry entry)
        {
#if NET4
            ThreadPool.QueueUserWorkItem(o => EntryPostRequest(JsonConvert.SerializeObject(entry)));
#else
            Task.Factory.StartNew(() => EntryPostRequest(JsonConvert.SerializeObject(entry)), TaskCreationOptions.LongRunning).ConfigureAwait(false);
#endif
        }

        private void EntryPostRequest(string json)
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers["Content-Type"] = "application/json";

                    if (!string.IsNullOrEmpty(AuthHeader))
                    {
                        client.Headers.Add(HttpRequestHeader.Authorization, AuthHeader);
                    }

                    client.UploadString(ServiceURI, json);
                }
                catch { }
            }
        }

        private void CreateAuthHeader(string credentials)
        {
            AuthHeader = string.Format("Basic {0}", Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials)));
        }
    }
}
