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

namespace aMuse.Core.APIData {
    internal class GeniusData : ITrackDataParsing
    {
        readonly string AccessToken = "ht8qQUrmTslR1YPPGMM_d-EnKOx9Byf4IYtsg8hssPLP8Xb2b5ck7K0hRKIKmP4C";

        private bool _parsingSuccessful;
        public bool ParsingSuccessful
        {
            get { return _parsingSuccessful; }
        }

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
            TrackData = GetTrackData();
        }

        //will make this async any time soom
        Track/*async Task<Track>*/ GetTrackData()
        {
            var url = BuildUrl("https://api.genius.com/search",
                                new Dictionary<string, string>()
                                {
                                    {"access_token", AccessToken },
                                    {"q", Artist + " " + Track},
                                });

            using (var client = new HttpClient())
            {
                try
                {
                    string strRresult = /*await*/ client.GetStringAsync(url).Result;
                    Data result = JsonConvert.DeserializeObject<Data>(strRresult);

                    _parsingSuccessful = (result.Response.Hits.Count != 0);
                    if (_parsingSuccessful)
                    {
                        return new Track()
                        {
                            Title = result.Response.Hits[0].TrackInfo.TrackTitle,
                            TitleWithFeatured = result.Response.Hits[0].TrackInfo.TrackTitleWithFeatured,
                            LyricsUrl = result.Response.Hits[0].TrackInfo.FullLyricsUrl,
                            Artist = new Artist()
                            {
                                Name = result.Response.Hits[0].TrackInfo.ArtistInfo.Name,
                            },
                            AlbumCoverUrl = result.Response.Hits[0].TrackInfo.AlbumImageUrl,
                            AlbumCoverThumbnailUrl = result.Response.Hits[0].TrackInfo.AlbumSmallImageUrl
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public string/*async Task<string>*/ GetLyrics()
        {
            var html = CQ.Create(new WebClient().DownloadString(TrackData.LyricsUrl));
            var uncleanedLyrics = html.Find(".lyrics").Text();
            var reg = new Regex(@"(\s{2,}.+\s{2,})|(\[.+\])| {2,}|");
            return reg.Replace(uncleanedLyrics, "");
        }

        public byte[][] GetAlbumCovers()
        {
            byte[][] covers = new byte[2][];

            using (var client = new HttpClient())
            {
                var FullCoverStream = client.GetByteArrayAsync(TrackData.AlbumCoverUrl).Result;
                var ThumbnailStream = client.GetByteArrayAsync(TrackData.AlbumCoverThumbnailUrl).Result;
                covers[0] = FullCoverStream;
                covers[1] = ThumbnailStream;
            }

            return covers;
        }

        public string GetArtist()
        {
            return TrackData.Artist.Name;
        }

        public string[] GetTitle()
        {
            var titles = new string[2];
            titles[0] = TrackData.Title;
            titles[1] = TrackData.TitleWithFeatured;
            return titles;
        }
    }
}
