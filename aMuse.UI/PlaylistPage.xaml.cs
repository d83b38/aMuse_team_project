using aMuse.Core.Library;
using aMuse.Core.Utils;
using System.Windows.Controls;
using System.Windows.Input;

namespace aMuse.UI
{
    public partial class PlaylistPage : Page
    {
        private static PlaylistPage instance;

        private PlaylistPage()
        {
            InitializeComponent();
        }

        public static PlaylistPage GetInstance()
        {
            if (instance == null)
            {
                instance = new PlaylistPage();
            }
            instance.ListTracks.ItemsSource = PlaylistLibrary.CurrentPlaylist.Tracks;
            return instance;
        }

        private void ListTracks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListTracks.SelectedItem != null)
            {
                MainWindow.GetInstance().SetAudio((AudioFileTrack)(ListTracks.SelectedItem), PlaylistLibrary.CurrentPlaylist.Tracks);
            }
        }

        public void SetSelection(int index)
        {
            ListTracks.SelectedIndex = index;
        }

        private void DeleteTrackFromPlaylist(object sender, MouseButtonEventArgs e)
        {
            if (ListTracks.SelectedItem != null)
            {
                MainWindow.GetInstance().SetProperFavState((AudioFileTrack)(ListTracks.SelectedItem));
                PlaylistLibrary.CurrentPlaylist.RemoveTrack((AudioFileTrack)(ListTracks.SelectedItem));
            }
        } 
    }
}
