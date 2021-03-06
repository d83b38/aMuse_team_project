﻿using aMuse.Core.Interfaces;
using aMuse.Core.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace aMuse.Core.Library
{
    public class Playlist
    {
        /// <summary>
        /// Name of the playlist
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// List of tracks in the playlist
        /// </summary>
        [JsonIgnore]
        public ObservableList<IAudio> Tracks { get; private set; }

        /// <summary>
        /// List of paths for tracks in the playlist
        /// </summary>
        public List<string> Paths { get; private set; }

        public Playlist(string name)
        {
            Name = name;
            Tracks = new ObservableList<IAudio>();
            Paths = new List<string>();
        }

        /// <summary>
        /// Removes all paths that don't exist and fills the list of tracks according to paths remainig
        /// </summary>
        public void Recover()
        {
            Paths.RemoveAll(path => !File.Exists(path));

            foreach (string path in Paths)
            {
                Tracks.Add(AudioDictionary.GetAudio(path));
            }
        }

        /// <summary>
        /// Adds track to the list
        /// </summary>
        /// <param name="track">track to add</param>
        public void AddTrack(IAudio track)
        {
            Tracks.Add(track);
            Paths.Add(track.FilePath);
        }

        /// <summary>
        /// Removes track from the list
        /// </summary>
        /// <param name="track">track to remove</param>
        public void RemoveTrack(IAudio track)
        {
            Tracks.Remove(track);
            Paths.Remove(track.FilePath);
        }
    }
}
