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
    /// Логика взаимодействия для PlaylistPage.xaml
    /// </summary>
    public partial class PlaylistPage : Page
    {
        
        public PlaylistPage(Playlist listOfTracks)
        {
            InitializeComponent();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ClickDeleteFromFavorites(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
