using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NLog;
using NzbDrone.Common.Http;
using NzbDrone.Common.Serializer;
using NzbDrone.Core.IndexerSearch.Definitions;

namespace NzbDrone.Core.Indexers.Soulseek
{
    public class SoulseekRequestGenerator : IIndexerRequestGenerator
    {
        public SoulseekSettings Settings { get; set; }

        public IHttpClient HttpClient { get; set; }
        public Logger Logger { get; set; }

        private string Token { get; set; }
        public virtual IndexerPageableRequestChain GetRecentRequests()
        {
            var pageableRequests = new IndexerPageableRequestChain();

            pageableRequests.Add(GetRequest(null));

            return pageableRequests;
        }

        public IndexerPageableRequestChain GetSearchRequests(AlbumSearchCriteria searchCriteria)
        {
            var pageableRequests = new IndexerPageableRequestChain();
            pageableRequests.Add(GetRequest(string.Format("&artistname={0}&groupname={1}", searchCriteria.ArtistQuery, searchCriteria.AlbumQuery)));
            return pageableRequests;
        }

        public IndexerPageableRequestChain GetSearchRequests(ArtistSearchCriteria searchCriteria)
        {
            var pageableRequests = new IndexerPageableRequestChain();
            pageableRequests.Add(GetRequest(string.Format("&artistname={0}", searchCriteria.ArtistQuery)));
            return pageableRequests;
        }

        private IEnumerable<IndexerRequest> GetRequest(string searchParameters)
        {
            Authenticate();

            //var filter = "";
            if (searchParameters == null)
            {
            }

            var request =
                new IndexerRequest(
                    $"{Settings.BaseUrl.Trim().TrimEnd('/')}{Settings.ApiPath.Trim().TrimEnd('/')}/Searches",
                    HttpAccept.Json);
            var searchObject = Json.ToJson(new
            {
                id = Guid.NewGuid(),
                searchText = searchParameters
            });
            request.HttpRequest.Method = HttpMethod.POST;
            request.HttpRequest.ContentData = Encoding.ASCII.GetBytes(searchObject);
            request.HttpRequest.Headers.Add("Authorization", "Bearer " + Token);
            request.HttpRequest.Headers.ContentType = "application/json";

            //var cookies = AuthCookieCache.Find(Settings.BaseUrl.Trim().TrimEnd('/'));
            //foreach (var cookie in cookies)
            //{
            //    request.HttpRequest.Cookies[cookie.Key] = cookie.Value;
            //}
            yield return request;
        }

        private void Authenticate()
        {
            if (string.IsNullOrEmpty(Token))
            {
                var requestBuilder = new HttpRequestBuilder($"{Settings.BaseUrl.Trim() + Settings.ApiPath}")
                {
                    LogResponseContent = true
                };

                requestBuilder.Method = HttpMethod.POST;
                requestBuilder.Resource("session");
                requestBuilder.PostProcess += r => r.RequestTimeout = TimeSpan.FromSeconds(15);

                var authLoginRequest = requestBuilder
                        .SetHeader("Content-Type", "application/json")
                        .Accept(HttpAccept.Json)
                        .Build();
                dynamic loginDetails = new
                {
                    username = Settings.Username,
                    password = Settings.Password
                };
                string jsonData = Json.ToJson(loginDetails);
                authLoginRequest.ContentData = Encoding.ASCII.GetBytes(jsonData);
                var response = HttpClient.Execute(authLoginRequest);
                var authResponse = Json.Deserialize<SoulseekAuthResponse>(response.Content);
                Token = authResponse.token;
            }
        }
    }

    internal class SoulseekAuthResponse
    {
        private DateTime _expiryDate;
        public string expires
        {
            get { return _expiryDate.ToString(); }
            set { _expiryDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(value)).UtcDateTime; }
        }

        private DateTime _issuedDate;
        public string issued
        {
            get { return _issuedDate.ToString(); }
            set { _issuedDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(value)).UtcDateTime; }
        }

        public string name { get; set; }
        public string notBefore { get; set; }
        public string token { get; set; }
        public string tokenType { get; set; }
    }
}
