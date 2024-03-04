using DLYoutube.DataAccess;
using DLYoutube.BusinessManagement;
using YoutubeExplode;

namespace DLYoutube
{
    class Program
    {
        private static DLYoutube.BusinessManagement.Download download = new DLYoutube.BusinessManagement.Download();

        /// <summary>
        /// Permet de télécharger des vidéos de youtube
        /// </summary>
        /// <param name="channel">url pour télécharger un channel complet</param>
        /// <param name="args">permet de mettre plusieurs urls de vidéos à télécharger</param>
        static async Task<int> Main(string channel, string[] args)
        {
            bool isDownload = true;
            if (channel != null) 
            {
                var youtube = new YoutubeClient();
                var channelObject = await youtube.Channels.GetByHandleAsync(channel);
                var channelId = channelObject.Id;
                isDownload = await download.DownloadChannel(channelId);
            }
            if (!isDownload) 
            {
                Console.WriteLine("There was an error during the downloading of the channel");
                return 1;
            }
            if (args.Length > 0)
            {
                isDownload = await download.DownloadVideo(args);
                if (!isDownload)
                {
                    Console.WriteLine("There was an error during the downloading of the videos");
                    return 1;
                }
            }
            return 0;

        }

    }

}