using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pplayer
{
    public class PLAYLIST
    {
        private List<string> Songs = new List<string>();
       

        static public bool IsFirst = true;
        public int CurTrackNumber { get; set; }
        public string this[int i]
        {
            get { return Songs[i]; }
            
        }

        public int Count()
        {
            return Songs.Count;
        }

        public PLAYLIST(string path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path, Encoding.Default))
                {

                    string line;
                    int lineCount = 0;


                    while ((line = reader.ReadLine()) != null)
                    {
                        if (lineCount == 0 && line != "#EXTM3U")
                            new M3U_Exception("M3U header is missing.");

                        if (line.StartsWith("#EXTINF:"))
                        {
                            string[] temp_m3u_line = line.Substring(8, line.Length - 8).Split(new[] { ',' }, 2);

                            if (temp_m3u_line.Length != 2)
                                new M3U_Exception("Invalid track information.");

                            int seconds;
                            if (!int.TryParse(temp_m3u_line[0], out seconds))
                                new M3U_Exception("Invalid track duration.");



                        }

                        else if (!line.StartsWith("#"))
                        {

                            Uri tpath;
                            if (!Uri.TryCreate(line, UriKind.RelativeOrAbsolute, out tpath))
                                new M3U_Exception("Invalid music path.");

                            Songs.Add(line);

                        }

                        lineCount++;
                    }
                }
            }
            catch
            {

            }
        }
        public PLAYLIST(string[] songs)
        {
            try
            {
                for (int i = 0; i < songs.Length; i++)
                {
                    Songs.Add(songs[i]);
                }
            }
            catch
            {

            }
        }

        public PLAYLIST(PLAYLIST obj1)
        {
            try
            {
                for (int i = 0; i < obj1.Count(); i++)
                {
                    Songs.Add(obj1[i]);
                }
            }
            catch
            {

            }
        }

        public static PLAYLIST operator +(PLAYLIST obj1, PLAYLIST obj2)
        {
            PLAYLIST c_Playlist = new PLAYLIST(obj1);

            for (int i = 0; i < obj2.Count(); i++)
            {
                c_Playlist.Songs.Add(obj2[i]);
            }

            return c_Playlist;
        }

        public void Save(string Path)
        {
            {

                using (StreamWriter writer = new StreamWriter(Path))
                {
                    writer.WriteLine("#EXTM3U");
                    foreach (string song in Songs)
                    {
                        Tag track = new Tag(song);
                        writer.WriteLine($"#EXTINF:{(int)track.Duration},{track.Artist} - {track.Title}");
                        writer.WriteLine(song);

                    }
                }
            }
        }

        public void Clear()
        {
            Songs.Clear();
            IsFirst = true;
        }

        public void Delete_pfile(string path)
        {
            File.Delete(path);
        }

        public void Delete(int i)
        {
            try
            {
                Songs.RemoveAt(i);
            }
            catch { }
        }

        public void Shaffle()
        {
            List<string> ShaffleSongs = Songs.OrderBy(a => Guid.NewGuid()).ToList();
            Songs = ShaffleSongs;
        }
    }

}
