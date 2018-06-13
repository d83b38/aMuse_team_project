using aMuse.Core.Library;
using System.Windows.Controls;
using aMuse.Core.Utils;
using System.Windows.Forms;

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
                var tracks = new ObservableList<AudioFileTrack>();
                _mainWindow.SetAudio(pupulatedAudio, tracks);
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
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.ShowDialog();

                if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    Library.Update(fbd.SelectedPath);
                    ListTracks.ItemsSource = Library.Files;
                }
            }
        }
    }
}
