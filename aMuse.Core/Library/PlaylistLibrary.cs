using System;
using System.IO;
using Newtonsoft.Json;

namespace aMuse.Core.Library
{
    public class PlaylistLibrary
    {
        public static PlaylistCollection Playlists { get; set; }

        public static Playlist CurrentPlaylist { get; set; }

        public static void AddList(string name)
        {
            Playlists.Add(new Playlist(name));
        }

        public static void RemoveList(Playlist playlist)
        {
            Playlists.Remove(playlist);
        }

        public static void Serialize()
        {
            string json = JsonConvert.SerializeObject(Playlists);
            File.WriteAllText(@"..\..\playlists.json", json);
        }

        public static void Deserialize()
        {
            try
            {
                string json = File.ReadAllText(@"..\..\playlists.json");

                Playlists = JsonConvert.DeserializeObject<PlaylistCollection>(json);

                foreach (Playlist p in Playlists)
                {
                    p.Recover();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
