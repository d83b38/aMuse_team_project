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
            listTracks.ItemsSource = PlaylistLibrary.Playlists;
        }

        private void ListTracks_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (listTracks.SelectedItem != null)
            {
                Playlist list = (Playlist)(listTracks.SelectedItem);
                PlaylistLibrary.CurrentPlaylist = list;
                _mainWindow.MainFrame.Content = new PlaylistPage(_mainWindow);
            }
        }
    }
}
