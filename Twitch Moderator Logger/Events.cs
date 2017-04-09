using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitchLib.Events.PubSub;

namespace Twitch_Moderator_Logger
{
    public static class Events
    {
        public static void onConnect(object sender, object e)
        {
            int channelId = 0, moderatorId = 0;
            try
            {
                channelId = Convert.ToInt32(TwitchLib.TwitchApi.Channels.GetChannel(Common.Config.Channel).Id);
            } catch(Exception)
            {
                Common.PubSub.Disconnect();
                MessageBox.Show("The supplied channel was invalid!");
                Helpers.LaunchLogin();
                return;
            }
            try
            {
                moderatorId = Convert.ToInt32(TwitchLib.TwitchApi.Channels.GetChannel(Common.Config.ModeratorUsername).Id);
            }
            catch (Exception)
            {
                Common.PubSub.Disconnect();
                MessageBox.Show("The supplied moderator username was invalid!");
                Helpers.LaunchLogin();
                return;
            }
            Common.PubSub.ListenToChatModeratorActions(moderatorId, channelId, Common.Config.ModeratorOAuth);
        }

        public static void onListenResponse(object sender, OnListenResponseArgs e)
        {
            if(e.Successful)
            {
                UI.Instance.toggleConnected(true);
            } else
            {
                Common.PubSub.Disconnect();
                MessageBox.Show("Either the provided OAuth was invalid or the provided moderator username was not actually a moderator on the channel provided.");
                Helpers.LaunchLogin();
            }
        }

        public static void onBan(object sender, OnBanArgs e)
        {
            Models.Listing newListing = new Models.Listing()
            {
                DateTime = DateTime.UtcNow,
                ActionPerformed = "Ban",
                ModeratorName = e.BannedBy,
                ViewerName = e.BannedUser,
                ActionMessage = e.BanReason
            };
            Common.Data.Add(newListing);
            UI.Instance.addListing(newListing);
        }

        public static void onTimeout(object sender, OnTimeoutArgs e)
        {
            Models.Listing newListing = new Models.Listing()
            {
                DateTime = DateTime.UtcNow,
                ActionPerformed = $"Timeout ({e.TimeoutDuration.ToReadableString()})",
                ModeratorName = e.TimedoutBy,
                ViewerName = e.TimedoutUser,
                ActionMessage = e.TimeoutReason
            };
            Common.Data.Add(newListing);
            UI.Instance.addListing(newListing);
        }

        public static void onUnban(object sender, OnUnbanArgs e)
        {
            Models.Listing newListing = new Models.Listing()
            {
                DateTime = DateTime.UtcNow,
                ActionPerformed = "Unban",
                ModeratorName = e.UnbannedBy,
                ViewerName = e.UnbannedUser,
                ActionMessage = ""
            };
            Common.Data.Add(newListing);
            UI.Instance.addListing(newListing);
        }
    }
}
