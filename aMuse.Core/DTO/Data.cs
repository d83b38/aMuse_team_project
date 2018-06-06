using aMuse.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMuse.Core.DTO {
    class Data {
        [JsonProperty("response")]
        public Response Response { get; set; }
        [JsonProperty("meta")]
        public RequestStatus RequestStatus { get; set; }
    }
}
