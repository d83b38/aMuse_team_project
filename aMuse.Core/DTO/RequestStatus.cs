﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMuse.Core.DTO {
    class RequestStatus {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
