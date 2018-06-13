using aMuse.Core.Model;
using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace aMuse.Core.Interfaces {
    public interface IAudio : ITrackDataParsing
    {
        TimeSpan Duration { get; set; }
        string Artist { get; set; }
        string Track { get; set; }
        Track TrackData { get; set; }
    }
}
