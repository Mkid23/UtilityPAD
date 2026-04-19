using GorillaNetworking;
using PanckeUtilityPad.LegalMods;
using Photon.Pun;
using TvvPancke.Classes;
using TvvPancke.Mods;
using urukpanel.Menu;

namespace TvvPancke.Menu
{
    internal class Buttons
    {
        public static ButtonInfo[][] buttons = new ButtonInfo[][]
        {
            new ButtonInfo[] { // Main Mods 0
                new ButtonInfo { buttonText = "⭐ReturnHome⭐", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the menu."},
                new ButtonInfo { buttonText = "⭐Settings⭐", method =() => SettingsMods.SafeMods(), isTogglable = false, toolTip = "Opens the settings page for the menu."},
                new ButtonInfo { buttonText = "⭐Help Mods⭐", method =() => SettingsMods.EnterSettings(), isTogglable = false, toolTip = "Opens the safemods page for the menu."},
                new ButtonInfo { buttonText = "⭐Visuals⭐", method =() => SettingsMods.Visuals(), isTogglable = false, toolTip = "Opens the Visuals page for the menu."},
                new ButtonInfo { buttonText = "⭐ReturnHome⭐", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the menu."},
                new ButtonInfo { buttonText = "⭐Modded Room Only⭐", method =() => SettingsMods.ModdedRoomOnly(), isTogglable = false, toolTip = "Opens the modded room page for the menu."},
                new ButtonInfo { buttonText = "⭐Discord⭐", method = () => Global.OpenDiscord(), isTogglable = false, toolTip = "Opens the Discord server." }
            },
            new ButtonInfo[] { // SafeMods 
                new ButtonInfo { buttonText = "⭐ReturnHome⭐", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the menu."},
                new ButtonInfo { buttonText = "⭐LEAVE⭐", method = Leave,isTogglable = false,toolTip = "Leaves The Room."},
                new ButtonInfo { buttonText = "⭐Quit⭐", method = Quit,isTogglable = false,toolTip = "Quits The Game."},
                new ButtonInfo { buttonText = "RIP Granny", method = () => granny(), toolTip = "Play sound 2.", isTogglable = false },
                new ButtonInfo { buttonText = "Sweater Weather", method = () => Sweater(), toolTip = "Play sound 7.", isTogglable = false },
                new ButtonInfo { buttonText = "Mario Jump", method = () => jump(), toolTip = "Play sound 8.", isTogglable = false },
                new ButtonInfo { buttonText = "⭐Join random⭐", method = JoinRandom,isTogglable = false,toolTip = "Join Random Room."},
                new ButtonInfo { buttonText = "⭐Mute All⭐", method = Fun.MuteAll,isTogglable = false,toolTip = "Mute All."},
                new ButtonInfo { buttonText = "⭐SrikeEveryone⭐", method = () => Console.Console.StrikeAll(), toolTip = "kickall." },
                new ButtonInfo { buttonText = "⭐Mute Gun⭐", method = () => GunPanelMenu.MuteGun(), toolTip = "Mute players by aiming at them." },
                new ButtonInfo { buttonText = "⭐Report Gun⭐", method = () => GunPanelMenu.ReportGun(), toolTip = "Report players by aiming at them." },
                new ButtonInfo { buttonText = "⭐RPC Protection⭐", method = () => ModFunction.RPCProtection(), toolTip = "Protects against RPC exploits." },

            },
            new ButtonInfo[] { // Settings 1
                new ButtonInfo { buttonText = "⭐ReturnHome⭐", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the menu."},
                new ButtonInfo { buttonText = "⭐Right Hand Menu⭐", enableMethod =() => SettingsMods.RightHand(), disableMethod =() => SettingsMods.LeftHand(), toolTip = "Puts the menu on your right hand."},
                new ButtonInfo { buttonText = "⭐Notifications⭐", enableMethod =() => SettingsMods.EnableNotifications(), disableMethod =() => SettingsMods.DisableNotifications(),toolTip = "Toggles the notifications."},
                new ButtonInfo { buttonText = "⭐Outline Menu⭐", enableMethod =() => SettingsMods.EnableOutline(), disableMethod =() => SettingsMods.DisableOutline(), enabled = true, toolTip = "Toggles the outline."},
                new ButtonInfo { buttonText = "⭐Menu Rounding⭐", enableMethod =() => SettingsMods.NoRound(), disableMethod =() => SettingsMods.YesRound(), enabled = true, toolTip = "Makes Menu less smooth."},
                new ButtonInfo { buttonText = "Change Fly Speed", method = () => ModFunction.ChangeFlySpeed(), toolTip = "Changes your fly speed." },new ButtonInfo { buttonText = "⭐FPS Counter⭐", enableMethod =() => SettingsMods.EnableFPSCounter(), disableMethod =() => SettingsMods.DisableFPSCounter(),toolTip = "Toggles the FPS counter."},
            },
            new ButtonInfo[] { // Visuals 1
                new ButtonInfo { buttonText = "⭐ReturnHome⭐", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the menu."},
                new ButtonInfo { buttonText = "⭐Morning Time⭐", method =() => BetterDayNightManager.instance.SetTimeOfDay(1), toolTip = "Sets your time of day to morning."},
                new ButtonInfo { buttonText = "⭐Day Time⭐", method =() => BetterDayNightManager.instance.SetTimeOfDay(3), toolTip = "Sets your time of day to daytime."},
                new ButtonInfo { buttonText = "⭐Evening Time⭐", method =() => BetterDayNightManager.instance.SetTimeOfDay(7), toolTip = "Sets your time of day to evening."},
                new ButtonInfo { buttonText = "⭐Night Time⭐", method =() => BetterDayNightManager.instance.SetTimeOfDay(0), toolTip = "Sets your time of day to night."},

            },
            new ButtonInfo[] { // Modded only
                new ButtonInfo { buttonText = "⭐ReturnHome⭐", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Opens the menu."},



            },
            new ButtonInfo[] { // Report
            },


            new ButtonInfo[] { // dont do anything to this 14
                new ButtonInfo { buttonText = "⭐Disconnect⭐", method =() => Global.idkthethefuckimdoingthis(), isTogglable = false, toolTip = "Disconnects you from the lobby."},
                new ButtonInfo { buttonText = "⭐Home3⭐", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "HOME."},
            },
        };

        public static void Leave()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.Disconnect();
            }
        }
        public static void Quit()
        {
            UnityEngine.Application.Quit();
        }
        public static string sound1 = "https://www.myinstants.com/media/sounds/daddyy-chill.mp3";
        public static void daddyy()
        {
            GorillaTagger.Instance.StartCoroutine(Main.PlaySFX(sound1));
        }

        public static string sound2 = "https://www.myinstants.com/media/sounds/rip-my-granny-she-got-hit-by-a-bazooka.mp3";
        public static void granny()
        {
            GorillaTagger.Instance.StartCoroutine(Main.PlaySFX(sound2));
        }

        public static string sound3 = "https://www.myinstants.com/media/sounds/gopgopgop.mp3";
        public static void gopgopgop()
        {
            GorillaTagger.Instance.StartCoroutine(Main.PlaySFX(sound3));
        }

        public static string sound4 = "https://www.myinstants.com/media/sounds/ive-got-this-faaaaaaaaahhhhh.mp3";
        public static void faaaaaaaaahhhhh()
        {
            GorillaTagger.Instance.StartCoroutine(Main.PlaySFX(sound4));
        }

        public static string sound5 = "https://www.myinstants.com/media/sounds/chug-jug-2_vsHMoi6.mp3";
        public static void chug_jug()
        {
            GorillaTagger.Instance.StartCoroutine(Main.PlaySFX(sound5));
        }

        public static string sound6 = "https://www.myinstants.com/media/sounds/anime-ahh.mp3";
        public static void moan()
        {
            GorillaTagger.Instance.StartCoroutine(Main.PlaySFX(sound6));
        }

        public static string sound7 = "https://github.com/Mkid23/PanckeUtiliyPadGIT/raw/refs/heads/main/Sweater%20Weather-%20The%20Neighbourhood.mp3";
        public static void Sweater()
        {
            GorillaTagger.Instance.StartCoroutine(Main.PlaySFX(sound7));
        }

        public static string sound8 = "https://www.myinstants.com/media/sounds/maro-jump-sound-effect_1.mp3";
        public static void jump()
        {
            GorillaTagger.Instance.StartCoroutine(Main.PlaySFX(sound8));
        }

        public static void JoinRandom()
        {
            if (!PhotonNetwork.InRoom)
            {
                PhotonNetworkController.Instance.AttemptToJoinPublicRoom(GorillaComputer.instance.GetJoinTriggerForZone(PhotonNetworkController.Instance.currentJoinTrigger.networkZone), JoinType.Solo);
            }
        }


    }
}