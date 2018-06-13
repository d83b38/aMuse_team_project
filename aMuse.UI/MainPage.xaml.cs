using aMuse.Core.Utils;
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
        string Lyrics { get; set; }
        public MainPage(MainWindow mainWindow, AudioFileTrack currentAudio)
        {
            /*(мне лень писать на англ.) 
             * так вот, мб придумаете способ получше "знать о текущем треке"
             * чем передавать его как связку поле-параметр 
             * пока так, я гуист я так вижу.
             * (мне надо чтоб работало прост, иначе беда)
             */
             //я ничего лучше не придумал пока что -Илья
             //   надеюсь, мы не забудем это удалить
             // Лёль ребят, одна я не переписываюсь в комментах. - heathen
            _currentAudio = currentAudio;

            _mainWindow = mainWindow;
            InitializeComponent();
        }
        
        private void Button_ClickToLyrics(object sender, RoutedEventArgs e)
        {
            if (_currentAudio != null && _currentAudio.IsParsingSuccessful() && !string.IsNullOrWhiteSpace(Lyrics))
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
                Lyrics = await _currentAudio.SetLyricsAsync();
                if (_currentAudio != null && _currentAudio.CoverImages[0] != null)
                //if track not selected cover=default cover
                {
                    var covers = await _currentAudio.SetCoversAsync();
                    AlbumCover.Source = covers[0];
                }
            }
            catch (System.Exception ex) {
               MessageBox.Show("Oops... Something went wrong.\nCheck your internet\n" +
                    "You won't be getting any data without it", ex.Message);
            }
            
        }
    }
}
