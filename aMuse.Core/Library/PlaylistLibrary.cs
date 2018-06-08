using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace aMuse.Core.Library
{
    public class PlaylistLibrary
    {
        public HashSet<Playlist> Playlists { get; set; }

        public PlaylistLibrary()
        {
            Playlists = new HashSet<Playlist>();
        }

        public void AddList(string name)
        {
            Playlists.Add(new Playlist(name));
        }

        public void RemoveList(string name)
        {
            Playlists.RemoveWhere(x => x.Name == name);
        }

        public void Serialize()
        {
            string json = JsonConvert.SerializeObject(Playlists);
            System.IO.File.WriteAllText(@"..\..\playlists.json", json);
        }

        public void Deserialize()
        {
            string json = System.IO.File.ReadAllText(@"..\..\playlists.json");

            Playlists = JsonConvert.DeserializeObject<HashSet<Playlist>>(json);
        }
    }
}
