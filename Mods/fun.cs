
using System.Linq;


namespace TvvPancke.Mods
{
    public class Fun
    {
        public static void MuteAll()
        {
            foreach (var line in GorillaScoreboardTotalUpdater.allScoreboardLines.Where(line => !line.muteButton.isAutoOn))
            {
                line.muteButton.isOn = true;
                line.PressButton(true, GorillaPlayerLineButton.ButtonType.Mute);
            }
        }

        public static void UnmuteAll()
        {
            foreach (var line in GorillaScoreboardTotalUpdater.allScoreboardLines.Where(line => line.muteButton.isAutoOn))
            {
                line.muteButton.isOn = false;
                line.PressButton(false, GorillaPlayerLineButton.ButtonType.Mute);
            }
        }


    }
}