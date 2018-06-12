using System.IO;
using System.Linq;
using System.Collections.Generic;
using aMuse.Core.Utils;

namespace aMuse.Core.Library
{
    public class Library
    {
        private static List<AudioFileTrack> _files;
        public static List<AudioFileTrack> Files
        {
            get
            {
                return _files;
            }
        }

        public Library(string Path) {
            Update(Path);
        }

        private static void SearchAudioFiles()
        {
            _files = new List<AudioFileTrack>();
            var audios = Directory.EnumerateFiles((SystemState.Instance.LibraryPath), "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".mp3") || s.EndsWith(".flac") || s.EndsWith(".wav"));

            foreach (string f in audios)
            {

                Files.Add(new AudioFileTrack(f));
            }
            System.Console.WriteLine("lol" + Files.Count);
        }

        public static void Update(string path)
        {
            SystemState.Instance.LibraryPath = path;
            SearchAudioFiles();
        }
    }
}
