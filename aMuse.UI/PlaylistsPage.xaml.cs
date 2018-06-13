using aMuse.Core.Library;
using System.Windows.Controls;

namespace aMuse.UI
{
    public partial class PlaylistsPage : Page
    {
        private static PlaylistsPage instance;
        private AddPlaylist addPlaylist = null;

        private PlaylistsPage()
        {
            InitializeComponent();
            listPlaylists.ItemsSource = PlaylistLibrary.Playlists;
        }

        public static PlaylistsPage GetInstance()
        {
            if (instance == null)
            {
                instance = new PlaylistsPage();
            }
            return instance;
        }

        private void ListTracks_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (listPlaylists.SelectedItem != null)
            {
                Playlist list = (Playlist)(listPlaylists.SelectedItem);
                PlaylistLibrary.CurrentPlaylist = list;
                MainWindow.GetInstance().MainFrame.Content = PlaylistPage.GetInstance();
            }
        }

        private void DeletePlaylist(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (listPlaylists.SelectedItem != null)
            {
                Playlist p = (Playlist)(listPlaylists.SelectedItem);
                MainWindow.GetInstance().SetProperFavState(p);
                PlaylistLibrary.RemoveList(p);
            }
        }

        private void AddNewPlaylist(object sender, System.Windows.RoutedEventArgs e)
        {
            if (PlaylistLibrary.Playlists == null)
            {
                PlaylistLibrary.Playlists = new Core.Utils.ObservableList<Playlist>();
                listPlaylists.ItemsSource = PlaylistLibrary.Playlists;
            }
            if (addPlaylist == null)
            {
                addPlaylist = new AddPlaylist();
                addPlaylist.Show();
            }
        }

        public void PlaylistAddClosed()
        {
            addPlaylist = null;
        }

        public void ClosePlaylistAdd()
        {
            if (addPlaylist != null)
            {
                addPlaylist.Close();
            }
        }
    }
}
