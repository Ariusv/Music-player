using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Un4seen.Bass;

namespace Pplayer
{
    public static class MainBass
    {
        //для зміни кнопки пауза - грати
        public static bool img_flag = true;

        // кінець плейлиста

        public static bool EndOfPlaylist;

        public static bool isStoped = true;

        //стан ініціалізації
        public static bool InitDefaultDevice;
        //Потік
        public static int Stream;

        public static int Volume = 100;

        //частота дискретизації
        public static int HG = 44100;

        // хендли плагінів
        private static readonly List<int> PluginsHandles = new List<int>();

        //Иніціалізація Bass.dll
        public static bool InitBass(int hg)
        {
            if (!InitDefaultDevice)
            {
                InitDefaultDevice = Bass.BASS_Init(-1, hg, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
                if (InitDefaultDevice)
                {
                    PluginsHandles.Add(Bass.BASS_PluginLoad(FileDialog.AppPath + @"plugins\bass_aac.dll"));
                    PluginsHandles.Add(Bass.BASS_PluginLoad(FileDialog.AppPath + @"plugins\bass_ac3.dll"));
                    PluginsHandles.Add(Bass.BASS_PluginLoad(FileDialog.AppPath + @"plugins\bass_mpc.dll"));
                    PluginsHandles.Add(Bass.BASS_PluginLoad(FileDialog.AppPath + @"plugins\bassalac.dll"));
                    //PluginsHandles.Add(Bass.BASS_PluginLoad(FileDialog.AppPath + @"plugins\bassopus.dll")); // error
                    PluginsHandles.Add(Bass.BASS_PluginLoad(FileDialog.AppPath + @"plugins\bassflac.dll"));
                    PluginsHandles.Add(Bass.BASS_PluginLoad(FileDialog.AppPath + @"plugins\basswma.dll"));
                    PluginsHandles.Add(Bass.BASS_PluginLoad(FileDialog.AppPath + @"plugins\basswv.dll"));
                    PluginsHandles.Add(Bass.BASS_PluginLoad(FileDialog.AppPath + @"plugins\bass_tta.dll"));

                    int Errors = 0;
                    for (int i = 0; i < PluginsHandles.Count; i++)
                    {
                        if (PluginsHandles[i] == 0) Errors++;
                    }

                    if (Errors != 0)
                    {
                        MessageBox.Show(Errors + " plugins didn't download", "ERROR");
                        Errors = 0;
                    }
                }
            }

            return InitDefaultDevice;
        }

        public static void Play(string path, int vol)
        {
            if (Bass.BASS_ChannelIsActive(Stream) != BASSActive.BASS_ACTIVE_PAUSED)
            {
                Stop();

                if (InitBass(HG))
                {
                    Stream = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_DEFAULT);
                    if (Stream != 0)
                    {
                        Volume = vol;
                        Bass.BASS_ChannelSetAttribute(Stream, BASSAttribute.BASS_ATTRIB_VOL, vol / 100F);
                        Bass.BASS_ChannelPlay(Stream, false);
                    }
                }
            }
            else Bass.BASS_ChannelPlay(Stream, false);
            isStoped = false;
        }

        //Stop
        public static void Stop()
        {
            Bass.BASS_ChannelStop(Stream);
            Bass.BASS_StreamFree(Stream);
            isStoped = true;
        }

        //Pause
        public static void Pause()
        {
            if (Bass.BASS_ChannelIsActive(Stream) == BASSActive.BASS_ACTIVE_PLAYING)
                Bass.BASS_ChannelPause(Stream);
        }

        //час в секундах
        public static int GetTimeOfStream(int stream)
        {
            //long T_Bytes = Bass.BASS_ChannelGetLength(stream);
            //double Time = Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream));
            return Convert.ToInt32(Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream)));
        }


        public static int GetPosOfStream(int stream)
        {
            return Convert.ToInt32(Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream)));
        }

        //SetVolume
        public static void SetVolumeToStream(int stream, int vol)
        {
            Volume = vol;
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, vol / 100F);
        }

        public static void Scroll(int stream, int pos)
        {
            Bass.BASS_ChannelSetPosition(stream, (double)pos);
        }

        public static bool NextTrack()
        {
            if ((Bass.BASS_ChannelIsActive(Stream) == BASSActive.BASS_ACTIVE_STOPPED) && (!isStoped))
            {
                if (MainWindow.playlist_.Count() > MainWindow.playlist_.CurTrackNumber + 1)
                {
                    Play(MainWindow.playlist_[++MainWindow.playlist_.CurTrackNumber], Volume);
                    EndOfPlaylist = false;
                    return true;
                }
                else
                    EndOfPlaylist = true;
            }
            return false;

        }
    }
}

