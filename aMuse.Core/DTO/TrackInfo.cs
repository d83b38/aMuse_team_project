using CsQuery;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;

namespace aMuse.Core.DTO {
    class TrackInfo {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string TrackTitle { get; set; }
        [JsonProperty("title_with_featured")]
        public string TrackTitleWithFeatured { get; set; }
        [JsonProperty("url")]
        public string FullLyricsUrl { get; set; }
        [JsonProperty("primary_artist")]
        public ArtistInfo ArtistInfo { get; set; }
        [JsonProperty("header_image_url")]
        public string AlbumImageUrl { get; set; }
        [JsonProperty("header_image_thumbnail_url")]
        public string AlbumSmallImageUrl { get; set; }
        public string GetLyrics() {
            var html = CQ.Create(new WebClient().DownloadString(FullLyricsUrl));
            var reg = new Regex(@"(\s{2,}.+\s{2,})|(\[.+\])| {2,}|");
            return reg.Replace(html.Find(".lyrics").Text(), "");
        }
    }
}
