using System;
using System.IO;
using aMuse.Core.Utils;
using Newtonsoft.Json;

namespace aMuse.Core.Library
{
    public class PlaylistLibrary
    {
        public static ObservableList<Playlist> Playlists { get; set; }

        public static Playlist CurrentPlaylist { get; set; }

        public static void AddList(string name)
        {
            Playlists.Add(new Playlist(name));
        }

        public static void RemoveList(Playlist playlist)
        {
            Playlists.Remove(playlist);
            if (playlist == CurrentPlaylist)
            {
                CurrentPlaylist = null;
            }
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

                Playlists = JsonConvert.DeserializeObject<ObservableList<Playlist>>(json);

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
