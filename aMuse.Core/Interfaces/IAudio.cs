using System;
using System.Windows.Media.Imaging;

namespace aMuse.Core.Interfaces {
    public interface IAudio
    {
        TimeSpan Duration { get; set; }
        string Artist { get; set; }
        string Track { get; set; }
        string Lyrics { get; set; }
        byte[][] Covers { get; set; }
        BitmapImage[] CoverImages { get; set; }
    }
}
