using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace aMuse.Core.Interfaces {
    public interface IAudio : IFileManagement {
        TimeSpan Duration { get; set; }
        bool NowPlaying { get; set; }
        string Artist { get; set; }
        string Track { get; set; }
        string Lyrics { get; set; }
        List<byte[]> Covers { get; set; }
    }
}
