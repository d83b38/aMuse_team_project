using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace aMuse.Core.Library
{
    public class PlaylistLibrary
    {
        public static HashSet<Playlist> Playlists { get; set; } = new HashSet<Playlist>(){new Playlist("lolList")};

        public static Playlist CurrentPlaylist { get; set; }

        public static void AddList(string name)
        {
            Playlists.Add(new Playlist(name));
        }

        public static void RemoveList(string name)
        {
            Playlists.RemoveWhere(x => x.Name == name);
        }

        public static void Serialize()
        {
            string json = JsonConvert.SerializeObject(Playlists);
            System.IO.File.WriteAllText(@"..\..\playlists.json", json);
        }

        public static void Deserialize()
        {
            try
            {
                string json = System.IO.File.ReadAllText(@"..\..\playlists.json");

                Playlists = JsonConvert.DeserializeObject<HashSet<Playlist>>(json);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
