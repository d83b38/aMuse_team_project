﻿using System;
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
    public partial class MainWindow : Window
    {
        List<BitmapImage> Covers { get; set; }
        DispatcherTimer PlayerTimer = new DispatcherTimer();
        DispatcherTimer TrackTimeTimer = new DispatcherTimer();
        IAudio currentAudio;
        public Action SettingMaximun;
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new WelcomePage();
            Player.MediaPlayer.VlcLibDirectory = new DirectoryInfo("libvlc/win-x86");
            Player.MediaPlayer.EndInit();
            SettingMaximun += OnSettingMaximum;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            if(dialog.FileName != "") {
                Player.MediaPlayer.SetMedia(new Uri(dialog.FileName));
                currentAudio = new AudioFileTrack(dialog.FileName);
                currentAudio.NowPlaying = true;
            }
            else {
                Close();
            }
            
            infoBoxArtist.Text = currentAudio.Artist;
            infoBoxTrackName.Text = currentAudio.Track;
            EnableTimer();
        }
        
        private void PlayPause_Click(object sender, RoutedEventArgs e) {
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
            Player.MediaPlayer.Audio.ToggleMute();
        }

        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> e) {
            if (Player.MediaPlayer.Audio != null )
                Player.MediaPlayer.Audio.Volume = (int)volumeSlider.Value;
        }
 
        private void TrackBar_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            PlayerTimer.Stop();
        }

        private void TrackBar_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            PlayerTimer.Start();
            Player.MediaPlayer.Time = (long)TrackBar.Value;
        }

        private void EnableTimer() {
            PlayerTimer.Tick += DispatcherTimer_Tick;
            PlayerTimer.Interval = new TimeSpan(0, 0, 0, 0, 400);
            TrackTimeTimer.Tick += TrackTimeTimer_Tick;
            TrackTimeTimer.Interval = new TimeSpan(0, 0, 1);
            
        }

        private void TrackTimeTimer_Tick(object sender, EventArgs e) {
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

        private void StartTimers() {
            PlayerTimer.Start();
            TrackTimeTimer.Start();
        }

        private void StopTimers() {
            PlayerTimer.Stop();
            TrackTimeTimer.Stop();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e) {
            TrackBar.Value = Player.MediaPlayer.Time;
            if ((Player.MediaPlayer.Time / 1000) == (currentAudio.Duration.TotalSeconds - 1)) {
                Player.MediaPlayer.Stop();
                imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Play_52px.png"));
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void OnSettingMaximum() {
            TrackBar.Maximum = currentAudio.Duration.TotalMilliseconds;
        }

    }
}

