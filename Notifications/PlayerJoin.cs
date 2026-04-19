using HarmonyLib;
using TvvPancke.Notifications;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine;
using static TvvPancke.Menu.Main;

namespace TvvPancke.Patches
{
    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerEnteredRoom")]
    internal class JoinPatch : MonoBehaviour  /// From IIDK
    {
        private static void Prefix(Player newPlayer)
        {
            if (newPlayer != oldnewplayer)
            {
                NotifiLib.SendNotification("[<color=grey>Xe</color><color=cyan>non</color>]: " + newPlayer.NickName + " joined!");
                oldnewplayer = newPlayer;             // color 1            //color 2
            }
        }

        private static Player oldnewplayer;
    }
}