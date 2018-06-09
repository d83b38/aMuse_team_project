using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMuse.Core.Model {
    public class Track {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleWithFeatured { get; set; }
        public string LyricsUrl { get; set; }
        public Artist Artist { get; set; }
        public string AlbumCoverUrl { get; set; }
        public string AlbumCoverThumbnailUrl{ get; set; }
    }
}
