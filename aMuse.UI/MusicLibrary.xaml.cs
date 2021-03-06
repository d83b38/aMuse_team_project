﻿using aMuse.Core.Library;
using System.Windows.Controls;
using aMuse.Core.Utils;
using System.Windows.Forms;
using aMuse.Core.Interfaces;

namespace aMuse.UI
{
    public partial class MusicLibrary : Page
    {
        private static MusicLibrary instance;

        private MusicLibrary()
        {
            InitializeComponent();
            UpdateFiles();
        }

        public static MusicLibrary GetInstance()
        {
            if (instance == null)
            {
                instance = new MusicLibrary();
            }
            return instance;
        }

        public void UpdateFiles()
        {
            ListTracks.ItemsSource = Library.Files;
        }

        private void ListTracks_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ListTracks.SelectedItem != null)
            {
                MainWindow.GetInstance().SetAudio((IAudio)(ListTracks.SelectedItem), Library.Files);
            }
        }

        public void SetSelection(int index)
        {
            ListTracks.SelectedIndex = index;
        }

        private void OpenNewDirectory(object sender, System.Windows.RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.ShowDialog();

                if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    Library.Update(fbd.SelectedPath);
                    MainWindow.GetInstance().SetLibraryEmpty();
                    ListTracks.ItemsSource = Library.Files;
                }
            }
        }
    }
}
