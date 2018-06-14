using System.IO;
using System.Linq;
using aMuse.Core.Utils;

namespace aMuse.Core.Library
{
    public class Library
    {
        /// <summary>
        /// Set of audiofiles in library folder
        /// </summary>
        public static ObservableList<AudioFileTrack> Files { get; private set; }

        /// <summary>
        /// Searches for the audiofiles in the chosen folder
        /// </summary>
        private static void SearchAudioFiles()
        {
            Files = new ObservableList<AudioFileTrack>();

            if (SystemState.Instance.LibraryPath != null)
            {
                var audios = Directory.EnumerateFiles((SystemState.Instance.LibraryPath), "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".mp3") || s.EndsWith(".flac") || s.EndsWith(".wav"));

                foreach (string f in audios)
                {
                    Files.Add(AudioDictionary.GetAudio(f));
                }
            }
        }

        /// <summary>
        /// Updates music library path and fills the list of files
        /// </summary>
        /// <param name="path">new music library path</param>
        public static void Update(string path)
        {
            SystemState.Instance.LibraryPath = path;
            SearchAudioFiles();
        }
    }
}
