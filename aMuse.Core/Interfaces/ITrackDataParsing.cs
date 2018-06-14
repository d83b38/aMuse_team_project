using aMuse.Core.Model;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace aMuse.Core.Interfaces
{
    public interface ITrackDataParsing {
        //bool IsParsingSuccessful { get; }
        Task<Track> GetTrackTaskAsync();
        Task<Artist> GetArtistAsync(string Id);
        Task<BitmapImage> GetAlbumCoverTaskAsync(string url);
        Task<BitmapImage> GetImageTaskAsync(string url);
        Task<string> GetLyricsTaskAsync(string url);
        Task<string> GetAlbumTaskAsync(string url);
    }
}
