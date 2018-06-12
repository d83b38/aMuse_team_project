using System.Windows;

namespace aMuse.UI
{
    /// <summary>
    /// Логика взаимодействия для AddPlaylist.xaml
    /// </summary>
    public partial class AddPlaylist : Window
    {
        public AddPlaylist()
        {
            InitializeComponent();
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.textBoxName.Text))
            {
                Core.Library.PlaylistLibrary.AddList(this.textBoxName.Text);
            }

            this.Close();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
