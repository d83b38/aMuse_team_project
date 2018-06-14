using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using aMuse.Core.Library;
using System.Windows.Threading;
using System.Windows.Forms;
using aMuse.Core.Utils;
using aMuse.Core.Interfaces;

namespace aMuse.UI
{
    public partial class MainWindow : Window
    {
        private static MainWindow instance;

        private List<BitmapImage> Covers { get; set; }
        private DispatcherTimer playerTimer = new DispatcherTimer();
        private DispatcherTimer trackTimeTimer = new DispatcherTimer();
        private IAudio _currentAudio;
        private ObservableList<IAudio> _tracks;

        private bool geniusInfoAvailable = false;

        private MainWindow()
        {
            InitializeComponent();

            InitializeSystem();
            Player.MediaPlayer.VlcLibDirectory = new DirectoryInfo("libvlc/win-x86");
            Player.MediaPlayer.EndInit();
            EnableTimer();
        }

        public static MainWindow GetInstance()
        {
            if (instance == null)
            {
                instance = new MainWindow();
            }
            return instance;
        }

        /// <summary>
        /// Sets proper state for "add to playlist" button for current playing audio after track removal
        /// </summary>
        internal void SetProperFavState(IAudio removedTrack)
        {
            if (removedTrack == _currentAudio)
            {
                addToFavs.IsChecked = false;
                _tracks = null;
            }
        }

        /// <summary>
        /// Sets proper state for "add to playlist" button for current playing audio after playlist removal
        /// </summary>
        internal void SetProperFavState(Playlist removedPlaylist)
        {
            if (removedPlaylist.Tracks.Contains(_currentAudio))
            {
                addToFavs.IsChecked = false;
                _tracks = null;
            }
        }

        internal void SetLibraryEmpty()
        {
            _tracks = null;
        }

        private void InitializeSystem()
        {
            SystemState.Deserialize();
            Library.Update(SystemState.Instance.LibraryPath);
            PlaylistLibrary.Deserialize();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            PlaylistsPage.GetInstance().ClosePlaylistAdd();
            SystemState.Serialize();
            PlaylistLibrary.Serialize();
        }

        /// <summary>
        /// Plays the audio file chosen by user
        /// </summary>
        /// <param name="tracks">the list containing the audio file (e. g. playlist or library)</param>
        public async void SetAudio(IAudio audio, ObservableList<IAudio> tracks)
        {
            imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Pause.png"));
            Player.MediaPlayer.SetMedia(new Uri(audio.FilePath));
            _tracks = tracks;
            _currentAudio = audio;
            TrackBar.IsEnabled = true;
            _currentAudio.GetData();
            Player.MediaPlayer.Play();
            SetFavsState();
            StartTimers();
            SettingMaximum();
            try
            {
                var TrackData = await audio.GetTrackTaskAsync();

                if(TrackData != null)
                {
                    geniusInfoAvailable = true;
                    infoBoxArtist.Text = TrackData.Artist.Name;
                    infoBoxTrackName.Text = TrackData.Title;
                    Thumbnail.Source = await _currentAudio.GetImageTaskAsync(TrackData.AlbumCoverThumbnailUrl);
                    MainPage.GetInstance().Update(_currentAudio);
                }
                else
                {
                    geniusInfoAvailable = false;
                    if (_tracks == Library.Files)
                    {
                        MainFrame.Content = MusicLibrary.GetInstance();
                    }
                    else
                    {
                        MainFrame.Content = PlaylistPage.GetInstance();
                    }

                    infoBoxArtist.Text = _currentAudio.Artist;
                    infoBoxTrackName.Text = _currentAudio.Track;
                    Thumbnail.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/music-record-small.png"));
                }
            }
            catch (Exception) {
                return;
            }
        }

        public void SetFavsState()
        {
            if (_currentAudio != null && PlaylistLibrary.CurrentPlaylist != null)
            {
                addToFavs.IsChecked = PlaylistLibrary.CurrentPlaylist.Tracks.Contains(_currentAudio);
            }
        }

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (!Player.MediaPlayer.IsPlaying)
            {
                if (_currentAudio != null)
                {
                    Player.MediaPlayer.Play();
                    imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Pause.png"));
                    TrackBar.IsEnabled = true;
                    StartTimers();
                    return;
                }
                else
                    return;
            }
            Player.MediaPlayer.Pause();
            StopTimers();
            imageInside.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Play.png"));
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (geniusInfoAvailable)
            {
                MainFrame.Content = MainPage.GetInstance();
            }
        }

        /// <summary>
        /// Plays next track in the list
        /// </summary>
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (_tracks != null)
            {
                IAudio audio = _tracks.GetNext(_currentAudio);
                if (audio != null)
                {
                    int index = _tracks.GetIndex(audio);
                    if (_tracks == Library.Files)
                    {
                        MusicLibrary.GetInstance().SetSelection(index);
                    }
                    else
                    {
                        PlaylistPage.GetInstance().SetSelection(index);
                    }
                    SetAudio(audio, _tracks);
                }
            }
        }

        /// <summary>
        /// Plays previous track in the list
        /// </summary>
        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (_tracks != null)
            {
                IAudio audio = _tracks.GetPrev(_currentAudio);
                if (audio != null)
                {
                    int index = _tracks.GetIndex(audio);
                    if (_tracks == Library.Files)
                    {
                        MusicLibrary.GetInstance().SetSelection(index);
                    }
                    else
                    {
                        PlaylistPage.GetInstance().SetSelection(index);
                    }
                    SetAudio(audio, _tracks);
                }
            }
        }
         
        private void Mute_Click(object sender, RoutedEventArgs e)
        {
            Player.MediaPlayer.Audio.ToggleMute();
        }

        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Player.MediaPlayer.Audio != null)
            {
                Player.MediaPlayer.Audio.Volume = (int)volumeSlider.Value;
            }
        }
 
        private void TrackBar_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            playerTimer.Stop();
        }

        private void TrackBar_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            playerTimer.Start();
            Player.MediaPlayer.Time = (long)TrackBar.Value;
        }

        private void EnableTimer()
        {
            playerTimer.Tick += DispatcherTimer_Tick;
            playerTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            trackTimeTimer.Tick += TrackTimeTimer_Tick;
            trackTimeTimer.Interval = new TimeSpan(0, 0, 1);
        }
        /// <summary>
        /// Displays correct time/Drives TrackBar during playing
        /// </summary>
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
            playerTimer.Start();
            trackTimeTimer.Start();
        }

        private void StopTimers()
        {
            playerTimer.Stop();
            trackTimeTimer.Stop();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            TrackBar.Value = Player.MediaPlayer.Time;
            if (!Player.MediaPlayer.IsPlaying &&
                (Player.MediaPlayer.Time / 1000) >= ((int)_currentAudio.Duration.TotalSeconds - 2)) {
                Next_Click(sender, e as RoutedEventArgs);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Sets TrackBar length and song duration
        /// </summary>
        private void SettingMaximum()
        {
            TrackBar.Maximum = _currentAudio.Duration.TotalMilliseconds;
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
            // no library path chosen
            if (string.IsNullOrWhiteSpace(SystemState.Instance.LibraryPath))
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.ShowDialog();

                    // user has chosen the library
                    if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        Library.Update(fbd.SelectedPath);
                        MainFrame.Content = MusicLibrary.GetInstance();
                    }
                }
            }
            else
            {
                MainFrame.Content = MusicLibrary.GetInstance();
            }
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(e.Key == Key.Space) {
                PlayPause_Click(sender, e);
                e.Handled = true;
            }
        }


        private void Button_ClickToPlaylists(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = PlaylistsPage.GetInstance();
        }

        private void AddToFavs_Click(object sender, RoutedEventArgs e)
        {
            if (_currentAudio == null)
            {
                addToFavs.IsChecked = false;
            }
            else if (PlaylistLibrary.CurrentPlaylist == null)
            {
                addToFavs.IsChecked = false;
                MainFrame.Content = PlaylistsPage.GetInstance();
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
    }
}

