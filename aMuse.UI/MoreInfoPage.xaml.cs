using aMuse.Core.Interfaces;
using aMuse.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace aMuse.UI
{
    public partial class MoreInfoPage : Page
    {
        private static MoreInfoPage instance = new MoreInfoPage();

        private IAudio _currentAudio;

        private MoreInfoPage()
        {
            InitializeComponent();
        }

        public static MoreInfoPage GetInstance()
        {
            return instance;
        }

        private void Button_ClickBackToMainPage(object sender, RoutedEventArgs e)
        {
            MainWindow.GetInstance().MainFrame.GoBack();
        }

        public async void Update(IAudio audio)
        {
            _currentAudio = audio;

            if (_currentAudio != null && _currentAudio.TrackData != null)
            {
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

        private void Button_ClickToBrowser(object sender, RoutedEventArgs e)
        {

        }
    }
}
