using System.Windows;
using System.Windows.Controls;

namespace aMuse.UI
{
    public partial class LyricsPage : Page
    {
        private static LyricsPage instance = new LyricsPage();

        private LyricsPage()
        {
            InitializeComponent();
        }

        public static LyricsPage GetInstance()
        {
            return instance;
        }

        public static void Update(string lyrics)
        {
            instance.lyricsBox.Text = lyrics;
        }

        private void Button_ClickBack(object sender, RoutedEventArgs e)
        {
            MainWindow.GetInstance().MainFrame.GoBack();
        }
    }
}
