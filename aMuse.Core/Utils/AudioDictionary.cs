using System.Collections.Generic;

namespace aMuse.Core.Utils
{
    public class AudioDictionary
    {
        private static readonly Dictionary<string, AudioFileTrack> _audios = new Dictionary<string, AudioFileTrack>();

        public static AudioFileTrack GetAudio(string path)
        {
            if (!_audios.ContainsKey(path))
            {
                _audios.Add(path, new AudioFileTrack(path));
            }

            return _audios[path];
        }
    }
}
