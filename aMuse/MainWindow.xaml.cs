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
using Vlc.DotNet.Wpf;
using Vlc.DotNet.Core;
using System.IO;
using aMuse.UI;

namespace aMuse
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool IsPaused { get; set; }
        public MainWindow()
        {
            IsPaused = true;
            InitializeComponent();
            vlcPlayer.MediaPlayer.VlcLibDirectory = new DirectoryInfo("libvlc/win-x86");
            vlcPlayer.MediaPlayer.EndInit();
        }

        private void PlayPause_Click(object sender, RoutedEventArgs e) {
            if (IsPaused == true) {
                vlcPlayer.MediaPlayer.Play(new FileInfo("track.mp3"));
                imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Pause_52px.png"));
                IsPaused = false;
            }
            else
            {
                vlcPlayer.MediaPlayer.VlcMediaPlayer.Pause();
                IsPaused = true;
                imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Play_52px.png"));
            }

        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Content = new MainPage();

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
