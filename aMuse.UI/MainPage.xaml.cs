using aMuse.Core.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace aMuse.UI
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        AudioFileTrack _currentAudio;
        MainWindow _mainWindow;
        string Lyrics { get; set; }
        public MainPage(MainWindow mainWindow, AudioFileTrack currentAudio)
        {
            _currentAudio = currentAudio;

            _mainWindow = mainWindow;
            InitializeComponent();
        }
        
        private void Button_ClickToLyrics(object sender, RoutedEventArgs e)
        {
            if (_currentAudio != null && Lyrics != null && !string.IsNullOrWhiteSpace(Lyrics))
            {
                _mainWindow.MainFrame.Content = new LyricsPage(_mainWindow, Lyrics);
            }
            else
            {
                _mainWindow.MainFrame.Content = new LyricsPage(_mainWindow, "Couldn't find lyrics for this song.");
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e) {
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
            _mainWindow.MainFrame.Content = new MoreInfoPage(_mainWindow, _currentAudio);
        }
    }
}
