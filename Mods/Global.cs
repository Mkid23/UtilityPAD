using BepInEx;
using Colorlib;
using ExitGames.Client.Photon;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaNetworking;
using GorillaTag.Reactions;
using GorillaTagScripts;
using Gunlib;
using HarmonyLib;
using Mono.Cecil.Cil;
using Oculus.Interaction;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using POpusCodec.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using TvvPancke.Classes;
using TvvPancke.Menu;
using TvvPancke.Notifications;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.Timeline;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using Valve.VR;
using Valve.VR.InteractionSystem;
using static TvvPancke.Menu.Main;
using Action = System.Action;
using Hashtable = System.Collections.Hashtable;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


namespace TvvPancke.Mods
{
    internal class Global
    {
        public static void ReturnHome() // Return to main menu
        {
            buttonsType = 0;
            pageNumber = 0;
            shouldHome = false;
        }

        public static void placeholder() // Just a placeholder function
        {

        }
        public static void idkthethefuckimdoingthis()
        {
            PhotonNetwork.Disconnect();
        }

        public static void OpenDiscord()
        {
            OpenUrl("https://discord.gg/hhyjzZK7HQ");
        }

        private static void OpenUrl(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
            }
        }
    }
}


