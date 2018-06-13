using aMuse.Core.Utils;
using System.Windows;
using System.Windows.Controls;

namespace aMuse.UI
{
    public partial class MainPage : Page
    {
        private static MainPage instance;

        private MusicFile _currentAudio;
        private string Lyrics { get; set; }

        private MainPage()
        {
            InitializeComponent();
        }

        public static MainPage GetInstance(MusicFile audio)
        {
            if (instance == null)
            {
                instance = new MainPage();
            }
            instance._currentAudio = audio;

            return instance;
        }
        
        private void Button_ClickToLyrics(object sender, RoutedEventArgs e)
        {
            if (_currentAudio != null && _currentAudio.IsParsingSuccessful() && !string.IsNullOrWhiteSpace(Lyrics))
            {
                MainWindow.GetInstance().MainFrame.Content = LyricsPage.GetInstance(Lyrics);
            }
            else
            {
                MainWindow.GetInstance().MainFrame.Content = LyricsPage.GetInstance("Couldn't find lyrics for this song.");
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Lyrics = await _currentAudio.SetLyricsAsync();
                if (_currentAudio != null && _currentAudio.CoverImages[0] != null)
                //if track not selected cover=default cover
                {
                    var covers = await _currentAudio.SetCoversAsync();
                    AlbumCover.Source = covers[0];
                }
            }
            catch (System.Exception ex)
            {
               MessageBox.Show("Oops... Something went wrong.\nCheck your internet\n" +
                    "You won't be getting any data without it", ex.Message);
            }
            
        }
    }
}
