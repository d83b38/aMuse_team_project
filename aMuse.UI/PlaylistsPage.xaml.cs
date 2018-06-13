using aMuse.Core.Library;
using System.Windows.Controls;

namespace aMuse.UI
{
    /// <summary>
    /// Логика взаимодействия для PlaylistsPage.xaml
    /// </summary>
    public partial class PlaylistsPage : Page
    {
        private MainWindow _mainWindow;

        public PlaylistsPage(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            listPlaylists.ItemsSource = PlaylistLibrary.Playlists;
        }

        private void ListTracks_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (listPlaylists.SelectedItem != null)
            {
                Playlist list = (Playlist)(listPlaylists.SelectedItem);
                PlaylistLibrary.CurrentPlaylist = list;
                _mainWindow.MainFrame.Content = new PlaylistPage(_mainWindow);
            }
        }


        private void DeletePlaylist(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (listPlaylists.SelectedItem != null)
            {
                Playlist p = (Playlist)(listPlaylists.SelectedItem);
                _mainWindow.SetProperFavState(p);
                PlaylistLibrary.RemoveList(p);
            }
        }

        private void AddNewPlaylist(object sender, System.Windows.RoutedEventArgs e)
        {
            AddPlaylist addPlaylist = new AddPlaylist();
            addPlaylist.Show();
        }
    }
}
