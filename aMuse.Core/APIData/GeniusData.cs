using aMuse.Core.DTO;
using aMuse.Core.Interfaces;
using aMuse.Core.Model;
using CsQuery;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace aMuse.Core.APIData {
    internal class GeniusData : ITrackDataParsing {
        readonly string AccessToken = "ht8qQUrmTslR1YPPGMM_d-EnKOx9Byf4IYtsg8hssPLP8Xb2b5ck7K0hRKIKmP4C";

        public bool IsParsingSuccessful { get; private set; }

        private string Artist { get; set; }
        private string Track { get; set; }
        private Track TrackData { get; set; }

        static string BuildSearchUrl(string baseUrl, IDictionary<string, string> parameters)
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

        static string BuildArtistUrl(string baseUrl, string artistId, string Token) {
            var sb = new StringBuilder(baseUrl);
            sb.Append('/');
            sb.Append(artistId);
            sb.Append('?');
            sb.Append("access_token=");
            sb.Append(Token);
            sb.Append("&text_format=plain");
            return sb.ToString();
        }

        public GeniusData(string Artist, string Track)
        {
            this.Artist = Artist;
            this.Track = Track;
        }

        public async Task<Artist> GetArtistAsync(string Id) {
            var url = BuildArtistUrl("https://api.genius.com/artists", Id, AccessToken);
            using (var client = new HttpClient()) {
                var strResult = await client.GetStringAsync(url);
                Data result = JsonConvert.DeserializeObject<Data>(strResult);
                return new Artist {
                    Id = Id,
                    Name = result.Response.ArtistDesctiption.Name,
                    Description = result.Response.ArtistDesctiption.Description.Text,
                    ImageUrl = result.Response.ArtistDesctiption.ArtistDescriptionImageUrl,
                    GenuisPageUrl = result.Response.ArtistDesctiption.ArtistDescriptionGeniusUrl
                };
            }
        }

        public async Task<Track> GetTrackTaskAsync() {
            var url = BuildSearchUrl("https://api.genius.com/search",
                                new Dictionary<string, string>()
                                {
                                    {"access_token", AccessToken },
                                    {"q", Artist + " " + Track},
                                });
            using (var client = new HttpClient()) {
                var strResult = await client.GetStringAsync(url);
                Data result = JsonConvert.DeserializeObject<Data>(strResult);
                try {
                        return new Track {
                            Title = result.Response.Hits[0].TrackInfo.TrackTitle,
                            TitleWithFeatured = result.Response.Hits[0].TrackInfo.TrackTitleWithFeatured,
                            LyricsUrl = result.Response.Hits[0].TrackInfo.FullLyricsUrl,
                            Artist = new Artist() {
                                Id = result.Response.Hits[0].TrackInfo.ArtistInfo.Id,
                                Name = result.Response.Hits[0].TrackInfo.ArtistInfo.Name,
                            },
                            AlbumCoverUrl = result.Response.Hits[0].TrackInfo.AlbumImageUrl,
                            AlbumCoverThumbnailUrl = result.Response.Hits[0].TrackInfo.AlbumSmallImageUrl
                        };
                }
                catch (Exception) {
                    return null;
                }
            }
        }

        public async Task<string> GetLyricsTaskAsync(string Url) {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            var client = await webClient.DownloadStringTaskAsync(Url);
            var html = CQ.Create(client);
            var uncleanedLyrics = html.Find(".lyrics").Text();
            var reg = new Regex(@"(\s{2,}.+\s{2,})|(\[.+\])| {2,}");
            var regLineBreaks = new Regex(@"\s{3,}");
            return regLineBreaks.Replace(reg.Replace(uncleanedLyrics, "\n"), "\n\n");
        }

        public async Task<string> GetAlbumTaskAsync(string Url) {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            var client = await webClient.DownloadStringTaskAsync(Url);
            var html = CQ.Create(client);
            var uncleanedAlbum = html.Find(".song_album-info-title").Text();
            var regAlbum = new Regex(@"\s{2,}");
            return regAlbum.Replace(uncleanedAlbum, "");
        }

        public async Task<BitmapImage> GetImageTaskAsync(string url) {
            using (var client = new HttpClient()) {
                byte[] data = await client.GetByteArrayAsync(url);
                return BytesToImage(data);
            }
        }

        public async Task<BitmapImage> GetAlbumCoverTaskAsync(string url) {
            using (var client = new HttpClient()) {
                byte[] data = await client.GetByteArrayAsync(url);
                return BytesToImage(data);
            }
        }

        public BitmapImage BytesToImage(byte[] data) {
            var ms = new System.IO.MemoryStream(data);
            ms.Seek(0, SeekOrigin.Begin);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = ms;
            image.EndInit();
            image.Freeze();
            return image;
        }
    }
}
