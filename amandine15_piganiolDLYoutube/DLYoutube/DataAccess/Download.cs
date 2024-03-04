using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace DLYoutube.DataAccess
{
    internal class Download
    {
        public async Task<(Stream stream, string title)> DownloadVideo(string urlVideo)
        {
            var youtube = new YoutubeClient();

            var video = await youtube.Videos.GetAsync(urlVideo);

            var title = video.Title;

            // Get the list of all available streams
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(urlVideo);

            // Get highest quality muxed stream
            var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();

            // Get the actual stream
            var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

            return (stream, title);
        }

        public async IAsyncEnumerable<(Stream stream, string title)> DownloadChannel(string channelId)
        {
            // I didn't know if the channelId was the  url or the id of the channel,
            // I considered that it was the id
            var youtube = new YoutubeClient();

            // Get the channel
            var channel = await youtube.Channels.GetAsync(channelId);

            // Get the list of videos of the channel
            var videos = await youtube.Channels.GetUploadsAsync(channelId);

            var result = new List<(Stream, string)>();

            foreach ( var video in videos)
            {
                yield return await DownloadVideo(video.Url);
            }
        }
    }
}
