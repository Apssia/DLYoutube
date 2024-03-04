using DLYoutube.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos;

namespace DLYoutube.BusinessManagement
{
    internal class Download
    {
        private static DataAccess.Download download = new DataAccess.Download();
        private static Storage storage = new Storage();

        internal async Task<bool> DownloadVideo(string[] urlVideos)
        {
            try
            {
                foreach (var url in urlVideos)
                {
                    var (stream, title) = await download.DownloadVideo(url);
                    bool isSaved = await storage.SaveFile(stream, title);
                    if (!isSaved) 
                    {
                        Console.WriteLine("The video at this url " + url + " couldn't be download");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }            
        }

        internal async Task<bool> DownloadChannel(string channelId)
        {
            try
            {
                IAsyncEnumerable<(Stream, String)> videos = download.DownloadChannel(channelId);
                await foreach (var (stream, title) in videos)
                {
                    if (!await storage.SaveFile(stream, title))
                    {
                        Console.WriteLine("One of the videos couldn't be download");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
