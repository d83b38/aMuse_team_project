using Newtonsoft.Json;

namespace aMuse.Core.DTO {
    class ArtistInfo {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public DescriptionData Description { get; set; }
        [JsonProperty("image_url")]
        public string ArtistDescriptionImageUrl { get; set; }
        [JsonProperty("url")]
        public string ArtistDescriptionGeniusUrl { get; set; }
    }
}
