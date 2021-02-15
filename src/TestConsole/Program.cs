using Orbital7.Apis.IGDB;
using System;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static async Task Main(
            string[] args)
        {
            const string CLIENT_ID = "TODO";
            const string CLIENT_SECRET = "TODO";

            var client = new IGDBApiClient(CLIENT_ID, CLIENT_SECRET);

            var marioGames = await client.QueryGamesAsync(search: "Mario");
            if (marioGames.Count > 0)
            {
                var covers = await client.QueryCoversAsync(
                    fields: "image_id",
                    where: $"id = {marioGames[0].cover.Value}");
                if (covers.Count > 0)
                {
                    var coverUrl = client.GetImageJpegUrl(
                        covers[0].image_id,
                        ImageSizeType.t_720p);

                    var coverContents = await client.DownloadImageJpegAsync(
                        covers[0].image_id,
                        ImageSizeType.t_720p);
                }
            }
        }
    }
}
