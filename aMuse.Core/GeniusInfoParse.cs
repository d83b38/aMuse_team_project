using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using aMuse.Core.DTO;
using aMuse.Core.Model;
using CsQuery;
using Newtonsoft.Json;

namespace aMuse.Core {
    public class GeniusInfoParse {
        private readonly string AccessToken = "ht8qQUrmTslR1YPPGMM_d-EnKOx9Byf4IYtsg8hssPLP8Xb2b5ck7K0hRKIKmP4C";

        public string Artist { get; set; }
        public string Track { get; set; }

        public GeniusInfoParse(string Artist, string Track) {
            this.Artist = Artist;
            this.Track = Track;
        }

        static string BuildUrl(string baseUrl, IDictionary<string, string> parameters) {
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

        //will make this async any time soom
        private Track/*async Task<Track>*/ GetTrack(string Artist, string Track) {
            var url = BuildUrl("https://api.genius.com/search",
                new Dictionary<string, string>() 
                {
                    {"access_token", AccessToken },
                    {"q", Artist + " " + Track},
                });
            using (var client = new HttpClient()) {
                var strRresult = /*await*/ client.GetStringAsync(url).Result;
                var result = JsonConvert.DeserializeObject<Data>(strRresult);
                return new Track() {
                    Title = result.Response.Hits[0].TrackInfo.TrackTitle,
                    LyricsUrl = result.Response.Hits[0].TrackInfo.FullLyricsUrl,
                    Artist = new Artist() {
                        Name = result.Response.Hits[0].TrackInfo.ArtistInfo.Name,
                    },
                    AlbumCoverUrl = result.Response.Hits[0].TrackInfo.AlbumImageUrl,
                    AlbumCoverThumbnailUrl = result.Response.Hits[0].TrackInfo.AlbumSmallImageUrl
                };
            }
        }

        public string/*async Task<string>*/ GetLyrics() {
            var track = /*await*/ GetTrack(Artist, Track);
            var html = CQ.Create(new WebClient().DownloadString(track.LyricsUrl));
            var uncleanedLyrics = html.Find(".lyrics").Text();
            var reg = new Regex(@"(\s{2,}.+\s{2,})|(\[.+\])| {2,}|");
            return reg.Replace(uncleanedLyrics, "");
        }

        public List<BitmapImage> GetAlbumCovers() {
            List<BitmapImage> Covers = new List<BitmapImage>();
            var track = GetTrack(Artist, Track);
            using (var client = new HttpClient()) {
                var FullCoverStream = client.GetByteArrayAsync(track.AlbumCoverUrl).Result;
                var ThumbnailStream = client.GetByteArrayAsync(track.AlbumCoverThumbnailUrl).Result;
                //var cover = Image.FromStream(FullCoverStream);
                //var thumbnail = Image.FromStream(ThumbnailStream);
                Covers.Add(ConvertBytesToImage(FullCoverStream));
                Covers.Add(ConvertBytesToImage(ThumbnailStream));
            }
            return Covers;
        }

        //it'll sit here for a while, before we manage to make an abstraction for converting
        public BitmapImage ConvertBytesToImage(byte[] data) {
            MemoryStream stream = new MemoryStream(data);
            stream.Seek(0, SeekOrigin.Begin);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();

            image.Freeze();
            return image;
        }
    }
}
