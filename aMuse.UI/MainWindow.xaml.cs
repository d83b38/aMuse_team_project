﻿using System;
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
using System.Collections.ObjectModel;

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

            InitializeSystem();
            //MainFrame.Content = new MainPage(this,null);
            Player.MediaPlayer.VlcLibDirectory = new DirectoryInfo("libvlc/win-x86");
            Player.MediaPlayer.EndInit();
            SettingMaximun += OnSettingMaximum;
            EnableTimer();
        }

        private void InitializeSystem()
        {
            Core.Utils.SystemState.Deserialize();
            string path = Core.Utils.SystemState.Instance.LibraryPath;
            if (!string.IsNullOrWhiteSpace(path))
            {
                Library.Update(Core.Utils.SystemState.Instance.LibraryPath);
            }
            else
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.ShowDialog();

                    if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        Library.Update(fbd.SelectedPath);
                    }
                    else
                    {
                        Close();
                    }
                }
            }
            PlaylistLibrary.Deserialize();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Core.Utils.SystemState.Serialize();
            PlaylistLibrary.Serialize();
        }

        public void SetAudio(AudioFileTrack audio)
        {
            imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Pause_52px.png"));
            Player.MediaPlayer.SetMedia(new Uri(audio._path));
            _currentAudio = audio;
            _currentAudio.GetData();
            TrackBar.IsEnabled = true;
            Player.MediaPlayer.Play();
            if (PlaylistLibrary.CurrentPlaylist != null)
            {
                if (PlaylistLibrary.CurrentPlaylist.Tracks.Contains(audio))
                {
                    addToFavs.IsChecked = true;
                }
                else
                {
                    addToFavs.IsChecked = false;
                }
            }
            
            infoBoxArtist.Text = _currentAudio.Artist;
            infoBoxTrackName.Text = _currentAudio.Titles[0];
            Thumbnail.Source = _currentAudio.CoverImages[1];
            //}
            //catch (Exception ex) {
            //    System.Windows.MessageBox.Show("Oops... Something went wrong.\nCheck your internet\n" +
            //        "You won't be getting any data without it", ex.Message);
            //}
            StartTimers();
            SettingMaximun?.Invoke();
        }

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (!Player.MediaPlayer.IsPlaying)
            {
                if (_currentAudio != null) {
                    Player.MediaPlayer.Play();
                    imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Pause_52px.png"));
                    TrackBar.IsEnabled = true;
                    StartTimers();
                    return;
                }
                else
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
            PlayerTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
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
            if (!Player.MediaPlayer.IsPlaying &&
                (Player.MediaPlayer.Time / 1000) >= ((int)_currentAudio.Duration.TotalSeconds - 2)) {
                Player.MediaPlayer.Stop();
                imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Play_52px.png"));
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void OnSettingMaximum() {
            TrackBar.Maximum = _currentAudio.Duration.TotalMilliseconds;
            var titles = _currentAudio.Lyrics;
            double minutes = _currentAudio.Duration.Minutes;
            double seconds = _currentAudio.Duration.Seconds;
            if (minutes < 10 && seconds < 10)
                textBlockDuration.Text = $"0{minutes}:0{seconds}";
            else if(minutes > 10 && seconds < 10)
                textBlockDuration.Text = $"{minutes}:0{seconds}";
            else if(minutes < 10 && seconds > 10)
                textBlockDuration.Text = $"0{minutes}:{seconds}";
            else
                textBlockDuration.Text = $"{minutes}:{seconds}";
           
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

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(e.Key == Key.Space) {
                PlayPause_Click(sender, e);
                e.Handled = true;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e) {
            StopTimers();
            Player.MediaPlayer.Stop();
            TrackBar.Value = 0;
            imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Play_52px.png"));
        }

        private void Button_ClickToPlaylists(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new PlaylistsPage(this);
        }

        private void Favorite_Add(object sender, MouseButtonEventArgs e)
        {
            if (PlaylistLibrary.CurrentPlaylist != null && _currentAudio != null)
            {
                //if (!_addedToFavs)
                {
                    PlaylistLibrary.CurrentPlaylist.AddTrack(_currentAudio);
                }
               // else
                {
                    PlaylistLibrary.CurrentPlaylist.RemoveTrack(_currentAudio);
                }
                //_addedToFavs = !_addedToFavs;
            }

        }

        private void AddToFavs_Click(object sender, RoutedEventArgs e)
        {
            if (_currentAudio == null || PlaylistLibrary.CurrentPlaylist == null)
            {
                addToFavs.IsChecked = false;
            }
            else
            {
                if (addToFavs.IsChecked == false)
                {
                    PlaylistLibrary.CurrentPlaylist.RemoveTrack(_currentAudio);
                }
                else
                {
                    PlaylistLibrary.CurrentPlaylist.AddTrack(_currentAudio);
                }
            }
        }

        private void AddToFavs_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}

