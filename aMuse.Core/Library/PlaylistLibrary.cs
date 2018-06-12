﻿using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace aMuse.Core.Library
{
    public class PlaylistLibrary
    {
        public static HashSet<Playlist> Playlists { get; set; } = new HashSet<Playlist>(){new Playlist("Add new playlist..."), new Playlist("lol")};

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
            File.WriteAllText(@"..\..\playlists.json", json);
        }

        public static void Deserialize()
        {
            try
            {
                string json = File.ReadAllText(@"..\..\playlists.json");

                Playlists = JsonConvert.DeserializeObject<HashSet<Playlist>>(json);

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
