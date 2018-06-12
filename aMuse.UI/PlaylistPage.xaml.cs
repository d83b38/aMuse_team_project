using aMuse.Core.Library;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace aMuse.UI
{
    /// <summary>
    /// Логика взаимодействия для PlaylistPage.xaml
    /// </summary>
    public partial class PlaylistPage : Page
    {
        private MainWindow _mainWindow;

        public PlaylistPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this._mainWindow = mainWindow;
            ListTracks.ItemsSource = PlaylistLibrary.CurrentPlaylist.Tracks;
        }

        private void ClickDeleteFromFavorites(object sender, MouseButtonEventArgs e)
        {
            PlaylistLibrary.CurrentPlaylist.RemoveTrack((AudioFileTrack)(ListTracks.SelectedItem));
        }

        private void ListTracks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListTracks.SelectedItem != null)
            {
                _mainWindow.SetAudio((AudioFileTrack)(ListTracks.SelectedItem));
            }
        }
    }
}
