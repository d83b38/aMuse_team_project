using System;
using System.IO;
using System.Windows.Media.Imaging;
using aMuse.Core.Interfaces;
using aMuse.Core.APIData;
using TagLib;
using System.Threading.Tasks;

namespace aMuse.Core.Utils
{
    public class MusicFile : IAudio
    {
        public readonly string path;

        public TimeSpan Duration { get; set; }
        public BitmapImage[] CoverImages { get; set; }
        public string Artist { get; set; }
        public string Track { get; set; }
        public string Lyrics { get; set; }
        public byte[][] Covers { get; set; }
        public string[] Titles { get; set; }
        private TagLib.File File { get; set; }

        ITrackDataParsing DataParsing;

        /// <summary>
        /// Determines if the genius data has been got for this file
        /// </summary>
        private bool hasInfo;

        internal MusicFile(string path) {
            this.path = path;
            Covers = new byte[2][];
            CoverImages = new BitmapImage[2];
            Titles = new string[2];
            GetFile();
        }

        /// <summary>
        /// Get genuis information about the audio
        /// </summary>
        public void GetData()
        {
            if (!hasInfo)
            {
                DataParsing = new GeniusData(Artist, Track);
                if (DataParsing.IsParsingSuccessful)
                {
                    GetInfo();
                }
                hasInfo = true;
            }
        }


        private void Save() {
            File.Save();
       }

       private void GetFile()
       {
            File = TagLib.File.Create(path);
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
                string[] info = (Path.GetFileNameWithoutExtension(path)).Split('-');

                Artist = info[0].Trim();
                File.Tag.Performers = new string[1] { info[0] };

                if (info.Length > 1)
                {
                    Track = info[1].Trim();
                    File.Tag.Title = info[1];
                }
                Save();
            }
        }

        public async Task<string> SetArtistAsync() {
            string artist = await DataParsing.GetArtistTaskAsync();
            Artist = artist;
            return artist;
        }

        public async Task<string> SetLyricsAsync() {
            string lyrics = await DataParsing.GetLyricsTaskAsync();
            Lyrics = lyrics;
            return lyrics;
        }

        public async Task<BitmapImage[]> SetCoversAsync() {
            Covers = await DataParsing.GetAlbumCoversTaskAsync();
            if (Covers[0] != null) {
                CoverImages[0] = ToImage(Covers[0]);
            }
            if (Covers[1] != null) {
                CoverImages[1] = ToImage(Covers[1]);
            }
            return CoverImages;
        }

        public async Task<string[]> SetTitlesAsync() {
            var titles = await DataParsing.GetTitleTaskAsync();
            Titles = titles;
            return titles;
        }

        private void GetInfo()
        {
            bool tagsChanged = false;
            if (!File.Tag.Performers.Equals(Artist)) {
                File.Tag.Performers = new string[1] { Artist };
                tagsChanged = true;
            }
            if (!File.Tag.Title.Equals(Titles[0])) {
                File.Tag.Title = Titles[0];
                tagsChanged = true;
            }
            if (!File.Tag.Lyrics.Equals(Lyrics)) {
                File.Tag.Lyrics = Lyrics;
                tagsChanged = true;
            }
            if(Covers[0] != null && Covers[1] != null) { //TODO: check if both pictures are not null -- done (?)
                if (File.Tag.Pictures == new IPicture[2] { new Picture(new ByteVector(Covers[0])),
                                                  new Picture(new ByteVector(Covers[1]))}) {
                    return;
                }
                else {
                    File.Tag.Pictures = new IPicture[2] { new Picture(new ByteVector(Covers[0])),
                                                  new Picture(new ByteVector(Covers[1]))};
                    tagsChanged = true;
                }
            }
            if(tagsChanged == true) {
                File.Save();
            }

            //if (Artist != DataParsing.GetArtist(TrackInfo))
            //{
            //   Artist = DataParsing.GetArtist(TrackInfo);
            //}
            //if (Track != DataParsing.GetTitle(TrackInfo)[0] && Track !=  DataParsing.GetTitle(TrackInfo)[1]) {
            //    Track =  DataParsing.GetTitle(TrackInfo)[0];
            //}
            //Lyrics =  DataParsing.GetLyrics(TrackInfo);
            //Covers =  DataParsing.GetAlbumCovers(TrackInfo);

            //if (Covers[0] != null) {
            //    CoverImages[0] = ToImage(Covers[0]);
            //}
            //if (Covers[1] != null) {
            //    CoverImages[1] = ToImage(Covers[1]);
            //}
        }

        private BitmapImage ToImage(byte[] array) {
            using (var ms = new System.IO.MemoryStream(array)) {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        public bool IsParsingSuccessful() {
            return DataParsing.IsParsingSuccessful;
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
