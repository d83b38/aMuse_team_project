using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMuse.Core.DTO {
    class Hit {
        [JsonProperty("result")]
        public TrackInfo TrackInfo { get; set; }
    }
}
