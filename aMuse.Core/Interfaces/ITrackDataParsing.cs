using aMuse.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace aMuse.Core.Interfaces {
    interface ITrackDataParsing {
        string GetArtist();
        string[] GetTitle();
        List<byte[]> GetAlbumCovers();
        string GetLyrics();
    }
}
