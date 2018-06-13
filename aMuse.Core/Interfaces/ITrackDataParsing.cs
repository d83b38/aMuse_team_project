using aMuse.Core.Model;
using System.Threading.Tasks;

namespace aMuse.Core.Interfaces
{
    interface ITrackDataParsing {
        bool IsParsingSuccessful { get; }
        Task<string> GetArtistTaskAsync();
        Task<string[]> GetTitleTaskAsync();
        Task<string[]> GetLyricsAndAlbumTaskAsync();
        Task<byte[][]> GetAlbumCoversTaskAsync();
    }
}
