using System.Collections.Specialized;
using System.Collections.Generic;

namespace aMuse.Core.Library
{
    public class Playlist
    {
        public string Name { get; set; }

        public AudioCollection Tracks { get; set; }

        public Playlist(string name)
        {
            Name = name;
            Tracks = new AudioCollection();
        }

        public void GetFile()
        {
        }

        public void AddTrack(AudioFileTrack track)
        {
            Tracks.Add(track);
        }

        public void RemoveTrack(AudioFileTrack track)
        {
            Tracks.Remove(track);
        }
    }
}
