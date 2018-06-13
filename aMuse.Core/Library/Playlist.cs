﻿using aMuse.Core.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace aMuse.Core.Library
{
    public class Playlist
    {
        public string Name { get; }

        [JsonIgnore]
        public ObservableList<AudioFileTrack> Tracks { get; private set; }

        public List<string> Paths { get; private set; }

        public Playlist(string name)
        {
            Name = name;
            Tracks = new ObservableList<AudioFileTrack>();
            Paths = new List<string>();
        }

        public void Recover()
        {
            Paths.RemoveAll(path => !File.Exists(path));

            foreach (string path in Paths)
            {
                Tracks.Add(AudioDictionary.GetAudio(path));
            }
        }

        public void GetFile()
        {
        }

        public void AddTrack(AudioFileTrack track)
        {
            Tracks.Add(track);
            Paths.Add(track._path);
        }

        public void RemoveTrack(AudioFileTrack track)
        {
            Tracks.Remove(track);
            Paths.Remove(track._path);
        }
    }
}
