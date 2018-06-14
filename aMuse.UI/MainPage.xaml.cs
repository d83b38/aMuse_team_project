using aMuse.Core.Utils;
using System.Windows;
using System.Windows.Controls;

namespace aMuse.UI
{
    public partial class MainPage : Page
    {
        private static MainPage instance;

        private AudioFileTrack _currentAudio;
        string Lyrics { get; set; }

        private MainPage()
        {
            InitializeComponent();
        }

        public static MainPage GetInstance(AudioFileTrack audio)
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
            if (_currentAudio != null && Lyrics != null && !string.IsNullOrWhiteSpace(Lyrics))
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
            try {
                if (_currentAudio != null && _currentAudio.TrackData != null)
                    AlbumCover.Source = await _currentAudio.GetAlbumCoverTaskAsync(_currentAudio.TrackData.AlbumCoverUrl);
                else {
                    return;
                }
                Lyrics = await _currentAudio.GetLyricsTaskAsync(_currentAudio.TrackData.LyricsUrl);
            }
            catch (System.Exception ex) {
               MessageBox.Show("Something went wrong.\nCheck your internet\n" + ex.Message);
            }
        }

        private void Button_ClickToInfo(object sender, RoutedEventArgs e)
        {
            MainWindow.GetInstance().MainFrame.Content = MoreInfoPage.GetInstance(_currentAudio);
        }
    }
}
