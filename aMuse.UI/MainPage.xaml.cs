using aMuse.Core.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace aMuse.UI
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        IAudio _currentAudio;
        MainWindow _mainWindow;
        public MainPage(MainWindow mainWindow, IAudio currentAudio)
        {
            //posible solution for byte[] --> BitmapImage (transfer to kokoy-to klass)
            BitmapImage ToImage(byte[] array)
            {
                using (var ms = new System.IO.MemoryStream(array))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    return image;
                }
            }
            //vremenno

            /*(мне лень писать на англ.) 
             * так вот, мб придумаете способ получше "знать о текущем треке"
             * чем передавать его как связку поле-параметр 
             * пока так, я гуист я так вижу.
             * (мне надо чтоб работало прост, иначе беда)
             */
            _currentAudio = currentAudio;

            _mainWindow = mainWindow;
            InitializeComponent();

            if (_currentAudio!=null) //if track not selected cover=default cover
            {
                AlbumCover.Source = ToImage(_currentAudio.Covers[0]);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_ClickToLirics(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.Content = new LyricsPage(_mainWindow, _currentAudio.Lyrics);
        }
    }
}
