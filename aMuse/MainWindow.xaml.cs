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
        bool needToChangeIcon = false;
        public MainWindow()
        {
            InitializeComponent();
            vlcPlayer.MediaPlayer.VlcLibDirectory = new DirectoryInfo("libvlc/win-x86");
            vlcPlayer.MediaPlayer.EndInit();

        }

        private void PlayPause_Click(object sender, RoutedEventArgs e) {
            if (needToChangeIcon == true)
            {
                vlcPlayer.MediaPlayer.VlcMediaPlayer.Pause();
                imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Pause_52px.png"));
                needToChangeIcon =false;
                return;
            }

            if (IsPaused == false )
            {
                TagLib.File tagFile = TagLib.File.Create("track.mp3");
                var title = tagFile.Tag.Title.ToString();
                var artist = tagFile.Tag.Performers[0].ToString();

                infoBox.Text = artist+" - " +title;
                imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Pause_52px.png"));
                vlcPlayer.MediaPlayer.Play(new FileInfo("track.mp3"));

                IsPaused = true;
                return;
            }
            
            vlcPlayer.MediaPlayer.VlcMediaPlayer.Pause();
            imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Play_52px.png"));
            needToChangeIcon = true;

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

        private void Mute_Click(object sender, RoutedEventArgs e)
        {
            vlcPlayer.MediaPlayer.Audio.ToggleMute();
        }

        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (vlcPlayer.MediaPlayer.Audio!=null)
            {
                vlcPlayer.MediaPlayer.Audio.Volume = (int)volumeSlider.Value;
            }

        }
    }
}
