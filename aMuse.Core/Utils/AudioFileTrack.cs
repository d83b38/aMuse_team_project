using System;
using System.IO;
using System.Windows.Media.Imaging;
using aMuse.Core.Interfaces;
using aMuse.Core.APIData;
using TagLib;
using System.Threading.Tasks;
using aMuse.Core.Model;

namespace aMuse.Core.Utils
{
    public class AudioFileTrack : IAudio {
        public readonly string _path;
        public TimeSpan Duration { get; set; }
        public string Artist { get; set; }
        public string Track { get; set; }
        public Track TrackData { get; set; }
        private TagLib.File File { get; set; }

        ITrackDataParsing DataParsing;

        private bool HasCover;
        private bool hasInfo;
        public void GetData()
        {
            if (!hasInfo)
            {
                GetInfo();
                hasInfo = true;
            }
        }
       
        internal AudioFileTrack(string path) {
            _path = path;
            GetFile();
            SetupParsing();
        }

        private void SetupParsing() {
            DataParsing = new GeniusData(Artist, Track);
        }

        private void GetFile()
        {
            File = TagLib.File.Create(_path);
            Duration = File.Properties.Duration;

            // try getting artist and title from file tags
            bool hasTags = true;

            if (File.Tag.Performers.Length > 0 && !String.IsNullOrWhiteSpace(File.Tag.Performers[0]))
            {
                Artist = CleanText(File.Tag.Performers[0]);
            }
            else
            {
                hasTags = false;
            }

            if (!String.IsNullOrWhiteSpace(File.Tag.Title))
            {
                Track = CleanText(File.Tag.Title);
            }
            else
            {
                hasTags = false;
            }

            // if no artist or title tag is present, then set them based on file name
            if (!hasTags)
            {
                string[] info = (Path.GetFileNameWithoutExtension(_path)).Split('-');

                Artist = info[0].Trim();
                File.Tag.Performers = new string[1] { info[0] };

                if (info.Length > 1)
                {
                    Track = info[1].Trim();
                    File.Tag.Title = info[1];
                }
                File.Save();
            }
        }

        public async Task<Track> GetTrackTaskAsync() {
            TrackData = await DataParsing.GetTrackTaskAsync();
            return TrackData;
        }

        public async Task<string> GetLyricsTaskAsync(string url) {
            return await DataParsing.GetLyricsTaskAsync(url);
        }

        public async Task<string> GetAlbumTaskAsync(string url) {
            return await DataParsing.GetAlbumTaskAsync(url);
        }

        public async Task<BitmapImage> GetAlbumCoverTaskAsync(string url) {
            if (!HasCover)
                return await DataParsing.GetImageTaskAsync(url);
            else {
                MemoryStream ms = new MemoryStream(File.Tag.Pictures[0].Data.Data);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
        }

        public async Task<BitmapImage> GetImageTaskAsync(string url) {
            return await DataParsing.GetImageTaskAsync(url);
        }

        public async Task<Artist> GetArtistAsync(string Id) {
            return await DataParsing.GetArtistAsync(Id);
        }

        private void GetInfo()
        {
            HasCover = false;
            if (File.Tag.Pictures != null)
                HasCover = true;
        }

        private string CleanText(string uncleaned) {
            string eraseAfter = "";
            if (uncleaned.Contains("ft"))
            {
                eraseAfter = "ft";
            }
            else if (uncleaned.Contains("feat"))
            {
                eraseAfter = "feat";
            }

            if (eraseAfter != "")
            {
                return uncleaned.Remove(uncleaned.IndexOf(eraseAfter));
            }
            else
                return uncleaned;
        }
    }
}
