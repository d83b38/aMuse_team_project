using System.IO;
using System.Linq;
using System.Collections.Generic;
using aMuse.Core.Utils;

namespace aMuse.Core.Library
{
    public class Library
    {
        private readonly string _path;
        public List<AudioFileTrack> Files { get; set; }

        public Library(string path)
        {
            this._path = path;
            SystemState.Instance.LibraryPath = path;
            Files = new List<AudioFileTrack>();
        }

        public void SearchAudioFiles()
        {
            var audios = Directory.EnumerateFiles(_path, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".mp3") || s.EndsWith(".flac") || s.EndsWith(".wav"));

            foreach (string f in audios)
            {
                Files.Add(new AudioFileTrack(f));
            }
        }
    }
}
