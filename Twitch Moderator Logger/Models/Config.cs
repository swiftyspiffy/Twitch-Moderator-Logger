using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Moderator_Logger.Models
{
    public class Config
    {
        public string Channel;
        public string ModeratorUsername;
        public string ModeratorOAuth;

        public int AppWidth;
        public int AppHeight;

        public Column[] Columns;
    }
}
