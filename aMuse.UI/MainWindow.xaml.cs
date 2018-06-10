using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using aMuse.Core.Library;
using System.Windows.Threading;
using System.Windows.Forms;
using aMuse.Core.Interfaces;
using System.Threading;

namespace aMuse.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        List<BitmapImage> Covers { get; set; }
        DispatcherTimer PlayerTimer = new DispatcherTimer();
        DispatcherTimer TrackTimeTimer = new DispatcherTimer();
        AudioFileTrack _currentAudio;
        public Action SettingMaximun;

        public MainWindow()
        {
            InitializeComponent();
            //MainFrame.Content = new MainPage(this,null);

            Player.MediaPlayer.VlcLibDirectory = new DirectoryInfo("libvlc/win-x86");
            Player.MediaPlayer.EndInit();
            SettingMaximun += OnSettingMaximum;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            if (dialog.FileName != "")
            {
                SetAudio(new AudioFileTrack(dialog.FileName));
            }
            else
            {
                Close();
            }
            
            EnableTimer();
        }

        public void SetAudio(AudioFileTrack audio)
        {
            imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Play_52px.png"));

            Player.MediaPlayer.SetMedia(new Uri(audio._path));
            
            _currentAudio = audio;
            _currentAudio.NowPlaying = true;

            if (_currentAudio.ParsingSuccessful() && _currentAudio.CoverImages[1] != null)
            {
                Thumbnail.Source = _currentAudio.CoverImages[1];
            }

            infoBoxArtist.Text = _currentAudio.Artist;
            infoBoxTrackName.Text = _currentAudio.Track;
        }
        
        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (Player.MediaPlayer.IsPlaying == false)
            {
                Player.MediaPlayer.Play();
                imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Pause_52px.png"));
                TrackBar.IsEnabled = true;
                StartTimers();
                Thread.Sleep(300);//that is wrong -- need to find a solution for that
                SettingMaximun?.Invoke();
                return;
            }
            Player.MediaPlayer.Pause();
            StopTimers();
            imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Play_52px.png"));
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Content = new MainPage(this, _currentAudio);
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Mute_Click(object sender, RoutedEventArgs e)
        {
            Player.MediaPlayer.Audio.ToggleMute();
        }

        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (Player.MediaPlayer.Audio != null)
            {
                Player.MediaPlayer.Audio.Volume = (int)volumeSlider.Value;
            }
        }
 
        private void TrackBar_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            PlayerTimer.Stop();
        }

        private void TrackBar_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            PlayerTimer.Start();
            Player.MediaPlayer.Time = (long)TrackBar.Value;
        }

        private void EnableTimer()
        {
            PlayerTimer.Tick += DispatcherTimer_Tick;
            PlayerTimer.Interval = new TimeSpan(0, 0, 0, 0, 400);
            TrackTimeTimer.Tick += TrackTimeTimer_Tick;
            TrackTimeTimer.Interval = new TimeSpan(0, 0, 1);
        }

        private void TrackTimeTimer_Tick(object sender, EventArgs e)
        {
            var seconds = (int)Player.MediaPlayer.Time / 1000;
            int minutes = seconds / 60;
            seconds = seconds - minutes * 60;
            if (minutes < 10 && seconds < 10)
                textBlockTime.Text = $"0{minutes}:0{seconds}";
            else if (minutes >= 10 && seconds < 10)
                textBlockTime.Text = $"{minutes}:0{seconds}";
            else if (minutes < 10)
                textBlockTime.Text = $"0{minutes}:{seconds}";
            else
                textBlockTime.Text = $"{minutes}:{seconds}";
            CommandManager.InvalidateRequerySuggested();
        }

        private void StartTimers()
        {
            PlayerTimer.Start();
            TrackTimeTimer.Start();
        }

        private void StopTimers()
        {
            PlayerTimer.Stop();
            TrackTimeTimer.Stop();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            TrackBar.Value = Player.MediaPlayer.Time;
            if ((Player.MediaPlayer.Time / 1000) == (_currentAudio.Duration.TotalSeconds - 1)) {
                Player.MediaPlayer.Stop();
                imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Play_52px.png"));
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void OnSettingMaximum() {
            TrackBar.Maximum = _currentAudio.Duration.TotalMilliseconds;
        }

        private void ToolTipArtist_Opened(object sender, RoutedEventArgs e)
        {
            object item = LayoutRoot.FindName(infoBoxArtist.Name);
            toolTipArtist.Content = infoBoxArtist.Text;
        }

        private void ToolTipTrack_Opened(object sender, RoutedEventArgs e)
        {
            object item = LayoutRoot.FindName(infoBoxTrackName.Name);
            toolTipTrack.Content = infoBoxTrackName.Text;
        }

        private void VolumeChanger_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Player.MediaPlayer.Audio.IsMute==true)
            {
                volumeSlider.Value = 80;
                volumeSlider.IsEnabled = true;
                volumeChanger.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Speaker_52px.png"));
                Player.MediaPlayer.Audio.IsMute = false;
            }
            else
            {
                Player.MediaPlayer.Audio.IsMute = true;
                volumeSlider.Value = 0;
                volumeSlider.IsEnabled = false;
                volumeChanger.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Mute_52px.png"));
            }

        }

        private void Button_ClickToLibrary(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new MusicLibrary(this);
        }
    }
}

