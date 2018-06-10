using aMuse.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace aMuse.UI
{
    /// <summary>
    /// Логика взаимодействия для MusicLibrary.xaml
    /// </summary>
    public partial class MusicLibrary : Page
    {
        public Library CurrentLibrary { get; set; }
        public MusicLibrary()
        {
            //Library lib= new Library("C:\\Users\\Даниил\\Desktop\\testedLib");
            CurrentLibrary = new Library("C:\\Users\\Даниил\\Desktop\\testedLib" ); //my folder yopta
            CurrentLibrary.SearchAudioFiles();
            InitializeComponent();
            listTracks.ItemsSource = CurrentLibrary.files ;
        }

        private void ListTracks_TrackSelected(object sender, RoutedEventArgs e)
        {

        }
    }
}
