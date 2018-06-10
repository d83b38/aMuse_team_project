using System;
using System.IO;
using System.Windows.Media.Imaging;
using aMuse.Core.Interfaces;
using aMuse.Core.APIData;
using TagLib;

namespace aMuse.Core.Library
{
    public class AudioFileTrack : IAudio
    {
        private readonly string _path;

        public TimeSpan Duration { get; set; }
        public string Artist { get; set; }
        public string Track { get; set; }
        public string Lyrics { get; set; }
        public byte[][] Covers { get; set; }
        public BitmapImage[] CoverImages { get; set; }

        private TagLib.File File { get; set; }
        ITrackDataParsing DataParsing;

        private bool nowPlaying;
        private bool hasInfo;

        public bool NowPlaying
        { 
            get
            {
                return nowPlaying;
            }
            set
            {
                nowPlaying = value;

                if (nowPlaying && !hasInfo)
                {
                    GetInfo();
                    hasInfo = true;
                }
            }
        }
       
        public AudioFileTrack(string path)
        { 
            _path = path;
            GetFile();
        }

       public void GetFile()
       {
            Covers = new byte[2][];
            NowPlaying = false;

            File = TagLib.File.Create(_path);
            Duration = File.Properties.Duration;
            bool hasInfo = true;

            if (File.Tag.Performers.Length > 0 && !String.IsNullOrWhiteSpace(File.Tag.Performers[0]))
            {
                Artist = CleanText(File.Tag.Performers[0]);
            }
            else
            {
                hasInfo = false;
            }

            if (!String.IsNullOrWhiteSpace(File.Tag.Title))
            {
                Track = CleanText(File.Tag.Title);
            }
            else
            {
                hasInfo = false;
            }

            if (!hasInfo)
            {
                string[] info = (Path.GetFileNameWithoutExtension(_path)).Split('-');
                Artist = info[0];
                File.Tag.AlbumArtists = new string[1] { info[0] };
                if (info.Length > 1)
                {
                    Track = info[1];
                    File.Tag.Title = info[1];
                }
                SaveFile();
            }
            DataParsing = new GeniusData(Artist, Track);
        }

        public void SaveFile()
        {
            File.Save();
        }
        
        public void GetInfo()
        {
            if ( Artist != DataParsing.GetArtist())
            {
               Artist = DataParsing.GetArtist();
            }

            Lyrics = DataParsing.GetLyrics();

            Covers = DataParsing.GetAlbumCovers();

            File.Tag.AlbumArtists = new string[1] { Artist };
            File.Tag.Title = Track;
            File.Tag.Lyrics = Lyrics;

             // TODO: check if both pictures are not null
            File.Tag.Pictures = new IPicture[2] { new Picture(new ByteVector(Covers[0])),
                                                  new Picture(new ByteVector(Covers[1]))};
            SaveFile();

            if (Covers[0] != null)
            {
                CoverImages[0] = ToImage(Covers[0]);
            }
            if (Covers[1] != null)
            {
                CoverImages[1] = ToImage(Covers[1]);
            }
        }

        private BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        private string CleanText(string uncleaned)
        {
            var eraseAfter = "";
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
