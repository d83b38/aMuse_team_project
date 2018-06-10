using Newtonsoft.Json;
using System.Collections.Generic;

namespace aMuse.Core.DTO {
    class Response {
        [JsonProperty("hits")]
        public List<Hit> Hits { get; set; }
    }
}
