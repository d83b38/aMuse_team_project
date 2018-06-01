using CsQuery;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aMuse.Core.DTO {
    class TrackInfo {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string TrackTitle { get; set; }
        [JsonProperty("url")]
        public string FullLyricsUrl { get; set; }
        [JsonProperty("primary_artist")]
        public ArtistInfo ArtistInfo { get; set; }

        public string GetLyrics() {
            var html = CQ.Create(new WebClient().DownloadString(FullLyricsUrl));
            var reg = new Regex(@"(\s{2,}.+\s{2,})|(\[.+\])| {2,}|");
            return reg.Replace(html.Find(".lyrics").Text(), "");
        }
    }
}
