using Newtonsoft.Json;

namespace aMuse.Core.Utils
{
    public sealed class SystemState
    {
        private static SystemState _instance = new SystemState();

        public string LibraryPath { get; set; }

        public static SystemState Instance
        {
            get
            {
                return _instance;
            }
        }

        public static void Serialize()
        {
            string json = JsonConvert.SerializeObject(_instance);
            System.IO.File.WriteAllText(@"..\..\options.json", json);
        }

        public static void Deserialize()
        {
            string json = System.IO.File.ReadAllText(@"..\..\options.json");

            _instance = JsonConvert.DeserializeObject<SystemState>(json);
        }
    }
}
