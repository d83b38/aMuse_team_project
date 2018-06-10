using Newtonsoft.Json;

namespace aMuse.Core.DTO {
    class Data {
        [JsonProperty("response")]
        public Response Response { get; set; }
        [JsonProperty("meta")]
        public RequestStatus RequestStatus { get; set; }
    }
}
