namespace aMuse.Core.Interfaces {
    interface ITrackDataParsing {
        string GetArtist();
        string[] GetTitle();
        byte[][] GetAlbumCovers();
        string GetLyrics();
    }
}
