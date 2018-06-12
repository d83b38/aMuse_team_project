using System.IO;
using System.Linq;
using aMuse.Core.Utils;

namespace aMuse.Core.Library
{
    public class Library
    {
        public static ObservableList<AudioFileTrack> Files { get; private set; }

        public Library(string Path) {
            Update(Path);
        }

        private static void SearchAudioFiles()
        {
            Files = new ObservableList<AudioFileTrack>();
            var audios = Directory.EnumerateFiles((SystemState.Instance.LibraryPath), "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".mp3") || s.EndsWith(".flac") || s.EndsWith(".wav"));

            foreach (string f in audios)
            {
                Files.Add(AudioDictionary.GetAudio(f));
            }
        }

        public static void Update(string path)
        {
            SystemState.Instance.LibraryPath = path;
            SearchAudioFiles();
        }
    }
}
