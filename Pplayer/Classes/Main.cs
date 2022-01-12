using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pplayer
{
    public static class FileDialog
    {
        public static OpenFileDialog opf = new OpenFileDialog();


        public static string AppPath = AppDomain.CurrentDomain.BaseDirectory;

       



        public static string GetFileName(string file)
        {
            string[] temp = file.Split('\\');
            return temp[temp.Length - 1];
        }

        public static void InputFormats()
        {
            opf.Filter = "All music formats|*.mp3; *.m4a; *.mp4; *tta; *alac; *wv; *wma; *flac|" +
                "Playlist format(*.m3u)|*.m3u";
        }
    }
}
