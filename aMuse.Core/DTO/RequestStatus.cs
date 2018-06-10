using Newtonsoft.Json;

namespace aMuse.Core.DTO {
    class RequestStatus {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
