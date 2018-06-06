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
using System.Drawing;
using System.IO;
using aMuse.Core;
using System.Windows.Threading;
using System.Windows.Forms;

namespace aMuse.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool NeedToChangeIcon { get; set; }
        List<BitmapImage> Covers { get; set; }
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public bool TrackBarValueChangedByMouse { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new WelcomePage();
            vlcPlayer.MediaPlayer.VlcLibDirectory = new DirectoryInfo("libvlc/win-x86");
            vlcPlayer.MediaPlayer.EndInit();
            /// <summary>
            ///  How it should work, but it doesn't
            /// </summary>
            //OpenFileDialog dialog = new OpenFileDialog();
            //dialog.ShowDialog(); --> window to select a file to play 
            //var vlcMediaInstance = vlcPlayer.MediaPlayer.VlcMediaPlayer.Manager.CreateNewMediaFromPath(dialog.FileName);
            //vlcPlayer.MediaPlayer.VlcMediaPlayer.Manager.ParseMedia(vlcMediaInstance);
            //TrackBar.Maximum = vlcPlayer.MediaPlayer.GetCurrentMedia().Duration.TotalMilliseconds;
            vlcPlayer.MediaPlayer.SetMedia(new FileInfo("track.mp3"));
            var Genius = new GeniusInfoParse("Madonna", "Frozen");
            var lyrics = Genius.GetLyrics();
            Covers = Genius.GetAlbumCovers();
            Thumbnail.Source = Covers[1];
        }
        
        private void PlayPause_Click(object sender, RoutedEventArgs e) {
            if (NeedToChangeIcon == true) {  
                vlcPlayer.MediaPlayer.VlcMediaPlayer.Pause();
                imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Pause_52px.png"));
                NeedToChangeIcon = false;
                return;
            }
            if (vlcPlayer.MediaPlayer.IsPlaying == false)
            {
                vlcPlayer.MediaPlayer.Play();
                TagLib.File tagFile = TagLib.File.Create("track.mp3");
                var title = tagFile.Tag.Title.ToString();
                var artist = tagFile.Tag.Performers[0].ToString();
                var lyrics = tagFile.Tag.Lyrics;
                infoBoxArtist.Text = artist;
                infoBoxTrackName.Text = title;
                imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Pause_52px.png"));
                TrackBar.IsEnabled = true;
                StartTimer();
                return;
            }
            vlcPlayer.MediaPlayer.Pause();
            imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Play_52px.png"));
            NeedToChangeIcon = true;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Content = new MainPage(this);
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

        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (vlcPlayer.MediaPlayer.Audio != null )
                vlcPlayer.MediaPlayer.Audio.Volume = (int)volumeSlider.Value;
        }

        private void TrackBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if(TrackBarValueChangedByMouse == true) {
                vlcPlayer.MediaPlayer.Time = (long)TrackBar.Value;
            }
        }

        private void TrackBar_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            vlcPlayer.MediaPlayer.Pause();
            TrackBarValueChangedByMouse = true;
        }

        private void TrackBar_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            TrackBarValueChangedByMouse = false;
            vlcPlayer.MediaPlayer.Play();
        }

        private void StartTimer() {
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 650);
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e) {
            TrackBar.Value = vlcPlayer.MediaPlayer.Time;
            CommandManager.InvalidateRequerySuggested();
        }
        private void PlayPauseButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            TrackBar.Maximum = vlcPlayer.MediaPlayer.GetCurrentMedia().Duration.TotalMilliseconds;
        }
        /// <summary>
        /// Only thing i was able to come up with to get track length for
        /// rewinding to work properly (or kind of)
        /// </summary>
        /// 
        private void PlayPauseButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) {
            TrackBar.Maximum = vlcPlayer.MediaPlayer.GetCurrentMedia().Duration.TotalMilliseconds;
        }
    }
}

