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
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class LyricsPage : Page
    {
        MainWindow _mainWindow;
        public LyricsPage(MainWindow mainWindow, string lyrics)
        {
            _mainWindow= mainWindow;
            InitializeComponent();
            liricsBox.Text = lyrics;
            /* if smth wrong use this to test lyrics
           TagLib.File tagFile = TagLib.File.Create("track.mp3");
           var lyrics = tagFile.Tag.Lyrics;
           liricsBox.Text = lyrics;*/
        }

        private void Button_ClickBack(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.GoBack();
        }
    }
}
