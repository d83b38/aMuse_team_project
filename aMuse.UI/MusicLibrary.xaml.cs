using aMuse.Core.Library;
using System.Windows.Controls;
using System.Linq;

namespace aMuse.UI
{
    /// <summary>
    /// Логика взаимодействия для MusicLibrary.xaml
    /// </summary>
    public partial class MusicLibrary : Page
    {
        private MainWindow _mainWindow;

        public MusicLibrary(MainWindow mainWindow)
        {
            this._mainWindow = mainWindow;
            InitializeComponent();
            ListTracks.ItemsSource = Library.Files;
        }

        private async void ListTracks_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ListTracks.SelectedItem != null)
            {
                var selectedAudio = (AudioFileTrack)(ListTracks.SelectedItem);
                var pupulatedAudio = await selectedAudio.PopulateTrack();
                _mainWindow.SetAudio(pupulatedAudio);
            }
        }

        //private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var newSelectedItem = e.AddedItems.FirstOrDefault();
        //    if (newSelectedItem != null)
        //    {
        //        (sender as ListBox).ScrollIntoView(newSelectedItem);
        //    }
        //}

        private void OpenNewDirectory(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
