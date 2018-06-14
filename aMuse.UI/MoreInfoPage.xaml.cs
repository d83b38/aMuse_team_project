using aMuse.Core.Interfaces;
using aMuse.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace aMuse.UI
{
    /// <summary>
    /// Логика взаимодействия для MoreInfoPage.xaml
    /// </summary>
    public partial class MoreInfoPage : Page
    {
        MainWindow _mainWindow;
        AudioFileTrack _currentAudio;
        public MoreInfoPage(MainWindow mainWindow, AudioFileTrack currentAudio)
        {
            _currentAudio = currentAudio;
            _mainWindow = mainWindow;
            InitializeComponent();
        }

        private void Button_ClickBackToMainPage(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.GoBack();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            if (_currentAudio != null && _currentAudio.TrackData != null) {
                string Id = _currentAudio.TrackData.Artist.Id;
                _currentAudio.TrackData.Artist = await _currentAudio.GetArtistAsync(Id);
                Description.Text = _currentAudio.TrackData.Artist.Description;
                Name.Text = _currentAudio.Artist;
                var imageUrl = _currentAudio.TrackData.Artist.ImageUrl;
                ArtistImage.Source = await _currentAudio.GetImageTaskAsync(imageUrl);
            }
            else
                return;
        }
    }
}
