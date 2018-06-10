using aMuse.Core.Library;
using System.Windows;
using System.Windows.Controls;

namespace aMuse.UI
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        AudioFileTrack _currentAudio;
        MainWindow _mainWindow;

        public MainPage(MainWindow mainWindow, AudioFileTrack currentAudio)
        {
            /*(мне лень писать на англ.) 
             * так вот, мб придумаете способ получше "знать о текущем треке"
             * чем передавать его как связку поле-параметр 
             * пока так, я гуист я так вижу.
             * (мне надо чтоб работало прост, иначе беда)
             */
            _currentAudio = currentAudio;

            _mainWindow = mainWindow;
            InitializeComponent();

            if (_currentAudio != null && _currentAudio.ParsingSuccessful() && _currentAudio.CoverImages[0] != null) 
            //if track not selected cover=default cover
            {
                AlbumCover.Source = _currentAudio.CoverImages[0];
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_ClickToLirics(object sender, RoutedEventArgs e)
        {
            if (_currentAudio.ParsingSuccessful() && !string.IsNullOrWhiteSpace(_currentAudio.Lyrics))
            {
                _mainWindow.MainFrame.Content = new LyricsPage(_mainWindow, _currentAudio.Lyrics);
            }
            else
            {
                _mainWindow.MainFrame.Content = new LyricsPage(_mainWindow, "Couldn't find lyrics for this song.");
            }
        }
    }
}
