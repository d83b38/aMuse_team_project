using System.Windows;
using System.Windows.Controls;

namespace aMuse.UI
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class LyricsPage : Page
    {
        private static LyricsPage instance;

        private LyricsPage()
        {
            InitializeComponent();
            /* if smth wrong use this to test lyrics
           TagLib.File tagFile = TagLib.File.Create("track.mp3");
           var lyrics = tagFile.Tag.Lyrics;
           liricsBox.Text = lyrics;*/
        }

        public static LyricsPage GetInstance(string lyrics)
        {
            if (instance == null)
            {
                instance = new LyricsPage();
            }
            instance.lyricsBox.Text = lyrics;
            return instance;
        }

        private void Button_ClickBack(object sender, RoutedEventArgs e)
        {
            MainWindow.GetInstance().MainFrame.GoBack();
        }
    }
}
