using aMuse.Core.Model;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace aMuse.Core.Interfaces
{
    public interface ITrackDataParsing {
        //bool IsParsingSuccessful { get; }
        //Task<string> GetArtistTaskAsync();
        //Task<string[]> GetTitleTaskAsync();
        //Task<byte[][]> GetAlbumCoversTaskAsync();
        Task<Track> GetTrackTaskAsync();
        Task<BitmapImage> GetAlbumCoverTaskAsync(string url);
        Task<BitmapImage> GetAlbumCoverThumbnailTaskAsync(string url);
        Task<string> GetLyricsTaskAsync(string url);
        Task<string> GetAlbumTaskAsync(string url);
    }
}
