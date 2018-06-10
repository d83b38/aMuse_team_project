using System.IO;
using System.Linq;
using System.Collections.Generic;
using aMuse.Core.Utils;

namespace aMuse.Core.Library
{
    class Library
    {
        private readonly string _path; // sample path "C:\\Users\\heathen\\Downloads"

        private List<AudioFileTrack> files;

        public Library(string path)
        {
            this._path = path;
            SystemState.Instance.LibraryPath = path;
            files = new List<AudioFileTrack>();
        }

        public void SearchAudioFiles()
        {
            var audios = Directory.EnumerateFiles(_path, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".mp3") || s.EndsWith(".flac") || s.EndsWith(".wav"));

            foreach (string f in audios)
            {
                files.Add(new AudioFileTrack(f));
            }
        }
    }
}
