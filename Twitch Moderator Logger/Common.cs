using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Moderator_Logger
{
    public static class Common
    {
        public static string TwitchClientId = "wlohmxcq3me66su5i9go8hywbrcz5r";

        public static Models.Config Config = new Models.Config();
        public static TwitchLib.TwitchPubSub PubSub = new TwitchLib.TwitchPubSub();

        public static List<Models.Listing> Data = new List<Models.Listing>();
    }
}
