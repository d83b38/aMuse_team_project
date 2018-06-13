using Newtonsoft.Json;
using System;

namespace aMuse.Core.Utils
{
    public sealed class SystemState
    {
        public static SystemState Instance { get; private set; } = new SystemState();

        /// <summary>
        /// Current chosen path to library
        /// </summary>
        public string LibraryPath { get; set; }

        /// <summary>
        /// Serialize options
        /// </summary>
        public static void Serialize()
        {
            string json = JsonConvert.SerializeObject(Instance);
            System.IO.File.WriteAllText(@"..\..\options.json", json);
        }

        /// <summary>
        /// Deserialize options
        /// </summary>
        public static void Deserialize()
        {
            try
            {
                string json = System.IO.File.ReadAllText(@"..\..\options.json");
                Instance = JsonConvert.DeserializeObject<SystemState>(json);
            }
            catch (Exception ex)
            {
               
            }
        }
    }
}
