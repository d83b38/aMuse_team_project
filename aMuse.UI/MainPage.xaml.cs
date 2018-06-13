﻿using aMuse.Core.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace aMuse.UI
{
    public partial class MainPage : Page
    {
        private static MainPage instance;

        private MusicFile _currentAudio;
        private string Lyrics { get; set; }

        private MainPage()
        {
            InitializeComponent();
        }

        public static MainPage GetInstance(MusicFile audio)
        {
            if (instance == null)
            {
                instance = new MainPage();
            }
            instance._currentAudio = audio;

            return instance;
        }
        
        private void Button_ClickToLyrics(object sender, RoutedEventArgs e)
        {
            if (_currentAudio != null && Lyrics != null && !string.IsNullOrWhiteSpace(Lyrics))
            {
                MainWindow.GetInstance().MainFrame.Content = LyricsPage.GetInstance(Lyrics);
            }
            else
            {
                MainWindow.GetInstance().MainFrame.Content = LyricsPage.GetInstance("Couldn't find lyrics for this song.");
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            try {
                if (_currentAudio != null)
                    AlbumCover.Source = await _currentAudio.GetAlbumCoverTaskAsync(_currentAudio.TrackData.AlbumCoverUrl);
                else {
                    //set defaullt cover or this will catch an exception
                    //AlbumCover.Source = new BitmapImage(new Uri("../../Icons/music-record-big.png", UriKind.Relative));
                }
                Lyrics = await _currentAudio.GetLyricsTaskAsync(_currentAudio.TrackData.LyricsUrl);
            }
            catch (System.Exception ex)
            {
               MessageBox.Show("Oops... Something went wrong.\nCheck your internet\n" +
                    "You won't be getting any data without it", ex.Message);
            }
            
        }

        private void Button_ClickToInfo(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.Content = new MoreInfoPage(_mainWindow);
        }
    }
}
