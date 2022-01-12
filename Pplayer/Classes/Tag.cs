using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Tags;

namespace Pplayer
{
    public class Tag
    {
        public int Bitrate;
        public int HG;
        public string Channels;
        public string Artist;
        public string Album;
        public string Title;
        public string Year;
        public string Type;
        public double Duration;
        public Image picture;


        Dictionary<int, string> D_Channels = new Dictionary<int, string>()
        {
            {0, "Null" },
            {1, "Mono" },
            {2, "Stereo"}
        };

        public Tag(string file_path)
        {
            TAG_INFO tag_INFO = new TAG_INFO();
            tag_INFO = BassTags.BASS_TAG_GetFromFile(file_path);
            Bitrate = tag_INFO.bitrate;
            HG = tag_INFO.channelinfo.freq;
            Channels = D_Channels[tag_INFO.channelinfo.chans];
            Artist = tag_INFO.artist;
            Album = tag_INFO.album;
            Year = tag_INFO.year;
            Title = (tag_INFO.title == "") ? FileDialog.GetFileName(file_path) : tag_INFO.title;
            Duration = tag_INFO.duration;
            picture = tag_INFO.PictureGetImage(0);

            switch (tag_INFO.channelinfo.ctype)
            {
                case BASSChannelType.BASS_CTYPE_MUSIC_IT:
                    Type = "IT";
                    break;
                case BASSChannelType.BASS_CTYPE_MUSIC_MO3:
                    Type = "MO3";
                    break;
                case BASSChannelType.BASS_CTYPE_MUSIC_MOD:
                    Type = "MOD";
                    break;
                case BASSChannelType.BASS_CTYPE_MUSIC_MTM:
                    Type = "MTM";
                    break;
                case BASSChannelType.BASS_CTYPE_MUSIC_S3M:
                    Type = "S3M";
                    break;
                case BASSChannelType.BASS_CTYPE_MUSIC_XM:
                    Type = "XM";
                    break;
                case BASSChannelType.BASS_CTYPE_RECORD:
                    Type = "HRECORD";
                    break;
                case BASSChannelType.BASS_CTYPE_SAMPLE:
                    Type = "HCHANNEL";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM:
                    Type = "Stream";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_AAC:
                    Type = "AAC";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_AC3:
                    Type = "AC3";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_ADX:
                    Type = "ADX";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_AIFF:
                    Type = "AIFF";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_ALAC:
                    Type = "ALAC";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_APE:
                    Type = "APE";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_CA:
                    Type = "CoreAudio";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_CD:
                    Type = "Audio CD, CDA";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_FLAC:
                    Type = "FLAC";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG:
                    Type = "FLAC OGG";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_MIDI:
                    Type = "MIDI";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_MIXER:
                    Type = "BassMix";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_MP1:
                    Type = "MP1";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_MP2:
                    Type = "MP2";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_MP3:
                    Type = "MP3";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_MP4:
                    Type = "MP4";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_MPC:
                    Type = "MPC";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_OFR:
                    Type = "OFR";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_OGG:
                    Type = "OGG";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_SPLIT:
                    Type = "Bass Mix Splitter";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_SPX:
                    Type = "Speex";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_TTA:
                    Type = "TTA";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_VIDEO:
                    Type = "VIDEO";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_WAV:
                    Type = "WAV LOWORD";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_WAV_FLOAT:
                    Type = "WAV FLOAT 32-bit";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_WAV_PCM:
                    Type = "WAV PCM 16-bit";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_WINAMP:
                    Type = "Winamp";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_WMA:
                    Type = "WMA";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_WMA_MP3:
                    Type = "MP3 over WMA";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_WV:
                    Type = "WavPack Lossless";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_WV_H:
                    Type = "WavPack Hybrid";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_WV_L:
                    Type = "WavPack Lossy";
                    break;
                case BASSChannelType.BASS_CTYPE_STREAM_WV_LH:
                    Type = "WavPack Hybrid Lossy";
                    break;
                case BASSChannelType.BASS_CTYPE_UNKNOWN:
                    Type = "UNKNOWN";
                    break;
            }

        }
    }
}
