using System.Collections.Specialized;
using System.Collections.Generic;

namespace aMuse.Core.Library
{
    public class Playlist : INotifyCollectionChanged
    {
        public string Name { get; set; }

        public HashSet<AudioFileTrack> Tracks { get; set; }

        public Playlist(string name)
        {
            Name = name;
            Tracks = new HashSet<AudioFileTrack>();
        }

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add
            {
                // raise poperty changed event
            }

            remove
            {
                // raise poperty changed event
            }
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
