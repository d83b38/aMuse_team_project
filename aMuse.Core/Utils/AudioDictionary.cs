using aMuse.Core.Interfaces;
using System.Collections.Generic;

namespace aMuse.Core.Utils
{
    public class AudioDictionary
    {
        /// <summary>
        /// Dictionary created to store <code>IAudio</code> with unique paths
        /// </summary>
        private static readonly Dictionary<string, IAudio> _audios = new Dictionary<string, IAudio>();

        /// <summary>
        /// Get audio file by it's path
        /// </summary>
        /// <param name="path">path to the audio file</param>
        /// <returns><code>IAudio</code></returns>
        public static IAudio GetAudio(string path)
        {
            if (!_audios.ContainsKey(path))
            {
                IAudio audio = new AudioFileTrack(path);
                _audios.Add(path, audio);
            }

            return _audios[path];
        }
    }
}
