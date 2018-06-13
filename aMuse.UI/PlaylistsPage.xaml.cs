using aMuse.Core.Library;
using System.Windows.Controls;

namespace aMuse.UI
{
    /// <summary>
    /// Логика взаимодействия для PlaylistsPage.xaml
    /// </summary>
    public partial class PlaylistsPage : Page
    {
        private static PlaylistsPage instance;

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
                MainWindow.GetInstance().MainFrame.Content = new PlaylistPage(MainWindow.GetInstance());
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
            AddPlaylist addPlaylist = new AddPlaylist();
            addPlaylist.Show();
        }
    }
}
