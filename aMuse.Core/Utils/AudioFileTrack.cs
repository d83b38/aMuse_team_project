using System;
using System.IO;
using System.Windows.Media.Imaging;
using aMuse.Core.Interfaces;
using aMuse.Core.APIData;
using TagLib;
using System.Threading.Tasks;

namespace aMuse.Core.Utils
{
    
    public class AudioFileTrack : IAudio
    {
        public readonly string _path;
        public TimeSpan Duration { get; set; }
        public BitmapImage[] CoverImages { get; set; }
        public string Artist { get; set; }
        public string Track { get; set; }
        public string Lyrics { get; set; }
        public byte[][] Covers { get; set; }
        public string[] Titles { get; set; }
        private TagLib.File File { get; set; }
        private bool HasBasicTags { get; set; }
        public string Album { get; set; }

        ITrackDataParsing DataParsing;

        private bool hasInfo;

        public void GetData()
        {
            if (!hasInfo)
            {
                if (DataParsing.IsParsingSuccessful) {
                    GetInfo();
                }
                hasInfo = true;
            }
        }
       
        internal AudioFileTrack(string path) {
            _path = path;
            Covers = new byte[2][];
            CoverImages = new BitmapImage[2];
            Titles = new string[2];
            GetFile();
        }
         
        private void GetFile()
        {
            File = TagLib.File.Create(_path);
            Duration = File.Properties.Duration;

            HasBasicTags = true;
            // try getting artist and title from file tags
            //HasBasicTags = true;
            if (File.Tag.Performers.Length > 0 && !String.IsNullOrWhiteSpace(File.Tag.Performers[0]))
            {
                Artist = CleanText(File.Tag.Performers[0]);
            }
            else
            {
                HasBasicTags = false;
            }

            if (!String.IsNullOrWhiteSpace(File.Tag.Title))
            {
                Track = CleanText(File.Tag.Title);
            }
            else
            {
                HasBasicTags = false;
            }
            
                // if no artist or title tag is present, then set them based on file name
            if (!HasBasicTags)
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

        public async Task<AudioFileTrack> PopulateTrack() {
            DataParsing = new GeniusData(Artist, Track);
            Artist = await DataParsing.GetArtistTaskAsync();
            Titles = await DataParsing.GetTitleTaskAsync();
            var lyrAndAl = await DataParsing.GetLyricsAndAlbumTaskAsync();
            Lyrics = lyrAndAl[0];
            Album = lyrAndAl[1];
            Covers = await DataParsing.GetAlbumCoversTaskAsync();
            if (Covers[0] != null) {
                CoverImages[0] = ToImage(Covers[0]);
            }
            if (Covers[1] != null) {
                CoverImages[1] = ToImage(Covers[1]);
            }
            return this;
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

            if ((File.Tag.Lyrics != null && !File.Tag.Lyrics.Equals(Lyrics)) || File.Tag.Lyrics == null) {
                File.Tag.Lyrics = Lyrics;
                tagsChanged = true;
            }

            if((File.Tag.Album != null && !File.Tag.Album.Equals(Album)) || File.Tag.Album == null) {
                File.Tag.Album = Album;
                tagsChanged = true;
            }
            

            if (Covers[0] != null && Covers[1] != null) { //TODO: check if both pictures are not null -- done (?)
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

            if (tagsChanged == true) {
                File.Save();
            }
        }

        //public async Task<string> SetArtistAsync() {
        //    string artist = await DataParsing.GetArtistTaskAsync();
        //    Artist = artist;
        //    return artist;
        //}

        //public async Task<string> SetLyricsAsync() {
        //    string lyrics = await DataParsing.GetLyricsTaskAsync();
        //    Lyrics = lyrics;
        //    return lyrics;
        //}

        //public async Task<BitmapImage[]> SetCoversAsync() {
        //    Covers = await DataParsing.GetAlbumCoversTaskAsync();
        //    if (Covers[0] != null) {
        //        CoverImages[0] = ToImage(Covers[0]);
        //    }
        //    if (Covers[1] != null) {
        //        CoverImages[1] = ToImage(Covers[1]);
        //    }
        //    return CoverImages;
        //}

        //public async Task<string[]> SetTitlesAsync() {
        //    var titles = await DataParsing.GetTitleTaskAsync();
        //    Titles = titles;
        //    return titles;
        //}

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
