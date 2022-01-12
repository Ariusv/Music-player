using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pplayer
{
    class M3U_Exception
    {
        public M3U_Exception(string message)
        {
            System.Windows.MessageBox.Show(message, "ERROR");
        }
    }
}
