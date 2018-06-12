using aMuse.Core.Library;
using System.Windows.Controls;
using aMuse.Core.Utils;

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

        private void ListTracks_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ListTracks.SelectedItem != null)
            {
                _mainWindow.SetAudio((AudioFileTrack)(ListTracks.SelectedItem), Library.Files);
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
