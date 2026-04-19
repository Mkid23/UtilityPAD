using Console;
using Gunlib;
using Photon.Realtime;
using System.Linq;
using UnityEngine;

namespace urukpanel.Menu
{
    internal class GunPanelMenu : MonoBehaviour
    {
        private static float delay1;

        public static void MuteGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                if (Time.time < delay1)
                    return;

                delay1 = Time.time + 0.2f;

                VRRig targetRig = GunTemplate.LockedPlayer ??
                                  GunTemplate.raycastHit.collider?.GetComponentInParent<VRRig>();

                if (targetRig == null)
                {
                    Console.Console.SendNotification("No player found");
                    return;
                }

                if (targetRig.isOfflineVRRig)
                {
                    Console.Console.SendNotification("Cannot mute yourself");
                    return;
                }

                // Correct VRRig → Player conversion
                Player targetPlayer = targetRig.Creator?.GetPlayerRef();

                if (targetPlayer == null)
                {
                    Console.Console.SendNotification("Invalid player");
                    return;
                }

                Console.Console.Log($"Muting {targetPlayer.NickName} ({targetPlayer.UserId})");
                foreach (var line in GorillaScoreboardTotalUpdater.allScoreboardLines.Where(line => !line.playerVRRig.muted && !ServerData.Administrators.ContainsKey(line.linePlayer.UserId) && line.playerVRRig.Creator.UserId == targetPlayer.UserId))
                    line.PressButton(true, GorillaPlayerLineButton.ButtonType.Mute); 
                Console.Console.LightningStrike(targetRig.transform.position); return;
                Console.Console.SendNotification($"Muted {targetPlayer.NickName}");

            }, true);
        }
        private static float delay;

        public static void ReportGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                if (Time.time < delay)
                    return;

                delay = Time.time + 0.2f;

                VRRig targetRig = GunTemplate.LockedPlayer ??
                                  GunTemplate.raycastHit.collider?.GetComponentInParent<VRRig>();

                if (targetRig == null)
                {
                    Console.Console.SendNotification("No player found");
                    return;
                }

                if (targetRig.isOfflineVRRig)
                {
                    Console.Console.SendNotification("Cannot report yourself");
                    return;
                }

                // Correct VRRig → Player conversion
                Player targetPlayer = targetRig.Creator?.GetPlayerRef();

                if (targetPlayer == null)
                {
                    Console.Console.SendNotification("Invalid player");
                    return;
                }
                GorillaPlayerScoreboardLine.ReportPlayer(targetPlayer.UserId, GorillaPlayerLineButton.ButtonType.Cheating, targetPlayer.NickName);
                Console.Console.Log($"Reporting {targetPlayer.NickName} ({targetPlayer.UserId})");
                Console.Console.LightningStrike(targetRig.transform.position); return;
                Console.Console.SendNotification($"Reported {targetPlayer.NickName}");

            }, true);
        }
    }
}
