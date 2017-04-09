using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Twitch_Moderator_Logger
{
    public static class Settings
    {
        public static bool LoadSettings()
        {
            if(File.Exists("settings.json"))
            {
                try
                {
                    Common.Config = JsonConvert.DeserializeObject<Models.Config>(File.ReadAllText("settings.json"));
                    return true;
                } catch(Exception)
                {
                    MessageBox.Show("Failed to load settings from settings.json file! Creating new config file.");
                    File.Delete("settings.json");
                    populateDefaultSettings();
                    return false;
                }
            } else
            {
                populateDefaultSettings();
                return false;
            }
        }

        public static void SaveSettings()
        {
            File.WriteAllText("settings.json", JsonConvert.SerializeObject(Common.Config));
        }

        private static void populateDefaultSettings()
        {
            Common.Config.Channel = "";
            Common.Config.ModeratorUsername = "";
            Common.Config.ModeratorOAuth = "";

            Common.Config.AppWidth = 880;
            Common.Config.AppHeight = 865;

            int lvWidth = UI.Instance.getListViewWidth();

            Common.Config.Columns = new Models.Column[] {
                new Models.Column
                {
                    Name = "Date",
                    Visible = true,
                    DisplayOrder = 0,
                    Width = (int)lvWidth / 6
                },
                new Models.Column
                {
                    Name = "Time",
                    Visible = true,
                    DisplayOrder = 1,
                    Width = (int)lvWidth / 6
                },
                new Models.Column
                {
                    Name = "Moderator Name",
                    Visible = true,
                    DisplayOrder = 2,
                    Width = (int)lvWidth / 6
                },
                new Models.Column
                {
                    Name = "Action Performed",
                    Visible = true,
                    DisplayOrder = 3,
                    Width = (int)lvWidth / 6
                },
                new Models.Column
                {
                    Name = "Viewer Name",
                    Visible = true,
                    DisplayOrder = 4,
                    Width = (int)lvWidth / 6
                },
                new Models.Column
                {
                    Name = "Action Message",
                    Visible = true,
                    DisplayOrder = 5,
                    Width = (int)lvWidth / 6
                },
            };
        }
    }
}
