using System.Windows;

namespace aMuse.UI
{
    public partial class AddPlaylist : Window
    {
        public AddPlaylist()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates playlist
        /// </summary>
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

        private void Window_Closed(object sender, System.EventArgs e)
        {
            PlaylistsPage.GetInstance().PlaylistAddClosed();
        }
    }
}
