using aMuse.Core.Library;
using System.Windows;
using System.Windows.Controls;

namespace aMuse.UI
{
    /// <summary>
    /// Логика взаимодействия для MusicLibrary.xaml
    /// </summary>
    public partial class MusicLibrary : Page
    {
        public Library CurrentLibrary { get; set; }

        private MainWindow _mainWindow;

        public MusicLibrary(MainWindow mainWindow)
        {
            this._mainWindow = mainWindow;
            //CurrentLibrary = new Library("C:\\Users\\Даниил\\Desktop\\testedLib" ); //my folder yopta
            //CurrentLibrary = new Library("C:\\Users\\heathen\\Downloads\\1"); //nope, my!
            CurrentLibrary = new Library("C:\\Users\\Илья\\Desktop\\music"); //gtfo beaches))))0
            //CurrentLibrary.SearchAudioFiles();
            InitializeComponent();
            ListTracks.ItemsSource = Library.Files;
        }

        private void ListTracks_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ListTracks.SelectedItem != null)
            {
                _mainWindow.SetAudio((AudioFileTrack)(ListTracks.SelectedItem));
            }
        }
    }
}
