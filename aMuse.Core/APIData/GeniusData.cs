using aMuse.Core.DTO;
using aMuse.Core.Interfaces;
using aMuse.Core.Model;
using CsQuery;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aMuse.Core.APIData {
    internal class GeniusData : ITrackDataParsing
    {
        readonly string AccessToken = "ht8qQUrmTslR1YPPGMM_d-EnKOx9Byf4IYtsg8hssPLP8Xb2b5ck7K0hRKIKmP4C";

        public bool IsParsingSuccessful { get; private set; }

        private string Artist { get; set; }
        private string Track { get; set; }
        private Track TrackData { get; set; }

        static string BuildUrl(string baseUrl, IDictionary<string, string> parameters)
        {
            var sb = new StringBuilder(baseUrl);
            if (parameters?.Count > 0) {
                sb.Append('?');
                foreach (var p in parameters) {
                    sb.Append(p.Key);
                    sb.Append("=");
                    sb.Append(WebUtility.UrlEncode(p.Value));
                    sb.Append('&');
                }
                sb.Append("per_page=1");
            }
            return sb.ToString();
        }

        public GeniusData(string Artist, string Track)
        {
            this.Artist = Artist;
            this.Track = Track;
        }

        private async Task<Track> GetTrackTaskAsync() {
            var url = BuildUrl("https://api.genius.com/search",
                                new Dictionary<string, string>()
                                {
                                    {"access_token", AccessToken },
                                    {"q", Artist + " " + Track},
                                });
            using (var client = new HttpClient()) {
                try {
                    string strResult;
                    if ((strResult = await client.GetStringAsync(url)) != null) {
                        Data result = JsonConvert.DeserializeObject<Data>(strResult);
                        IsParsingSuccessful = (result.Response.Hits.Count != 0);
                        if (IsParsingSuccessful) {
                            return new Track {
                                Title = result.Response.Hits[0].TrackInfo.TrackTitle,
                                TitleWithFeatured = result.Response.Hits[0].TrackInfo.TrackTitleWithFeatured,
                                LyricsUrl = result.Response.Hits[0].TrackInfo.FullLyricsUrl,
                                Artist = new Artist() {
                                    Name = result.Response.Hits[0].TrackInfo.ArtistInfo.Name,
                                },
                                AlbumCoverUrl = result.Response.Hits[0].TrackInfo.AlbumImageUrl,
                                AlbumCoverThumbnailUrl = result.Response.Hits[0].TrackInfo.AlbumSmallImageUrl
                            };
                        }
                        else 
                            return null;
                    }
                    else
                        return null;
                }
                catch (Exception ex) {
                    throw ex;
                }
            }
        }

        public async Task<string[]> GetLyricsAndAlbumTaskAsync() {
            TrackData = await GetTrackTaskAsync();
            var data = new string[2];
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            var client = webClient.DownloadString(TrackData.LyricsUrl);
            var html = CQ.Create(client);
            var uncleanedLyrics = html.Find(".lyrics").Text();
            var uncleanedAlbum = html.Find(".song_album-info-title").Text();
            var reg = new Regex(@"(\s{2,}.+\s{2,})|(\[.+\])| {2,}");
            var regAlbum = new Regex(@"\s{2,}");
            var regLineBreaks = new Regex(@"\s{3,}");
            data[0] = regLineBreaks.Replace(reg.Replace(uncleanedLyrics, "\n"), "\n\n");
            data[1] = regAlbum.Replace(uncleanedAlbum, "");
            return data;
        }

        public async Task<byte[][]> GetAlbumCoversTaskAsync() {
            TrackData = await GetTrackTaskAsync();
            byte[][] covers = new byte[2][];

            using (var client = new HttpClient())
            {
                var FullCoverStream =  client.GetByteArrayAsync(TrackData.AlbumCoverUrl).Result;
                var ThumbnailStream =  client.GetByteArrayAsync(TrackData.AlbumCoverThumbnailUrl).Result;
                covers[0] = ThumbnailStream;
                covers[1] = FullCoverStream;
            }
            return covers;
        }

        public async Task<string> GetArtistTaskAsync() { 
            TrackData = await GetTrackTaskAsync();
            return TrackData.Artist.Name;
        }

        public async Task<string[]> GetTitleTaskAsync() {
            TrackData = await GetTrackTaskAsync();
            var titles = new string[2];
            titles[0] = TrackData.Title;
            titles[1] = TrackData.TitleWithFeatured;
            return titles;
        }
    }
}
