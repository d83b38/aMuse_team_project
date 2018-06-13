using System;
using System.IO;
using aMuse.Core.Utils;
using Newtonsoft.Json;

namespace aMuse.Core.Library
{
    public class PlaylistLibrary
    {
        /// <summary>
        /// List of playlists
        /// </summary>
        public static ObservableList<Playlist> Playlists { get; set; }

        /// <summary>
        /// Current chosen playlist
        /// </summary>
        public static Playlist CurrentPlaylist { get; set; }

        /// <summary>
        /// Adds new playlist
        /// </summary>
        /// <param name="name">name of the playlist to add</param>
        public static void AddList(string name)
        {
            Playlists.Add(new Playlist(name));
        }

        /// <summary>
        /// Removes playlist from the list
        /// </summary>
        /// <param name="playlist">the playlist to remove</param>
        public static void RemoveList(Playlist playlist)
        {
            Playlists.Remove(playlist);
            if (playlist == CurrentPlaylist)
            {
                CurrentPlaylist = null;
            }
        }

        /// <summary>
        /// Serializes the list of playlists
        /// </summary>
        public static void Serialize()
        {
            string json = JsonConvert.SerializeObject(Playlists);
            File.WriteAllText(@"..\..\playlists.json", json);
        }

        /// <summary>
        /// Deserialize the list of playlists
        /// </summary>
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
                // it's okay if the exception is thrown, as it happens if no "playlists.json" is found
            }
        }
    }
}
