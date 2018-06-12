using aMuse.Core.Interfaces;
using System.Collections.Generic;

namespace aMuse.Core.Library
{
    public class Playlist
    {
        public string Name { get; set; }

        public HashSet<AudioFileTrack> Tracks { get; set; }

        public Playlist(string name)
        {
            Name = name;
        }
        public void GetFile()
        {
            Tracks = new HashSet<AudioFileTrack>();
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
