using System.Windows;
using System.Windows.Controls;

namespace aMuse.UI
{
    public partial class MoreInfoPage : Page
    {
        private static MoreInfoPage instance;

        private MoreInfoPage()
        {
            InitializeComponent();
        }

        public static MoreInfoPage GetInstance()
        {
            if (instance == null)
            {
                instance = new MoreInfoPage();
            }

            return instance;
        }

        private void Button_ClickBackToMainPage(object sender, RoutedEventArgs e)
        {
            MainWindow.GetInstance().MainFrame.GoBack();
        }
    }
}
