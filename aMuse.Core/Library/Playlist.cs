using aMuse.Core.Interfaces;
using System.Collections.Generic;

namespace aMuse.Core.Library
{
    public class Playlist : IFileManagement
    {
        public string Name { get; set; }

        public HashSet<string> paths { get; set; }

        public Playlist(string name) {
            Name = name;
        }
        public void GetFile() {
            paths = new HashSet<string>();
        }

        public void SaveFile() {
            
        }

        public void AddTrack(string path)
        {
            paths.Add(path);
        }

        private void RemoveTrack(string path)
        {
            paths.Remove(path);
        }
    }
}
