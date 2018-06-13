using System.Collections.Generic;

namespace aMuse.Core.Utils
{
    /// <summary>
    /// Dictionary created to store <code>MusicFile</code> with unique paths
    /// </summary>
    public class AudioDictionary
    {
        private static readonly Dictionary<string, MusicFile> _audios = new Dictionary<string, MusicFile>();

        /// <summary>
        /// Get audio file by it's path
        /// </summary>
        /// <param name="path">path of the audio file</param>
        /// <returns><code>MusicFile</code></returns>
        public static MusicFile GetAudio(string path)
        {
            if (!_audios.ContainsKey(path))
            {
                _audios.Add(path, new MusicFile(path));
            }

            return _audios[path];
        }
    }
}
