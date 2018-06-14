using aMuse.Core.Interfaces;
using aMuse.Core.Utils;
using System.Windows;
using System.Windows.Controls;

namespace aMuse.UI
{
    public partial class MainPage : Page
    {
        private static MainPage instance = new MainPage();

        private IAudio _currentAudio;
        string Lyrics { get; set; }

        private MainPage()
        {
            InitializeComponent();
        }

        public static MainPage GetInstance()
        {
            return instance;
        }

        private void Button_ClickToLyrics(object sender, RoutedEventArgs e)
        {
            MainWindow.GetInstance().MainFrame.Content = LyricsPage.GetInstance();
        }

        public async void Update(IAudio audio)
        {
            _currentAudio = audio;

            try
            {
                if (_currentAudio != null && _currentAudio.TrackData != null)
                    AlbumCover.Source = await _currentAudio.GetAlbumCoverTaskAsync(_currentAudio.TrackData.AlbumCoverUrl);
                else
                {
                    return;
                }
                Lyrics = await _currentAudio.GetLyricsTaskAsync(_currentAudio.TrackData.LyricsUrl);
            }
            catch (System.Exception ex)
            {
                //MessageBox.Show("Something went wrong.\nCheck your internet\n" + ex.Message);
            }

            if (_currentAudio != null && Lyrics != null && !string.IsNullOrWhiteSpace(Lyrics))
            {
                LyricsPage.Update(Lyrics);
            }
            else
            {
                LyricsPage.Update("Couldn't find lyrics.");
            }

            MoreInfoPage.GetInstance().Update(_currentAudio);
        }

        private void Button_ClickToInfo(object sender, RoutedEventArgs e)
        {
            MainWindow.GetInstance().MainFrame.Content = MoreInfoPage.GetInstance();
        }
    }
}
