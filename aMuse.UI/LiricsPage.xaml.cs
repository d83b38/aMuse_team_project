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
    public partial class LiricsPage : Page
    {
        public LiricsPage()
        {

            InitializeComponent();
            TagLib.File tagFile = TagLib.File.Create("track.mp3");
            var lirics = tagFile.Tag.Lyrics;
            liricsBox.Text = lirics;
        }
    }
}
