using Newtonsoft.Json;
using Orbital7.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Apis.IGDB
{
    public class IGDBApiClient
    {
        private const string URL_BASE = "https://api.igdb.com/v4";

        private string ClientId { get; set; }

        private string ClientSecret { get; set; }

        private string AccessToken { get; set; }

        public IGDBApiClient(
            string clientId,
            string clientSecret,
            string accessToken = null)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.AccessToken = accessToken;
        }

        public async Task<TokenResponse> GetAccessTokenAsync()
        {
            var url = $"https://id.twitch.tv/oauth2/token" +
                $"?client_id={this.ClientId}" +
                $"&client_secret={this.ClientSecret}" +
                $"&grant_type=client_credentials";

            // Create the request.
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.KeepAlive = true;

            // Send.
            using (var webResponse = await request.GetResponseAsync())
            {
                var response = await webResponse.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TokenResponse>(response);
            }
        }

        public async Task<string> QueryAsync(
            string endpoint,
            string fields = "*",
            int limit = 50,
            int offset = 0,
            string search = null,
            string where = null,
            string sort = null)
        {
            string url = $"{URL_BASE}/{endpoint}";
            string query = CreateQuery(
                fields,
                limit,
                offset,
                search,
                where,
                sort);

            // Ensure we have an access token.
            await EnsureAccessTokenAsync();

            // Create the request.
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.KeepAlive = true;
            request.Headers["Client-ID"] = this.ClientId;
            request.Headers["Authorization"] = $"Bearer {this.AccessToken}";

            // Write the request body.
            var encoding = new UTF8Encoding();
            byte[] queryBytes = encoding.GetBytes(query);
            using (var requestStream = request.GetRequestStream())
            {

                requestStream.Write(queryBytes, 0, queryBytes.Length);
                requestStream.Close();
            }

            // Send.
            try
            {
                using (var webResponse = await request.GetResponseAsync())
                {
                    return await webResponse.ReadAsStringAsync();
                }
            }
            catch (WebException webException)
            {
                string response = webException.Response != null ?
                    await webException.Response.ReadAsStringAsync() :
                    webException.Message;
                throw new Exception(response);
            }
        }

        public async Task<List<Platform>> QueryPlatformsAsync(
            string fields = "*",
            int limit = 500,
            int offset = 0,
            string search = null,
            string where = null,
            string sort = null)
        {
            var json = await QueryAsync(
                "platforms",
                fields,
                limit,
                offset,
                search,
                where,
                sort);

            return DeserializeList<Platform>(json);
        }

        public async Task<List<Game>> QueryGamesAsync(
            string fields = "*",
            int limit = 50,
            int offset = 0,
            string search = null,
            string where = null,
            string sort = null)
        {
            var json = await QueryAsync(
                "games",
                fields,
                limit,
                offset,
                search,
                where,
                sort);

            return DeserializeList<Game>(json);
        }

        public async Task<List<Cover>> QueryCoversAsync(
            string fields = "*",
            int limit = 50,
            int offset = 0,
            string search = null,
            string where = null,
            string sort = null)
        {
            var json = await QueryAsync(
                "covers",
                fields,
                limit,
                offset,
                search,
                where,
                sort);

            return DeserializeList<Cover>(json);
        }

        public string GetImageJpegUrl(
            string image_id,
            ImageSizeType size)
        {
            return $"https://images.igdb.com/igdb/image/upload/{size}/{image_id}.jpg";
        }

        public async Task<byte[]> DownloadImageJpegAsync(
            string image_id,
            ImageSizeType size)
        {
            return await HttpHelper.DownloadFileContentsAsync(
                GetImageJpegUrl(
                    image_id,
                    size));
        }

        private string CreateQuery(
            string fields = "*",
            int limit = 50,
            int offset = 0,
            string search = null,
            string where = null,
            string sort = null)
        {
            var sb = new StringBuilder();

            sb.Append($"fields {fields};");
            sb.Append($"limit {limit};");
            sb.Append($"offset {offset};");
            if (!string.IsNullOrWhiteSpace(search))
                sb.Append($"search {search.EncloseInQuotes()};");
            if (!string.IsNullOrWhiteSpace(where))
                sb.Append($"where {where};");
            if (!string.IsNullOrWhiteSpace(sort))
                sb.Append($"sort {sort};");

            return sb.ToString();
        }

        private List<T> DeserializeList<T>(
            string result)
        {
            return JsonConvert.DeserializeObject<List<T>>(result);
        }

        private async Task EnsureAccessTokenAsync()
        {
            if (string.IsNullOrEmpty(this.AccessToken))
            {
                var response = await GetAccessTokenAsync();
                this.AccessToken = response.AccessToken;
            }
        }
    }
}
