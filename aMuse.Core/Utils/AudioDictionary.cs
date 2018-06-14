using System.Collections.Generic;

namespace aMuse.Core.Utils
{
    public class AudioDictionary
    {
        /// <summary>
        /// Dictionary created to store <code>AudioFileTrack</code> with unique paths
        /// </summary>
        private static readonly Dictionary<string, AudioFileTrack> _audios = new Dictionary<string, AudioFileTrack>();

        /// <summary>
        /// Get audio file by it's path
        /// </summary>
        /// <param name="path">path to the audio file</param>
        /// <returns><code>AudioFileTrack</code></returns>
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
