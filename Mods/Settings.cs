using BepInEx;
using System;
using TvvPancke.Classes;
using TvvPancke.Menu;
using UnityEngine;
using static TvvPancke.Menu.Main;
using static TvvPancke.Settings;

namespace TvvPancke.Mods
{
    internal class SettingsMods
    {

        public static void EnterSettings()
        {
            buttonsType = 1;
            pageNumber = 0;
        }

        public static void SafeMods()
        {
            buttonsType = 2;
            pageNumber = 0;
        }

        public static void Visuals()
        {
            buttonsType = 3;
            pageNumber = 0;
        }

        public static void ModdedRoomOnly()
        {
            buttonsType = 4;
            pageNumber = 0;

        }

        // HAND SETTINGS
        public static void RightHand()
        {
            rightHanded = true;
        }

        public static void LeftHand()
        {
            rightHanded = false;
        }

        // FPS COUNTER
        public static void EnableFPSCounter()
        {
            fpsCounter = true;
        }

        public static void DisableFPSCounter()
        {
            fpsCounter = false;
        }

        // NOTIFICATIONS
        public static void EnableNotifications()
        {
            disableNotifications = false;
        }

        public static void DisableNotifications()
        {
            disableNotifications = true;
        }

        // ANIMATION
        public static void EnableAni4()
        {
            Ani4 = true;
        }

        public static void DisableAni4()
        {
            Ani4 = false;
        }

        // ROUND CONTROL
        public static void YesRound()
        {
            Main.noround = true;
        }

        public static void NoRound()
        {
            Main.noround = false;
        }

        // OUTLINE
        public static void EnableOutline()
        {
            Main.Outlineint = true;
        }

        public static void DisableOutline()
        {
            Main.Outlineint = false;
        }

        // DISCONNECT BUTTON
        public static void EnableDisconnectButton()
        {
            disconnectButton = true;
        }

        public static void DisableDisconnectButton()
        {
            disconnectButton = false;
        }
    }
}
