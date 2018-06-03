using System.IO;
using System.Drawing;

namespace aMuse.Core.Library
{
    class AudioFile
    {
        public readonly string path;
        public readonly string album;
        public readonly string firstGenre;

        /// let it still be here, later it should be probably merged with <code>GeniusInfoParse</code> class
        public GeniusInfoParse geanyInfo;

        public readonly Image cover;

        public AudioFile(string path)
        {
            this.path = path;

            TagLib.File file = TagLib.File.Create(path);

            geanyInfo = new GeniusInfoParse(file.Tag.FirstPerformer, file.Tag.Title);

            album = file.Tag.Album;
            firstGenre = file.Tag.FirstGenre;

            if (file.Tag.Pictures.Length > 0 && file.Tag.Pictures[0] != null)
            {
                MemoryStream ms = new MemoryStream(file.Tag.Pictures[0].Data.Data);
                cover = Image.FromStream(ms);
            }
        }
    }
}
