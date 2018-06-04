using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using TagLib;


namespace aMuse.Core.Library
{
    class Library
    {
        private string path; // sample path "C:\\Users\\heathen\\Downloads"

        private List<AudioFile> files;

        public Library(string path)
        {
            this.path = path;
            files = new List<AudioFile>();
        }

        public void searchAudioFiles()
        {
            var audios = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".mp3") || s.EndsWith(".flac") || s.EndsWith("wav"));

            foreach (string f in audios)
            {
                files.Add(new AudioFile(f));
            }
        }
    }
}
