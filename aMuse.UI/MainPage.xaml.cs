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
                if (_currentAudio != null)
                    AlbumCover.Source = await _currentAudio.GetAlbumCoverTaskAsync(_currentAudio.TrackData.AlbumCoverUrl);
                else {
                    //set defaullt cover or this will catch an exception
                    //AlbumCover.Source = new BitmapImage(new Uri("../../Icons/music-record-big.png", UriKind.Relative));
                }
                Lyrics = await _currentAudio.GetLyricsTaskAsync(_currentAudio.TrackData.LyricsUrl);
            }
            catch (System.Exception ex) {
               MessageBox.Show("Oops... Something went wrong.\nCheck your internet\n" +
                    "You won't be getting any data without it", ex.Message);
            }
            
        }
    }
}
