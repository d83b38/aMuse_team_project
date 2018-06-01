using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using aMuse.Core.DTO;
using aMuse.Core.Model;
using CsQuery;
using Newtonsoft.Json;

namespace aMuse.Core {
    public class GeniusInfoParse {
        private const string AccessToken = "ht8qQUrmTslR1YPPGMM_d-EnKOx9Byf4IYtsg8hssPLP8Xb2b5ck7K0hRKIKmP4C";

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
        private Track GetTrack(string Artist, string Track) {
            var url = BuildUrl("https://api.genius.com/search",
                new Dictionary<string, string>() 
                {
                    {"access_token", AccessToken },
                    {"q", Artist + " " + Track},
                });
            using (var client = new HttpClient()) {
                var strResult = client.GetStringAsync(url).Result;
                var result = JsonConvert.DeserializeObject<Data>(strResult);
                return new Track() {
                    Title = result.Response.Hits[0].TrackInfo.TrackTitle,
                    LyricsUrl = result.Response.Hits[0].TrackInfo.FullLyricsUrl,
                    Artist = new Artist() {
                        Name = result.Response.Hits[0].TrackInfo.ArtistInfo.Name,
                    }
                };
            }
        }

        public string GetLyrics() {
            var html = CQ.Create(new WebClient().DownloadString(GetTrack(Artist, Track).LyricsUrl));
            var uncleanedLyrics = html.Find(".lyrics").Text();
            var reg = new Regex(@"(\s{2,}.+\s{2,})|(\[.+\])| {2,}|");
            return reg.Replace(uncleanedLyrics, "");
        }
    }
}
