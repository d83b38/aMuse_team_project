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
            HashSet<Playlist> AllPlaylists = new HashSet<Playlist>();
            InitializeComponent();
        }

        private void ListTracks_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Playlist list = (Playlist)(listTracks.SelectedItem);
            _mainWindow.MainFrame.Content = new PlaylistPage(list);
        }
    }
}
