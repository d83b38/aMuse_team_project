using Newtonsoft.Json;

namespace aMuse.Core.DTO {
    class Hit {
        [JsonProperty("result")]
        public TrackInfo TrackInfo { get; set; }
    }
}
