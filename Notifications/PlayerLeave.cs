using HarmonyLib;
using TvvPancke.Notifications;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine;
using static TvvPancke.Menu.Main;

namespace TvvPancke.Patches
{
    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerLeftRoom")]
    internal class LeavePatch : MonoBehaviour  /// From IIDK
    {
        private static void Prefix(Player otherPlayer)
        {
            if (otherPlayer != PhotonNetwork.LocalPlayer && otherPlayer != a)
            {
                NotifiLib.SendNotification("[<color=grey>TE</color><color=cyan>MP</color>]: " + otherPlayer.NickName + " left!");
                a = otherPlayer;                    // color 1            //color 2
            }
        }

        private static Player a;
    }
}