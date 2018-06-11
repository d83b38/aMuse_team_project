using aMuse.Core.Model;
using System.Threading.Tasks;

namespace aMuse.Core.Interfaces
{
    interface ITrackDataParsing {
        bool IsParsingSuccessful { get; }
        Task<string> GetArtistTaskAsync();
        Task<string[]> GetTitleTaskAsync();
        Task<byte[][]> GetAlbumCoversTaskAsync();
        Task<string> GetLyricsTaskAsync();
    }
}
