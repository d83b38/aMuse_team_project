using System.Windows;
using System.Windows.Controls;

namespace aMuse.UI
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class LyricsPage : Page
    {
        MainWindow _mainWindow;
        public LyricsPage(MainWindow mainWindow, string lyrics)
        {
            _mainWindow= mainWindow;
            InitializeComponent();
            lyricsBox.Text = lyrics;
            /* if smth wrong use this to test lyrics
           TagLib.File tagFile = TagLib.File.Create("track.mp3");
           var lyrics = tagFile.Tag.Lyrics;
           liricsBox.Text = lyrics;*/
        }

        private void Button_ClickBack(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.GoBack();
        }
    }
}
