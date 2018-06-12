using aMuse.Core.Library;
using aMuse.Core.Model;
using System.Collections.Generic;
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
                if (listPlaylists.SelectedIndex != 0)
                {
                    Playlist list = (Playlist)(listPlaylists.SelectedItem);
                    PlaylistLibrary.CurrentPlaylist = list;
                    _mainWindow.MainFrame.Content = new PlaylistPage(_mainWindow);
                }
                else
                {
                    AddPlaylist addPlaylist = new AddPlaylist();
                    addPlaylist.Show();
                }
            }
        }
    }
}
