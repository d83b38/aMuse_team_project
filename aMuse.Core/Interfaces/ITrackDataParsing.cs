namespace aMuse.Core.Interfaces
{
    interface ITrackDataParsing
    {
        bool ParsingSuccessful { get; }
        string GetArtist();
        string[] GetTitle();
        byte[][] GetAlbumCovers();
        string GetLyrics();
    }
}
