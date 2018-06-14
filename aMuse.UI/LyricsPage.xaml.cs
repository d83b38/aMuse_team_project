using System.Windows;
using System.Windows.Controls;

namespace aMuse.UI
{
    public partial class LyricsPage : Page
    {
        private static LyricsPage instance = new LyricsPage();

        private LyricsPage()
        {
            InitializeComponent();
            /* if smth wrong use this to test lyrics
           TagLib.File tagFile = TagLib.File.Create("track.mp3");
           var lyrics = tagFile.Tag.Lyrics;
           liricsBox.Text = lyrics;*/
        }

        public static LyricsPage GetInstance()
        {
            return instance;
        }

        public static void Update(string lyrics)
        {
            instance.lyricsBox.Text = lyrics;
        }

        private void Button_ClickBack(object sender, RoutedEventArgs e)
        {
            MainWindow.GetInstance().MainFrame.GoBack();
        }
    }
}
