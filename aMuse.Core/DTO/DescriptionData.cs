using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMuse.Core.DTO {
    class DescriptionData {
        [JsonProperty("plain")]
        public string Text { get; set; }
    }
}
