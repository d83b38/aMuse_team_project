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
            System.Console.WriteLine("here" + PlaylistLibrary.CurrentPlaylist.Tracks.Count);
            PlaylistLibrary.CurrentPlaylist.RemoveTrack((AudioFileTrack)(ListTracks.SelectedItem));
            System.Console.WriteLine(PlaylistLibrary.CurrentPlaylist.Tracks.Count);
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
