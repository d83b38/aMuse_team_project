using aMuse.Core.Interfaces;
using System.Collections.Generic;

namespace aMuse.Core.Library
{
    public class Playlist
    {
        public string Name { get; set; }

        public HashSet<string> Paths { get; set; }

        public Playlist(string name)
        {
            Name = name;
        }
        public void GetFile()
        {
            Paths = new HashSet<string>();
        }

        public void AddTrack(string path)
        {
            Paths.Add(path);
        }

        private void RemoveTrack(string path)
        {
            Paths.Remove(path);
        }
    }
}
