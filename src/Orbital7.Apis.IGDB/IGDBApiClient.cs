using Newtonsoft.Json;
using Orbital7.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Apis.IGDB
{
    public class IGDBApiClient
    {
        private const string URL_BASE = "https://api-v3.igdb.com";

        private string UserKey { get; set; }

        public IGDBApiClient(
            string userKey)
        {
            this.UserKey = userKey;
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

            // Create the request.
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.KeepAlive = true;
            request.Headers["user-key"] = this.UserKey;

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
            var firstIndex = result.IndexOf("[");
            var lastIndex = result.LastIndexOf("]");
            var json = result.Substring(firstIndex, result.Length - firstIndex -
                (result.Length - lastIndex) + 1);

            // Ignore deserialization errors.
            return JsonConvert.DeserializeObject<List<T>>(json,
                new JsonSerializerSettings()
                {
                    Error = (se, ev) => { ev.ErrorContext.Handled = true; }
                });
        }
    }
}
