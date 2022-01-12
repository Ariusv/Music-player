using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Threading;
using System.Windows.Interop;

namespace Pplayer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer timer = new DispatcherTimer();
        
        public static PLAYLIST playlist_;

       

        public MainWindow()
        {
            InitializeComponent();
            GridHome.Visibility = Visibility.Visible;
            GridPlaylist.Visibility = Visibility.Hidden;
            MainBass.InitBass(MainBass.HG);
            FileDialog.InputFormats();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            

            try
            {
                playlist_ = new PLAYLIST(FileDialog.AppPath + "playlists\\base.m3u");
                PLAYLIST.IsFirst = false;
                try
                {
                    for (int i = 0; i < playlist_.Count(); i++)
                    {

                        Tag Showed_track = new Tag(playlist_[i]);
                        PlaylistBox.Items.Add(Showed_track.Artist + " — " + Showed_track.Title);

                    }
                
                    PlaylistBox.SelectedIndex = 0;

                    Tag Showed_track_t = new Tag(playlist_[0]);

                    ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/temp.jpg")));
                    imgBrush.Stretch = Stretch.UniformToFill;


                    pictureBox.Fill = (Showed_track_t.picture != null) ? ConvertDrawingToImageBrush(new Bitmap(Showed_track_t.picture)) : imgBrush;
                    
                    ArtistTextBlock.Text = Showed_track_t.Artist;
                    TitleTextBlock.Text = Showed_track_t.Title;

                    PausePlaySong_Click(this, new RoutedEventArgs());
                }
                catch
                {
                    Imunitet();
                }

            }
            catch
            {
                playlist_.Clear();

                playlist_.Delete_pfile(FileDialog.AppPath + "playlists\\base.m3u");
            }

            SongsTextBlock.Text = "Songs: " + playlist_.Count();
        }

        public ImageBrush ConvertDrawingToImageBrush(Bitmap bitmap)
        {
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmap.Dispose();
            return new ImageBrush(bitmapSource);
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            LocalTimeLabel.Text = TimeSpan.FromSeconds(MainBass.GetPosOfStream(MainBass.Stream)).ToString();
            ProgresBar.Value = MainBass.GetPosOfStream(MainBass.Stream);


            if (MainBass.NextTrack())
            {

                PlaylistBox.SelectedIndex = playlist_.CurTrackNumber;
                RealTimeLabel.Text = TimeSpan.FromSeconds(MainBass.GetTimeOfStream(MainBass.Stream)).ToString();


                ProgresBar.Maximum = MainBass.GetTimeOfStream(MainBass.Stream);


                try
                {
                    Tag Showed_track_t = new Tag(playlist_[playlist_.CurTrackNumber]);
                    ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/temp.jpg")));
                    imgBrush.Stretch = Stretch.UniformToFill;


                    pictureBox.Fill = (Showed_track_t.picture != null) ? ConvertDrawingToImageBrush(new Bitmap(Showed_track_t.picture)) : imgBrush;
                    ArtistTextBlock.Text = Showed_track_t.Artist;
                    TitleTextBlock.Text = Showed_track_t.Title;
                }
                catch
                {
                    Imunitet();
                }
            }

            if (MainBass.EndOfPlaylist)
            {

                MainBass.Stop();
                timer.Stop();
                PlaylistBox.SelectedIndex = playlist_.CurTrackNumber = 0; ;

                ProgresBar.Value = 0;
                LocalTimeLabel.Text = "00:00:00";
                RealTimeLabel.Text = "00:00:00";

                MainBass.EndOfPlaylist = false;

                PausePlaySong.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/playM.png")));
                MainBass.img_flag = true;

                Tag Showed_track_t = new Tag(playlist_[0]);
                ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/temp.jpg")));
                imgBrush.Stretch = Stretch.UniformToFill;


                pictureBox.Fill = (Showed_track_t.picture != null) ? ConvertDrawingToImageBrush(new Bitmap(Showed_track_t.picture)) : imgBrush;
                ArtistTextBlock.Text = Showed_track_t.Artist;
                TitleTextBlock.Text = Showed_track_t.Title;
            }

        }

        private void ButtonPopUPExit_Click(object sender, RoutedEventArgs e)
        {
            playlist_.Save(FileDialog.AppPath + "playlists\\base.m3u");
            Application.Current.Shutdown();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void TopPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }


        private void TrayButton_Click(object sender, RoutedEventArgs e)
        {
             this.Hide();
        }



        private void TaskbarIcon_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            this.Show();
        }

        private void ListViewItem_MouseDown(object sender, MouseButtonEventArgs e)
        {

            //TopPanel.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#99B4D1");
            //GridMenu.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#3B5C7E");


            GridHome.Visibility = Visibility.Visible;
            GridPlaylist.Visibility = Visibility.Hidden;
        }

        private void ListViewItem_MouseDown_1(object sender, MouseButtonEventArgs e)
        {

            //TopPanel.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#17A26A");
            //GridMenu.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#06291A");


            GridHome.Visibility = Visibility.Hidden;
            GridPlaylist.Visibility = Visibility.Visible;
            
        }

        private void PausePlaySong_Click(object sender, RoutedEventArgs e)
        {

            if (MainBass.img_flag)
            {
                if ((PlaylistBox.Items.Count != 0) && (PlaylistBox.SelectedIndex != -1))
                {
                    string cur = playlist_[PlaylistBox.SelectedIndex];

                    playlist_.CurTrackNumber = PlaylistBox.SelectedIndex;

                    MainBass.Play(cur, MainBass.Volume);
                    RealTimeLabel.Text = TimeSpan.FromSeconds(MainBass.GetTimeOfStream(MainBass.Stream)).ToString();
                    

                    ProgresBar.Maximum = MainBass.GetTimeOfStream(MainBass.Stream);
                   




                    timer.Start();
                    PausePlaySong.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/pauseM.png")));
                    MainBass.img_flag = false;

                    Tag Showed_track_t = new Tag(playlist_[playlist_.CurTrackNumber]);
                    ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/temp.jpg")));
                    imgBrush.Stretch = Stretch.UniformToFill;


                    pictureBox.Fill = (Showed_track_t.picture != null) ? ConvertDrawingToImageBrush(new Bitmap(Showed_track_t.picture)) : imgBrush;
                    ArtistTextBlock.Text = Showed_track_t.Artist;
                    TitleTextBlock.Text = Showed_track_t.Title;
                }
            }
            else
            {
                MainBass.Pause();
                timer.Stop();
                PausePlaySong.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/playM.png")));
                MainBass.img_flag = true;

            }
        }

        private void ButtonNextPlaySong_Click(object sender, RoutedEventArgs e)
        {
            if ((PlaylistBox.Items.Count != 0) && (PlaylistBox.SelectedIndex != -1))
            {
                try
                {
                    MainBass.Stop();

                    playlist_.CurTrackNumber = (PlaylistBox.SelectedIndex + 1 < playlist_.Count()) ? ++PlaylistBox.SelectedIndex : 0;
                    PlaylistBox.SelectedIndex = playlist_.CurTrackNumber;

                    string cur = playlist_[PlaylistBox.SelectedIndex];

                    MainBass.Play(cur, MainBass.Volume);
                    RealTimeLabel.Text = TimeSpan.FromSeconds(MainBass.GetTimeOfStream(MainBass.Stream)).ToString();


                    ProgresBar.Maximum = MainBass.GetTimeOfStream(MainBass.Stream);


                    Tag Showed_track_t = new Tag(playlist_[playlist_.CurTrackNumber]);
                    ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/temp.jpg")));
                    imgBrush.Stretch = Stretch.UniformToFill;


                    pictureBox.Fill = (Showed_track_t.picture != null) ? ConvertDrawingToImageBrush(new Bitmap(Showed_track_t.picture)) : imgBrush;
                    ArtistTextBlock.Text = Showed_track_t.Artist;
                    TitleTextBlock.Text = Showed_track_t.Title;

                    timer.Start();
                }
                catch
                {
                   
                    Imunitet();
                    

                   

                }

            }
            else
            {
                MainBass.Stop();
                timer.Stop();


                ArtistTextBlock.Text = "Artist";
                TitleTextBlock.Text = "Title";
                ProgresBar.Value = 0;
                LocalTimeLabel.Text = "00:00:00";
                RealTimeLabel.Text = "00:00:00";
            }
        }

        void Imunitet()
        {

            for (int i = 0; i < playlist_.Count(); i++)
            {
                try
                {
                    Tag Showed_track_t = new Tag(playlist_[i]);
                }
                catch
                {
                    playlist_.Delete(i);
                    i--;
                }

            }
            PlaylistBox.Items.Clear();
            for (int i = 0; i< playlist_.Count(); i++)
            {
                Tag Showed_track = new Tag(playlist_[i]);
                PlaylistBox.Items.Add(Showed_track.Artist + " — " + Showed_track.Title);
            }

            MainBass.Stop();
            timer.Stop();


            ArtistTextBlock.Text = "Artist";
            TitleTextBlock.Text = "Title";
            ProgresBar.Value = 0;
            LocalTimeLabel.Text = "00:00:00";
            RealTimeLabel.Text = "00:00:00";
            ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/temp.jpg")));
            imgBrush.Stretch = Stretch.UniformToFill;

            PausePlaySong.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/pauseM.png")));
            MainBass.img_flag = true;

            pictureBox.Fill = imgBrush;
            MessageBox.Show("Error! Some files didn't exist! Program deleted files with Playlist! ");

            SongsTextBlock.Text = "Songs: " + playlist_.Count();
        }

        private void ButtonPrevPlaySong_Click(object sender, RoutedEventArgs e)
        {
            if ((PlaylistBox.Items.Count != 0) && (PlaylistBox.SelectedIndex != -1))
            {
                try
                {
                    MainBass.Stop();

                    playlist_.CurTrackNumber = (PlaylistBox.SelectedIndex != 0) ? --PlaylistBox.SelectedIndex : playlist_.Count() - 1;
                    PlaylistBox.SelectedIndex = playlist_.CurTrackNumber;

                    string cur = playlist_[PlaylistBox.SelectedIndex];

                    MainBass.Play(cur, MainBass.Volume);
                    RealTimeLabel.Text = TimeSpan.FromSeconds(MainBass.GetTimeOfStream(MainBass.Stream)).ToString();


                    ProgresBar.Maximum = MainBass.GetTimeOfStream(MainBass.Stream);


                    Tag Showed_track_t = new Tag(playlist_[playlist_.CurTrackNumber]);
                    ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/temp.jpg")));
                    imgBrush.Stretch = Stretch.UniformToFill;


                    pictureBox.Fill = (Showed_track_t.picture != null) ? ConvertDrawingToImageBrush(new Bitmap(Showed_track_t.picture)) : imgBrush;
                    ArtistTextBlock.Text = Showed_track_t.Artist;
                    TitleTextBlock.Text = Showed_track_t.Title;
                    timer.Start();
                }
                catch
                {
                    Imunitet();
                }
            }
            else
            {
                MainBass.Stop();
                timer.Stop();


                ArtistTextBlock.Text = "Artist";
                TitleTextBlock.Text = "Title";
                ProgresBar.Value = 0;
                LocalTimeLabel.Text = "00:00:00";
                RealTimeLabel.Text = "00:00:00";
                ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/temp.jpg")));
                imgBrush.Stretch = Stretch.UniformToFill;
                PausePlaySong.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/playM.png")));
                MainBass.img_flag = true;
                pictureBox.Fill = imgBrush;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileDialog.opf.Multiselect = true;
            FileDialog.opf.ShowDialog();
            

            if (FileDialog.opf.FileName.Contains(".m3u"))
            {
                if (PLAYLIST.IsFirst)
                {
                    try
                    {
                        playlist_ = new PLAYLIST(FileDialog.opf.FileName);
                        PLAYLIST.IsFirst = false;

                        for (int i = 0; i < playlist_.Count(); i++)
                        {
                            Tag Showed_track = new Tag(playlist_[i]);
                            PlaylistBox.Items.Add(Showed_track.Artist + " — " + Showed_track.Title);
                        }

                        Tag Showed_track_t = new Tag(playlist_[0]);
                        ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/temp.jpg")));
                        imgBrush.Stretch = Stretch.UniformToFill;


                        pictureBox.Fill = (Showed_track_t.picture != null) ? ConvertDrawingToImageBrush(new Bitmap(Showed_track_t.picture)) : imgBrush;
                        ArtistTextBlock.Text = Showed_track_t.Artist;
                        TitleTextBlock.Text = Showed_track_t.Title;
                        PlaylistBox.SelectedIndex = 0;
                    }
                    catch
                    {
                        Imunitet();
                    }
                    
                }
                else
                {
                    try
                    {
                        PLAYLIST new_playlist_ = new PLAYLIST(FileDialog.opf.FileName);


                        for (int i = 0; i < new_playlist_.Count(); i++)
                        {
                            Tag Showed_track = new Tag(new_playlist_[i]);
                            PlaylistBox.Items.Add(Showed_track.Artist + " — " + Showed_track.Title);
                        }

                        Tag Showed_track_t = new Tag(new_playlist_[0]);
                        ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/temp.jpg")));
                        imgBrush.Stretch = Stretch.UniformToFill;


                        pictureBox.Fill = (Showed_track_t.picture != null) ? ConvertDrawingToImageBrush(new Bitmap(Showed_track_t.picture)) : imgBrush;


                        playlist_ = playlist_ + new_playlist_;
                    }
                    catch
                    {
                        Imunitet();
                    }
                }
            }
            else
            {
                if (PLAYLIST.IsFirst)
                {
                    try
                    {
                        playlist_ = new PLAYLIST(FileDialog.opf.FileNames);
                        PLAYLIST.IsFirst = false;

                        for (int i = 0; i < playlist_.Count(); i++)
                        {

                            Tag Showed_track = new Tag(playlist_[i]);
                            PlaylistBox.Items.Add(Showed_track.Artist + " — " + Showed_track.Title);

                        }

                        Tag Showed_track_t = new Tag(playlist_[0]);
                        ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/temp.jpg")));
                        imgBrush.Stretch = Stretch.UniformToFill;


                        pictureBox.Fill = (Showed_track_t.picture != null) ? ConvertDrawingToImageBrush(new Bitmap(Showed_track_t.picture)) : imgBrush;
                        ArtistTextBlock.Text = Showed_track_t.Artist;
                        TitleTextBlock.Text = Showed_track_t.Title;
                        PlaylistBox.SelectedIndex = 0;
                    }
                    catch
                    {
                        Imunitet();
                    }


                }
                else
                {
                    try
                    {
                        PLAYLIST new_playlist_ = new PLAYLIST(FileDialog.opf.FileNames);

                        for (int i = 0; i < new_playlist_.Count(); i++)
                        {

                            Tag Showed_track = new Tag(new_playlist_[i]);
                            PlaylistBox.Items.Add(Showed_track.Artist + " — " + Showed_track.Title);

                        }

                        Tag Showed_track_t = new Tag(new_playlist_[0]);
                        ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/temp.jpg")));
                        imgBrush.Stretch = Stretch.UniformToFill;


                        pictureBox.Fill = (Showed_track_t.picture != null) ? ConvertDrawingToImageBrush(new Bitmap(Showed_track_t.picture)) : imgBrush;

                        playlist_ = playlist_ + new_playlist_;
                    }
                    catch
                    {
                        Imunitet();
                    }
                }
            }





        }

        private void PlaylistBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((PlaylistBox.Items.Count != 0) && (PlaylistBox.SelectedIndex != -1))
                {
                    string cur = playlist_[PlaylistBox.SelectedIndex];

                    playlist_.CurTrackNumber = PlaylistBox.SelectedIndex;

                    MainBass.Play(cur, MainBass.Volume);
                    RealTimeLabel.Text = TimeSpan.FromSeconds(MainBass.GetTimeOfStream(MainBass.Stream)).ToString();


                    ProgresBar.Maximum = MainBass.GetTimeOfStream(MainBass.Stream);



                    timer.Start();
                    PausePlaySong.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/pauseM.png")));
                    MainBass.img_flag = false;

                    Tag Showed_track_t = new Tag(playlist_[playlist_.CurTrackNumber]);
                    ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/temp.jpg")));
                    imgBrush.Stretch = Stretch.UniformToFill;


                    pictureBox.Fill = (Showed_track_t.picture != null) ? ConvertDrawingToImageBrush(new Bitmap(Showed_track_t.picture)) : imgBrush;
                    ArtistTextBlock.Text = Showed_track_t.Artist;
                    TitleTextBlock.Text = Showed_track_t.Title;
                }
            }
            catch
            {
                Imunitet();
            }
        }

        private void ProgresBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((PlaylistBox.Items.Count != 0) && (PlaylistBox.SelectedIndex != -1))
            {

                double absWidth = ProgresBar.ActualWidth;
                double absMouse = e.GetPosition(ProgresBar).X;
                double progresss = (absMouse * ProgresBar.Maximum) / absWidth;
                


                ProgresBar.Value = progresss;

                MainBass.Scroll(MainBass.Stream, (int)ProgresBar.Value);
                LocalTimeLabel.Text = TimeSpan.FromSeconds(MainBass.GetPosOfStream(MainBass.Stream)).ToString();
            }
        }

        private void ProgresBar_DragOver(object sender, DragEventArgs e)
        {

        }

        private void DeletePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            playlist_.Clear();
            
            PlaylistBox.Items.Clear();
            
            MainBass.Stop();
            timer.Stop();


            ArtistTextBlock.Text = "Artist";
            TitleTextBlock.Text = "Title";
            ProgresBar.Value = 0;
            LocalTimeLabel.Text = "00:00:00";
            RealTimeLabel.Text = "00:00:00";

            ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/temp.jpg")));
            imgBrush.Stretch = Stretch.UniformToFill;


            pictureBox.Fill = imgBrush;
            PausePlaySong.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/playM.png")));
            MainBass.img_flag = true;

            SongsTextBlock.Text = "Songs: " + playlist_.Count();
        }

        private void ClearPlaylistButton_Click(object sender, RoutedEventArgs e)
        {

            int temp = PlaylistBox.SelectedIndex;
            playlist_.Delete(PlaylistBox.SelectedIndex);
            try
            {
                PlaylistBox.Items.RemoveAt(PlaylistBox.SelectedIndex);
            }
            catch { }

            if (temp == 0 && !PlaylistBox.Items.IsEmpty) PlaylistBox.SelectedIndex = 0;

            else if (temp == playlist_.Count() - 1) PlaylistBox.SelectedIndex = playlist_.Count() - 1;
            else PlaylistBox.SelectedIndex = temp - 1;

            ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/temp.jpg")));
            imgBrush.Stretch = Stretch.UniformToFill;


            pictureBox.Fill = imgBrush;

            SongsTextBlock.Text = "Songs: " + playlist_.Count();


        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainBass.SetVolumeToStream(MainBass.Stream, (int)VolumeSlider.Value);
        }

        

        private void ButtonShafle_Click(object sender, RoutedEventArgs e)
        {
            int temp = PlaylistBox.SelectedIndex;
            playlist_.Shaffle();

            PlaylistBox.Items.Clear();

            for (int i =0; i<playlist_.Count(); i++)
            {
                Tag Showed_track = new Tag(playlist_[i]);
                PlaylistBox.Items.Add(Showed_track.Artist + " — " + Showed_track.Title);
            }
            PlaylistBox.SelectedIndex = temp;


        }
    }
}
