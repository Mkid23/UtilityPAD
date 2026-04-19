using BepInEx;
using Colorlib;
using GorillaNetworking;
using GorillaTag;
using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Valve.VR.InteractionSystem;
using TvvPancke.Classes;
using TvvPancke.Mods;
using TvvPancke.Notifications;
using static Fusion.Sockets.NetBitBuffer;
using static TvvPancke.Menu.Buttons;
using static TvvPancke.Mods.SettingsMods;
using static TvvPancke.Settings;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;

namespace TvvPancke.Menu
{
    [HarmonyPatch(typeof(GorillaLocomotion.GTPlayer))]
    [HarmonyPatch("LateUpdate", MethodType.Normal)]
    public class Main : MonoBehaviour
    {
        #region ========== EASY SETTINGS - Change These! ==========
        // This section contains values beginners can safely change

        // How many buttons fit on one page
        public const int buttonsPerPage = 4;

        // Space between each button (bigger = more space)
        private const float BUTTON_SPACING = 0.09f;

        // How rounded the corners are (0 = square, 0.075 = very round)
        private const float CORNER_ROUNDNESS = 0.075f;

        // Button corner roundness (smaller than menu corners)
        private const float BUTTON_CORNER_ROUNDNESS = 0.03f;

        // Animation speeds
        private const float OPEN_ANIMATION_SPEED = 0.45f;
        private const float CLOSE_ANIMATION_SPEED = 0.35f;
        private const float WOBBLE_OPEN_SPEED = 0.8f;
        private const float WOBBLE_CLOSE_SPEED = 0.5f;
        private const float ELASTIC_OPEN_SPEED = 0.6f;
        private const float ELASTIC_CLOSE_SPEED = 0.4f;

        // Sound effect URLs
        public static string openSfxUrl = "https://www.myinstants.com/media/sounds/can.mp3";
        public static string closeSfxUrl = "https://www.myinstants.com/media/sounds/meta-quest-shutdown-sound.mp3";
        public static string buttonSfxUrl = "https://www.myinstants.com/media/sounds/discord-notification.mp3";

        #endregion

        #region ========== Variables (Don't change unless you know what you're doing) ==========

        // Menu Objects
        public static GameObject menu;
        public static GameObject menu2;
        public static GameObject menuBackground2;
        public static GameObject menuBackground;
        public static GameObject reference;
        public static GameObject canvasObject;
        public static Camera TPC;
        public static Text fpsObject;
        public static SphereCollider buttonCollider;

        // Menu State
        public static int pageNumber = 0;
        public static int buttonsType = 0;
        public static bool Close = false;
        public static bool boardshit = true;

        // Textures and Materials
        public static Texture2D homeIcon, settingsIcon, enabledIcon, disconnectIcon;
        public static Material homeMat, settingsMat, enabledMat, disconnectMat;
        public static Material bgMat = new Material(Shader.Find("GorillaTag/UberShader"));
        public static Material ForestMat = null;
        public static Material StumpMat = null;

        // Feature Toggles
        public static bool shouldStar = true;
        public static bool shouldHome = false;
        public static bool shouldNext = true;
        public static bool shouldPrev = true;
        public static bool shouldConfig = true;
        public static bool masterNotif = false;
        public static bool shouldPanic = true;
        public static bool lastMasterClient;
        public static bool shouldAnimate = false;
        public static bool noround = false;
        public static bool oulineing = false;
        public static bool Outlineint = true;
        public static bool Ani4;

        // Leaderboard Settings
        public static string StumpLeaderboardID = "UnityTempFile";
        public static string ForestLeaderboardID = "UnityTempFile";
        public static int StumpLeaderboardIndex = 5;
        public static int ForestLeaderboardIndex = 9;

        // Theme Settings
        public static int Theme = 1;
        public static Color32 BgColor1 = new Color32(6, 6, 6, 255);
        public static Color32 BgColor2 = new Color32(14, 14, 14, 255);

        // Utility Lists
        public static List<TextMeshPro> udTMP = new List<TextMeshPro>();

        #endregion

        #region ========== MAIN UPDATE LOOP ==========
        // This runs every frame and handles everything

        public static void Prefix()
        {
            // Set background color
            bgMat.color = new Color32(25, 25, 25, 255);

            // Handle enabled mods list (buttonsType 13)
            if (buttonsType == 13)
            {
                List<ButtonInfo> enabledMods = new List<ButtonInfo>();
                int categoryIndex = 0;
                foreach (ButtonInfo[] buttonlist in Buttons.buttons)
                {
                    foreach (ButtonInfo v in buttonlist)
                    {
                        if (v.enabled)
                            enabledMods.Add(v);
                    }
                    categoryIndex++;
                }
            }

            // Update stump boards once
            if (boardshit)
            {
                UpdateStumpBoards();
            }

            // Create Xenon folder if it doesn't exist
            if (!Directory.Exists("urukpanel"))
            {
                Directory.CreateDirectory("urukpanel");
            }

            // Handle menu opening/closing
            try
            {
                HandleMenuToggle();
            }
            catch (Exception exc)
            {
                UnityEngine.Debug.LogError($"{TvvPancke.Mods.PluginInfo.Name} // Error initializing at {exc.StackTrace}: {exc.Message}");
            }

            // Update FPS and run enabled mods
            try
            {
                UpdateFPSCounter();
                RunEnabledMods();
            }
            catch (Exception exc)
            {
                UnityEngine.Debug.LogError($"{TvvPancke.Mods.PluginInfo.Name} // Error with executing mods at {exc.StackTrace}: {exc.Message}");
            }
        }

        #endregion

        #region ========== MENU OPENING/CLOSING ==========
        // Handles when you press the button to open/close menu

        private static void HandleMenuToggle()
        {
            // Check if player wants to open menu
            bool toOpen = (!rightHanded && ControllerInputPoller.instance.leftControllerSecondaryButton) ||
                         (rightHanded && ControllerInputPoller.instance.rightControllerSecondaryButton);
            bool keyboardOpen = UnityInput.Current.GetKey(keyboardButton);

            if (menu == null)
            {
                // Menu is closed, open it if button pressed
                if (toOpen || keyboardOpen)
                {
                    CreateMenu();
                    RecenterMenu(rightHanded, keyboardOpen);
                    GorillaTagger.Instance.StartCoroutine(OpenAni());

                    if (Ani4)
                    {
                        GorillaTagger.Instance.StartCoroutine(WobbleOpenAni());
                    }

                    if (reference == null)
                    {
                        CreateReference(rightHanded);
                    }
                }
            }
            else
            {
                // Menu is open
                if (toOpen || keyboardOpen)
                {
                    // Button still held, just recenter menu
                    RecenterMenu(rightHanded, keyboardOpen);
                }
                else
                {
                    // Button released, close menu
                    GameObject.Find("Shoulder Camera").transform.Find("CM vcam1").gameObject.SetActive(true);
                    GorillaTagger.Instance.StartCoroutine(CloseAni());

                    if (Ani4)
                    {
                        GorillaTagger.Instance.StartCoroutine(WobbleCloseAni());
                    }
                }
            }
        }

        #endregion

        #region ========== STUMP CUSTOMIZATION ==========
        // Changes the boards in stump to say XENON TEMP

        private static void UpdateStumpBoards()
        {
            try
            {
                // Update text boards
                GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdHeadingText").GetComponent<TextMeshPro>().text = "XENON TEMP"; /// MOTD heading
                GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText").GetComponent<TextMeshPro>().text = "\nXENON TEMP\nThe best Free Template."; /// MOTD body
                GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/CodeOfConductHeadingText").GetComponent<TextMeshPro>().text = "XENON TEMP \n----------------------------- \n"; /// COC heading
                GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/COCBodyText_TitleData").GetComponent<TextMeshPro>().text = "Made by G3IF"; /// COC body

                // Update forest background
                GameObject fuh = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomBoundaryStones/BoundaryStoneSet_Forest/wallmonitorforestbg");
                if (fuh != null)
                {
                    Renderer ren = fuh.GetComponent<Renderer>();
                    if (ren != null)
                    {
                        ren.material = bgMat;
                    }
                }

                // Update computer screen
                GameObject nuh = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomInteractables/GorillaComputerObject/ComputerUI/monitor/monitorScreen");
                if (nuh != null)
                {
                    Renderer ren1 = nuh.GetComponent<Renderer>();
                    if (ren1 != null)
                    {
                        ren1.material = bgMat;
                    }
                }

                // Update leaderboards
                UpdateLeaderboard("Environment Objects/LocalObjects_Prefab/TreeRoom", StumpLeaderboardID, StumpLeaderboardIndex, ref StumpMat);
                UpdateLeaderboard("Environment Objects/LocalObjects_Prefab/Forest", ForestLeaderboardID, ForestLeaderboardIndex, ref ForestMat);

                // Show credits when in room
                if (PhotonNetwork.InRoom)
                {
                    GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdBodyText")
                        .GetComponent<TextMeshPro>().text = "This menu temp is created by @G3IF";
                    boardshit = false;
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Error updating Stump: {e.Message}");
            }
        }

        private static void UpdateLeaderboard(string parentPath, string leaderboardID, int targetIndex, ref Material matRef)
        {
            GameObject parent = GameObject.Find(parentPath);
            if (parent == null) return;

            bool found = false;
            int indexOfThatThing = 0;
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                GameObject v = parent.transform.GetChild(i).gameObject;
                if (v.name.Contains(leaderboardID))
                {
                    indexOfThatThing++;
                    if (indexOfThatThing == targetIndex)
                    {
                        found = true;
                        if (matRef == null)
                            matRef = v.GetComponent<Renderer>().material;

                        v.GetComponent<Renderer>().material = bgMat;
                        break;
                    }
                }
            }
        }

        #endregion

        #region ========== MOD EXECUTION ==========
        // Runs all enabled mods every frame

        private static void UpdateFPSCounter()
        {
            if (fpsObject != null)
            {
                fpsObject.text = "FPS: " + Mathf.Ceil(1f / Time.unscaledDeltaTime).ToString();
            }
        }

        private static void RunEnabledMods()
        {
            foreach (ButtonInfo[] buttonlist in buttons)
            {
                foreach (ButtonInfo v in buttonlist)
                {
                    if (v.enabled && v.method != null)
                    {
                        try
                        {
                            v.method.Invoke();
                        }
                        catch (Exception exc)
                        {
                            UnityEngine.Debug.LogError($"{TvvPancke.Mods.PluginInfo.Name} // Error with mod {v.buttonText} at {exc.StackTrace}: {exc.Message}");
                        }
                    }
                }
            }
        }

        #endregion

        #region ========== MENU CREATION ==========
        // Creates the main menu structure

        public static void CreateMenu()
        {
            // Create root menu object (invisible container)
            menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(menu.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(menu.GetComponent<BoxCollider>());
            UnityEngine.Object.Destroy(menu.GetComponent<Renderer>());
            menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.3825f);

            // Create visual background with rounded corners
            CreateMenuBackground();

            // Create canvas for text and images
            CreateCanvas();

            // Create title and FPS counter
            CreateTitleAndFPS();

            // Create navigation buttons (home, settings, disconnect, etc)
            CreateNavigationButtons();

            // Create buttons for mods on current page
            CreateModButtons();
        }

        private static void CreateMenuBackground()
        {
            menuBackground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(menuBackground.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(menuBackground.GetComponent<BoxCollider>());
            menuBackground.transform.parent = menu.transform;
            menuBackground.transform.rotation = Quaternion.identity;
            menuBackground.transform.localScale = menuSize;
            menuBackground.transform.position = new Vector3(0.05f, 0f, 0f);
            menuBackground.GetComponent<Renderer>().material.color = new Color(101f / 255f, 158f / 255f, 105f / 255f, 1f);
            menuBackground.GetComponent<Renderer>().enabled = false;

            // Create rounded corners
            float bevel = noround ? 0f : CORNER_ROUNDNESS;
            CreateRoundedRectangleParts(menuBackground.transform.localPosition, menuSize, new Color32(25, 25, 25, 255), bevel);

            // Add color changer for theme support
            ColorChanger colorChanger = menuBackground.AddComponent<ColorChanger>();
            colorChanger.colorInfo = backgroundColor;
            colorChanger.Start();

            // Create outline if enabled
            if (Outlineint)
            {
                CreateMenuOutline();
            }
        }

        private static void CreateMenuOutline()
        {
            menu2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(menu2.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(menu2.GetComponent<BoxCollider>());
            UnityEngine.Object.Destroy(menu2.GetComponent<Renderer>());
            menu2.transform.localScale = new Vector3(0.1f, 0.3f, 0.3825f);

            menuBackground2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(menuBackground2.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(menuBackground2.GetComponent<BoxCollider>());
            menuBackground2.transform.parent = menu2.transform;
            menuBackground2.transform.rotation = Quaternion.identity;
            menuBackground2.transform.localScale = menuSize2;
            menuBackground2.transform.position = new Vector3(0.047f, 0f, 0f);
            menuBackground2.GetComponent<Renderer>().material.color = Settings.OutlineColor;
            menuBackground2.GetComponent<Renderer>().enabled = false;

            float bevel2 = noround ? 0f : CORNER_ROUNDNESS;
            CreateRoundedRectangleParts(menuBackground2.transform.localPosition, menuSize2, Settings.OutlineColor, bevel2);
        }

        private static void CreateCanvas()
        {
            canvasObject = new GameObject();
            canvasObject.transform.parent = menu.transform;
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvasScaler.dynamicPixelsPerUnit = 5000f;
        }

        private static void CreateTitleAndFPS()
        {
            // Create title text
            Text text = new GameObject { transform = { parent = canvasObject.transform } }.AddComponent<Text>();
            text.font = currentFont;
            if (shouldAnimate)
            {
                text.text = "";
                GorillaTagger.Instance.StartCoroutine(AnimateTitle(text));
            }
            else
            {
                text.text = TvvPancke.Mods.PluginInfo.Name;
            }
            text.fontSize = 1;
            text.color = textColors[0];
            text.supportRichText = true;
            text.fontStyle = FontStyle.Normal;
            text.alignment = TextAnchor.MiddleCenter;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;

            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(0.28f, 0.06f);
            component.position = new Vector3(0.06f, 0f, 0.09f);
            if (fpsCounter)
            {
                component.position = new Vector3(0.06f, 0f, 0.1086f);
            }
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

            // Create FPS counter if enabled
            if (fpsCounter)
            {
                fpsObject = new GameObject { transform = { parent = canvasObject.transform } }.AddComponent<Text>();
                fpsObject.font = currentFont;
                fpsObject.text = "FPS: " + Mathf.Ceil(1f / Time.unscaledDeltaTime).ToString();
                fpsObject.color = textColors[0];
                fpsObject.fontSize = 1;
                fpsObject.supportRichText = true;
                fpsObject.fontStyle = FontStyle.Normal;
                fpsObject.alignment = TextAnchor.MiddleCenter;
                fpsObject.horizontalOverflow = HorizontalWrapMode.Overflow;
                fpsObject.resizeTextForBestFit = true;
                fpsObject.resizeTextMinSize = 0;

                RectTransform component2 = fpsObject.GetComponent<RectTransform>();
                component2.localPosition = Vector3.zero;
                component2.sizeDelta = new Vector2(0.28f, 0.025f);
                component2.position = new Vector3(0.06f, 0f, 0.064f);
                component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            }
        }

        #endregion

        #region ========== NAVIGATION BUTTONS ==========
        // Creates home, settings, disconnect, next/prev page buttons

        private static void CreateNavigationButtons()
        {
            // Disconnect button
            if (disconnectButton)
            {
                CreateDisconnectButton();
            }

            // Home button (only show if not on home page)
            if (buttonsType > 0)
            {
                CreateHomeButton();
            }

            // Previous page button
            if (shouldPrev)
            {
                CreatePreviousPageButton();
            }

            // Next page button
            if (shouldNext)
            {
                CreateNextPageButton();
            }

            // Settings button
            if (shouldConfig)
            {
                CreateConfigButton();
            }
        }

        private static void CreateDisconnectButton()
        {
            Vector3 position = new Vector3(0.6f, -0.55f, 0.285f);
            Vector3 scale = new Vector3(0.09f, 0.07f, 0.07f);

            GameObject disconnectbutton = CreateButtonWithRoundedCorners(position, scale, new Color32(41, 41, 41, 255));
            disconnectbutton.AddComponent<Classes.Button>().relatedText = "Disconnect";

            ColorChanger colorChanger = disconnectbutton.AddComponent<ColorChanger>();
            colorChanger.colorInfo = buttonColors[0];
            colorChanger.Start();

            // Create outline if enabled
            if (Outlineint)
            {
                CreateButtonOutline(position, new Vector3(0.089f, 0.11f, 0.09f), Settings.OutlineColor);
            }

            // Add icon
            CreateIcon("disconnect.png", ref disconnectIcon, ref disconnectMat, new Vector3(0.066f, -0.166f, 0.108f), new Vector2(0.02f, 0.02f));
        }

        private static void CreateHomeButton()
        {
            Vector3 position = new Vector3(0.56f, 0f, -0.32f);
            Vector3 scale = new Vector3(0.09f, 0.2f, 0.1f);

            GameObject homebutton = CreateButtonWithRoundedCorners(position, scale, new Color32(41, 41, 41, 255));
            homebutton.AddComponent<Classes.Button>().relatedText = "Home3";

            ColorChanger colorChanger = homebutton.AddComponent<ColorChanger>();
            colorChanger.colorInfo = buttonColors[0];
            colorChanger.Start();

            // Create outline if enabled
            if (Outlineint)
            {
                CreateButtonOutline(new Vector3(0.54f, 0f, -0.32f), new Vector3(0.089f, 0.22f, 0.12f), Settings.OutlineColor);
            }

            // Add icon
            CreateIcon("home.png", ref homeIcon, ref homeMat, new Vector3(0.07f, 0f, -0.118f), new Vector2(0.03f, 0.03f));
        }

        private static void CreatePreviousPageButton()
        {
            Vector3 position = new Vector3(0.6f, 0.27f, -0.32f);
            Vector3 scale = new Vector3(0.09f, 0.15f, 0.07f);

            GameObject prevPage = CreateButtonWithRoundedCorners(position, scale, new Color32(45, 113, 192, 255));
            prevPage.AddComponent<Classes.Button>().relatedText = "PreviousPage";

            ColorChanger colorChanger = prevPage.AddComponent<ColorChanger>();
            colorChanger.colorInfo = buttonColors[0];
            colorChanger.Start();

            // Create outline if enabled
            if (Outlineint)
            {
                CreateButtonOutline(position, new Vector3(0.089f, 0.17f, 0.09f), new Color32(25, 93, 172, 255));
            }

            // Add text label
            CreateButtonText("<", new Vector3(0.07f, 0.083f, -0.1199f));
        }

        private static void CreateNextPageButton()
        {
            Vector3 position = new Vector3(0.6f, -0.27f, -0.32f);
            Vector3 scale = new Vector3(0.09f, 0.15f, 0.07f);

            GameObject nextPage = CreateButtonWithRoundedCorners(position, scale, new Color32(45, 113, 192, 255));
            nextPage.AddComponent<Classes.Button>().relatedText = "NextPage";

            ColorChanger colorChanger = nextPage.AddComponent<ColorChanger>();
            colorChanger.colorInfo = buttonColors[0];
            colorChanger.Start();

            // Create outline if enabled
            if (Outlineint)
            {
                CreateButtonOutline(position, new Vector3(0.089f, 0.17f, 0.09f), new Color32(25, 93, 172, 255));
            }

            // Add text label
            CreateButtonText(">", new Vector3(0.07f, -0.083f, -0.1199f));
        }

        private static void CreateConfigButton()
        {
            Vector3 position = new Vector3(0.6f, -0.55f, 0.38f);
            Vector3 scale = new Vector3(0.09f, 0.07f, 0.07f);

            GameObject ConfigButton = CreateButtonWithRoundedCorners(position, scale, new Color32(45, 45, 45, 255));
            ConfigButton.AddComponent<Classes.Button>().relatedText = "Config";

            ColorChanger colorChanger = ConfigButton.AddComponent<ColorChanger>();
            colorChanger.colorInfo = buttonColors[0];
            colorChanger.Start();

            // Create outline if enabled
            if (Outlineint)
            {
                CreateButtonOutline(position, new Vector3(0.089f, 0.11f, 0.09f), Settings.OutlineColor);
            }

            // Add icon
            CreateIcon("settingsicon.png", ref settingsIcon, ref settingsMat, new Vector3(0.066f, -0.164f, 0.1445f), new Vector2(0.02f, 0.02f));
        }

        #endregion

        #region ========== MOD BUTTONS ==========
        // Creates buttons for the mods on current page

        private static void CreateModButtons()
        {
            ButtonInfo[] activeButtons = buttons[buttonsType].Skip(pageNumber * buttonsPerPage).Take(buttonsPerPage).ToArray();
            for (int i = 0; i < activeButtons.Length; i++)
            {
                CreateButton(i * BUTTON_SPACING, activeButtons[i]);
            }
        }

        public static void CreateButton(float offset, ButtonInfo method)
        {
            Vector3 position = new Vector3(0.56f, -0.32f, 0.1f - offset);
            Vector3 scale = new Vector3(0.1f, 0.1f, 0.0825f);
            Color32 color = method.enabled ? new Color32(50, 198, 122, 255) : new Color32(45, 45, 45, 255);

            // Create button
            GameObject button = CreateButtonWithRoundedCorners(position, scale, color);
            button.AddComponent<Classes.Button>().relatedText = method.buttonText;

            // Add color changer
            ColorChanger colorChanger = button.AddComponent<ColorChanger>();
            colorChanger.colorInfo = method.enabled ? buttonColors[1] : buttonColors[0];
            colorChanger.Start();

            // Add enabled checkmark if mod is on
            if (method.enabled)
            {
                CreateIcon("enabled.png", ref enabledIcon, ref enabledMat,
                    new Vector3(0.066f, -0.095f, 0.037f - offset / 2.65f), new Vector2(0.02f, 0.02f));
            }

            // Create button label
            Text text = new GameObject { transform = { parent = canvasObject.transform } }.AddComponent<Text>();
            text.font = currentFont;
            text.text = method.overlapText ?? method.buttonText;
            text.supportRichText = true;
            text.fontSize = 1;
            text.color = method.enabled ? textColors[1] : textColors[0];
            text.alignment = TextAnchor.MiddleLeft;
            text.fontStyle = FontStyle.Normal;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;

            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(.15f, .02f);
            component.localPosition = new Vector3(.064f, 0.032f, .039f - offset / 2.6f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }

        #endregion

        #region ========== HELPER FUNCTIONS ==========
        // Reusable functions for creating UI elements

        private static GameObject CreateButtonWithRoundedCorners(Vector3 position, Vector3 scale, Color32 color)
        {
            // Create invisible button (for collider)
            GameObject button = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (!UnityInput.Current.GetKey(KeyCode.F))
            {
                button.layer = 2;
            }
            UnityEngine.Object.Destroy(button.GetComponent<Rigidbody>());
            button.GetComponent<BoxCollider>().isTrigger = true;
            button.transform.parent = menu.transform;
            button.transform.rotation = Quaternion.identity;
            button.transform.localScale = scale;
            button.transform.localPosition = position;
            button.GetComponent<Renderer>().enabled = false;

            // Create visual rounded shape
            CreateRoundedRectangleParts(position, scale, color, BUTTON_CORNER_ROUNDNESS);

            return button;
        }

        private static void CreateRoundedRectangleParts(Vector3 position, Vector3 scale, Color32 color, float bevel)
        {
            // Create two base rectangles (horizontal and vertical strips)
            GameObject BaseA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            BaseA.GetComponent<Renderer>().enabled = true;
            UnityEngine.Object.Destroy(BaseA.GetComponent<Collider>());
            BaseA.transform.parent = menu.transform;
            BaseA.transform.rotation = Quaternion.identity;
            BaseA.transform.localPosition = position;
            BaseA.transform.localScale = scale + new Vector3(0f, bevel * -2.55f, 0f);
            BaseA.GetComponent<Renderer>().material.color = color;

            GameObject BaseB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            BaseB.GetComponent<Renderer>().enabled = true;
            UnityEngine.Object.Destroy(BaseB.GetComponent<Collider>());
            BaseB.transform.parent = menu.transform;
            BaseB.transform.rotation = Quaternion.identity;
            BaseB.transform.localPosition = position;
            BaseB.transform.localScale = scale + new Vector3(0f, 0f, -bevel * 2f);
            BaseB.GetComponent<Renderer>().material.color = color;

            // Create four rounded corners using cylinders
            CreateCornerCylinder(position, scale, bevel, color, 1, 1);   // Top-Front
            CreateCornerCylinder(position, scale, bevel, color, -1, 1);  // Bottom-Front
            CreateCornerCylinder(position, scale, bevel, color, 1, -1);  // Top-Back
            CreateCornerCylinder(position, scale, bevel, color, -1, -1); // Bottom-Back
        }

        private static void CreateCornerCylinder(Vector3 basePosition, Vector3 scale, float bevel, Color32 color, int yDir, int zDir)
        {
            GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.GetComponent<Renderer>().enabled = true;
            UnityEngine.Object.Destroy(cylinder.GetComponent<Collider>());
            cylinder.transform.parent = menu.transform;
            cylinder.transform.rotation = Quaternion.Euler(0f, 0f, 90f);

            Vector3 cornerPosition = basePosition + new Vector3(
                0f,
                (scale.y / 2f - bevel * 1.275f) * yDir,
                (scale.z / 2f - bevel) * zDir
            );
            cylinder.transform.localPosition = cornerPosition;
            cylinder.transform.localScale = new Vector3(bevel * 2.55f, scale.x / 2f, bevel * 2f);
            cylinder.GetComponent<Renderer>().material.color = color;
        }

        private static void CreateButtonOutline(Vector3 position, Vector3 scale, Color32 color)
        {
            CreateRoundedRectangleParts(position, scale, color, BUTTON_CORNER_ROUNDNESS);
        }

        private static void CreateButtonText(string text, Vector3 position)
        {
            Text textObj = new GameObject { transform = { parent = canvasObject.transform } }.AddComponent<Text>();
            textObj.font = currentFont;
            textObj.text = text;
            textObj.fontSize = 1;
            textObj.color = textColors[0];
            textObj.alignment = TextAnchor.MiddleCenter;
            textObj.resizeTextForBestFit = true;
            textObj.resizeTextMinSize = 0;

            RectTransform component = textObj.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(0.2f, 0.03f);
            component.localPosition = position;
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }

        private static void CreateIcon(string resourceName, ref Texture2D icon, ref Material mat, Vector3 position, Vector2 size)
        {
            Image image = new GameObject { transform = { parent = canvasObject.transform } }.AddComponent<Image>();
            if (icon == null)
            {
                icon = LoadTextureFromResource(resourceName);
            }
            if (mat == null)
            {
                mat = new Material(image.material);
            }
            image.material = mat;
            image.material.SetTexture("_MainTex", icon);

            RectTransform rect = image.GetComponent<RectTransform>();
            rect.localPosition = Vector3.zero;
            rect.sizeDelta = size;
            rect.localPosition = position;
            rect.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }

        public static void OutlineObj(GameObject obj, Color clr)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(gameObject.GetComponent<BoxCollider>());
            gameObject.transform.parent = obj.transform;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localPosition = obj.transform.localPosition;
            gameObject.transform.localScale = obj.transform.localScale + new Vector3(-0.025f, 0.0125f, 0.005f);
            gameObject.GetComponent<Renderer>().material.color = ColorLib.kiwidark;
        }

        #endregion

        #region ========== ANIMATIONS ==========
        // Smooth animations for opening and closing

        public static IEnumerator OpenAni()
        {
            if (menu == null) yield break;

            GorillaTagger.Instance.StartCoroutine(PlaySFX(openSfxUrl));

            float elapsed = 0f;
            Vector3 startScale = Vector3.zero;
            Vector3 targetScale = new Vector3(0.1f, 0.3f, 0.3825f);

            while (elapsed < OPEN_ANIMATION_SPEED)
            {
                if (menu == null) yield break;

                float t = elapsed / OPEN_ANIMATION_SPEED;
                float s = 1.70158f;
                t -= 1f;
                float bounce = (t * t * ((s + 1f) * t + s) + 1f);

                menu.transform.localScale = Vector3.LerpUnclamped(startScale, targetScale, bounce);
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (menu != null)
                menu.transform.localScale = targetScale;
        }

        public static IEnumerator CloseAni()
        {
            if (menu == null || Close) yield break;
            Close = true;

            GorillaTagger.Instance.StartCoroutine(PlaySFX(closeSfxUrl));

            float elapsed = 0f;
            Vector3 startScale = menu.transform.localScale;
            Vector3 targetScale = Vector3.zero;

            while (elapsed < CLOSE_ANIMATION_SPEED)
            {
                if (menu == null) yield break;

                float t = elapsed / CLOSE_ANIMATION_SPEED;
                float s = 1.70158f;
                float bounce = t * t * ((s + 1f) * t - s);

                menu.transform.localScale = Vector3.LerpUnclamped(startScale, targetScale, bounce);
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (menu != null)
                UnityEngine.Object.Destroy(menu);
            menu = null;

            if (reference != null)
                UnityEngine.Object.Destroy(reference);
            reference = null;

            Close = false;
        }

        public static IEnumerator WobbleOpenAni()
        {
            if (menu == null) yield break;
            GorillaTagger.Instance.StartCoroutine(PlaySFX(openSfxUrl));

            float elapsed = 0f;
            Vector3 startScale = Vector3.zero;
            Vector3 targetScale = new Vector3(0.1f, 0.3f, 0.3825f);
            Vector3 originalPosition = menu.transform.localPosition;

            while (elapsed < WOBBLE_OPEN_SPEED)
            {
                if (menu == null) yield break;
                float t = elapsed / WOBBLE_OPEN_SPEED;

                // Overshoot easing
                float c1 = 1.70158f;
                float c3 = c1 + 1f;
                float overshoot = 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);

                // Wobble effect
                float wobbleX = Mathf.Sin(t * Mathf.PI * 6f) * (1f - t) * 0.02f;
                float wobbleY = Mathf.Cos(t * Mathf.PI * 8f) * (1f - t) * 0.01f;

                menu.transform.localScale = Vector3.LerpUnclamped(startScale, targetScale, overshoot);
                menu.transform.localPosition = originalPosition + new Vector3(wobbleX, wobbleY, 0);

                elapsed += Time.deltaTime;
                yield return null;
            }

            if (menu != null)
            {
                menu.transform.localScale = targetScale;
                menu.transform.localPosition = originalPosition;
            }
        }

        public static IEnumerator WobbleCloseAni()
        {
            if (menu == null || Close) yield break;
            Close = true;
            GorillaTagger.Instance.StartCoroutine(PlaySFX(closeSfxUrl));

            float elapsed = 0f;
            Vector3 startScale = menu.transform.localScale;
            Vector3 targetScale = Vector3.zero;
            Vector3 originalPosition = menu.transform.localPosition;

            while (elapsed < WOBBLE_CLOSE_SPEED)
            {
                if (menu == null) yield break;
                float t = elapsed / WOBBLE_CLOSE_SPEED;

                // Ease out back
                float c1 = 1.70158f;
                float c3 = c1 + 1f;
                float easeOut = 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);

                // Shrink wobble
                float wobble = Mathf.Sin(t * Mathf.PI * 4f) * t * 0.015f;

                menu.transform.localScale = Vector3.LerpUnclamped(startScale, targetScale, easeOut);
                menu.transform.localPosition = originalPosition + Vector3.up * wobble;

                elapsed += Time.deltaTime;
                yield return null;
            }

            if (menu != null)
                UnityEngine.Object.Destroy(menu);
            menu = null;
            if (reference != null)
                UnityEngine.Object.Destroy(reference);
            reference = null;
            Close = false;
        }

        public static IEnumerator ElasticOpenAni()
        {
            if (menu == null) yield break;
            GorillaTagger.Instance.StartCoroutine(PlaySFX(openSfxUrl));

            float elapsed = 0f;
            Vector3 startScale = Vector3.zero;
            Vector3 targetScale = new Vector3(0.1f, 0.3f, 0.3825f);
            Quaternion startRotation = menu.transform.localRotation;
            Quaternion targetRotation = startRotation * Quaternion.Euler(0, 360f, 0);

            while (elapsed < ELASTIC_OPEN_SPEED)
            {
                if (menu == null) yield break;
                float t = elapsed / ELASTIC_OPEN_SPEED;

                // Elastic easing function
                float c4 = (2f * Mathf.PI) / 3f;
                float elastic = t == 0 ? 0 : t == 1 ? 1 :
                    Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1f;

                menu.transform.localScale = Vector3.LerpUnclamped(startScale, targetScale, elastic);
                menu.transform.localRotation = Quaternion.LerpUnclamped(startRotation, targetRotation, t * t);

                elapsed += Time.deltaTime;
                yield return null;
            }

            if (menu != null)
            {
                menu.transform.localScale = targetScale;
                menu.transform.localRotation = startRotation;
            }
        }

        public static IEnumerator ElasticCloseAni()
        {
            if (menu == null || Close) yield break;
            Close = true;
            GorillaTagger.Instance.StartCoroutine(PlaySFX(closeSfxUrl));

            float elapsed = 0f;
            Vector3 startScale = menu.transform.localScale;
            Vector3 targetScale = Vector3.zero;
            Quaternion startRotation = menu.transform.localRotation;
            Quaternion targetRotation = startRotation * Quaternion.Euler(0, -180f, 0);

            while (elapsed < ELASTIC_CLOSE_SPEED)
            {
                if (menu == null) yield break;
                float t = elapsed / ELASTIC_CLOSE_SPEED;

                // Ease in cubic
                float cubic = t * t * t;

                menu.transform.localScale = Vector3.LerpUnclamped(startScale, targetScale, cubic);
                menu.transform.localRotation = Quaternion.LerpUnclamped(startRotation, targetRotation, t);

                elapsed += Time.deltaTime;
                yield return null;
            }

            if (menu != null)
                UnityEngine.Object.Destroy(menu);
            menu = null;
            if (reference != null)
                UnityEngine.Object.Destroy(reference);
            reference = null;
            Close = false;
        }

        private static IEnumerator AnimateTitle(Text text)
        {
            string targetText = TvvPancke.Mods.PluginInfo.Name;
            string currentText = "";

            while (true)
            {
                for (int i = 0; i <= targetText.Length; i++)
                {
                    currentText = targetText.Substring(0, i);
                    text.text = currentText;
                    yield return new WaitForSeconds(0.2f);
                }
                yield return new WaitForSeconds(0.25f);
                text.text = "";
                yield return new WaitForSeconds(0.2f);
            }
        }

        public static IEnumerator PlaySFX(string url)
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError ||
                    www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Audio download error: " + www.error);
                }
                else
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                    GameObject go = new GameObject("felsound");
                    AudioSource source = go.AddComponent<AudioSource>();
                    source.clip = clip;
                    source.Play();
                    UnityEngine.Object.Destroy(go, clip.length);
                }
            }
        }

        #endregion

        #region ========== MENU POSITIONING ==========
        // Handles positioning the menu in VR or keyboard mode

        public static void RecreateMenu()
        {
            if (menu != null)
            {
                UnityEngine.Object.Destroy(menu);
                menu = null;

                CreateMenu();
                RecenterMenu(rightHanded, UnityInput.Current.GetKey(keyboardButton));
            }
        }

        public static void RecenterMenu(bool isRightHanded, bool isKeyboardCondition)
        {
            if (!isKeyboardCondition)
            {
                // VR Controller Mode
                if (!isRightHanded)
                {
                    menu.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                    menu.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
                }
                else
                {
                    menu.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    Vector3 rotation = GorillaTagger.Instance.rightHandTransform.rotation.eulerAngles;
                    rotation += new Vector3(0f, 0f, 180f);
                    menu.transform.rotation = Quaternion.Euler(rotation);
                }
            }
            else
            {
                // Keyboard Mode
                try
                {
                    TPC = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera").GetComponent<Camera>();
                }
                catch { }

                GameObject.Find("Shoulder Camera").transform.Find("CM vcam1").gameObject.SetActive(false);

                if (TPC != null)
                {
                    TPC.transform.position = new Vector3(-999f, -999f, -999f);
                    TPC.transform.rotation = Quaternion.identity;
                    menu.transform.parent = TPC.transform;
                    menu.transform.position = (TPC.transform.position +
                        (Vector3.Scale(TPC.transform.forward, new Vector3(0.5f, 0.5f, 0.5f)))) +
                        (Vector3.Scale(TPC.transform.up, new Vector3(-0.02f, -0.02f, -0.02f)));
                    Vector3 rot = TPC.transform.rotation.eulerAngles;
                    rot = new Vector3(rot.x - 90, rot.y + 90, rot.z);
                    menu.transform.rotation = Quaternion.Euler(rot);

                    // Handle mouse clicking
                    if (reference != null)
                    {
                        if (Mouse.current.leftButton.isPressed)
                        {
                            Ray ray = TPC.ScreenPointToRay(Mouse.current.position.ReadValue());
                            RaycastHit hit;
                            bool worked = Physics.Raycast(ray, out hit, 100);
                            if (worked)
                            {
                                Classes.Button collide = hit.transform.gameObject.GetComponent<Classes.Button>();
                                if (collide != null)
                                {
                                    collide.OnTriggerEnter(buttonCollider);
                                }
                            }
                        }
                        else
                        {
                            reference.transform.position = new Vector3(999f, -999f, -999f);
                        }
                    }
                }
            }
        }

        public static void CreateReference(bool isRightHanded)
        {
            reference = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            if (isRightHanded)
            {
                reference.transform.parent = GorillaTagger.Instance.leftHandTransform;
            }
            else
            {
                reference.transform.parent = GorillaTagger.Instance.rightHandTransform;
            }
            reference.GetComponent<Renderer>().material.color = backgroundColor.colors[0].color;
            reference.transform.localPosition = new Vector3(0f, -0.1f, 0f);
            reference.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            buttonCollider = reference.GetComponent<SphereCollider>();

            ColorChanger colorChanger = reference.AddComponent<ColorChanger>();
            colorChanger.colorInfo = backgroundColor;
            colorChanger.Start();
        }

        #endregion

        #region ========== BUTTON CLICK HANDLING ==========
        // Handles what happens when you click a button

        public static void Toggle(string buttonText)
        {
            int lastPage = ((buttons[buttonsType].Length + buttonsPerPage - 1) / buttonsPerPage) - 1;

            // Handle page navigation
            if (buttonText == "PreviousPage")
            {
                pageNumber--;
                if (pageNumber < 0)
                {
                    pageNumber = lastPage;
                }
            }
            else if (buttonText == "Config")
            {
                SettingsMods.EnterSettings();
            }
            else if (buttonText == "Panic")
            {
                foreach (ButtonInfo[] buttonInfo in buttons)
                {
                    foreach (ButtonInfo button in buttonInfo)
                    {
                        if (button.enabled)
                        {
                            button.enabled = false;
                        }
                    }
                }
            }
            else if (buttonText == "NextPage")
            {
                pageNumber++;
                if (pageNumber > lastPage)
                {
                    pageNumber = 0;
                }
            }
            else if (buttonText == "Home")
            {
                TvvPancke.Mods.Global.ReturnHome();
            }
            else
            {
                // Handle mod toggle/activation
                ButtonInfo target = GetIndex(buttonText);
                if (target != null)
                {
                    if (target.isTogglable)
                    {
                        target.enabled = !target.enabled;
                        if (target.enabled)
                        {
                            if (target.enableMethod != null)
                            {
                                NotifiLib.SendNotification("[<color=grey>Xe</color><color=cyan>non</color>]: " + target.toolTip);
                                try { target.enableMethod.Invoke(); } catch { }
                            }
                        }
                        else
                        {
                            if (target.disableMethod != null)
                            {
                                NotifiLib.SendNotification("[<color=grey>Xe</color><color=cyan>non</color>]: " + target.toolTip);
                                try { target.disableMethod.Invoke(); } catch { }
                            }
                        }
                    }
                    else
                    {
                        if (target.method != null)
                        {
                            NotifiLib.SendNotification("[<color=grey>Xe</color><color=cyan>non</color>]: " + target.toolTip);
                            try { target.method.Invoke(); } catch { }
                        }
                    }
                }
                else
                {
                    UnityEngine.Debug.LogError(buttonText + " does not exist");
                }
            }

            RecreateMenu();
        }

        public static ButtonInfo GetIndex(string buttonText)
        {
            foreach (ButtonInfo[] buttons in Menu.Buttons.buttons)
            {
                foreach (ButtonInfo button in buttons)
                {
                    if (button.buttonText == buttonText)
                    {
                        return button;
                    }
                }
            }
            return null;
        }

        #endregion

        #region ========== THEME MANAGEMENT ==========
        // Functions for changing menu colors and themes

        public static GradientColorKey[] GetSolidGradient(Color color, float v)
        {
            return new GradientColorKey[] { new GradientColorKey(color, 0f), new GradientColorKey(color, 1f) };
        }

        public static void UpdateThemeButtonTexts()
        {
            var menuSettings = Buttons.buttons[2];
            foreach (var btn in menuSettings)
            {
                if (btn.buttonText.StartsWith("Background Theme ["))
                    btn.buttonText = $"Background Theme [{Settings.menuBackgroundThemes[Settings.currentBackgroundThemeIndex].name}]";
                else if (btn.buttonText.StartsWith("Button Theme ["))
                    btn.buttonText = $"Button Theme [{Settings.menuButtonThemes[Settings.currentButtonThemeIndex].name}]";
            }
        }

        public static void CycleMenuTheme()
        {
            Settings.currentButtonThemeIndex = (Settings.currentButtonThemeIndex + 1) % Settings.menuButtonThemes.Length;
            var theme = Settings.menuButtonThemes[Settings.currentButtonThemeIndex];
            buttonColors[0].colors = GetSolidGradient(theme.color, 1f);
            UpdateThemeButtonTexts();
            RecreateMenu();
        }

        public static void CycleMenuBackgroundTheme()
        {
            Settings.currentBackgroundThemeIndex = (Settings.currentBackgroundThemeIndex + 1) % Settings.menuBackgroundThemes.Length;
            var theme = Settings.menuBackgroundThemes[Settings.currentBackgroundThemeIndex];
            backgroundColor.colors = GetSolidGradient(theme.color, 1f);
            UpdateThemeButtonTexts();
            RecreateMenu();
        }

        public static void RandomizeMenuTheme()
        {
            // Randomize background
            Settings.currentBackgroundThemeIndex = UnityEngine.Random.Range(0, Settings.menuBackgroundThemes.Length);
            var bgTheme = Settings.menuBackgroundThemes[Settings.currentBackgroundThemeIndex];
            backgroundColor.colors = GetSolidGradient(bgTheme.color, 1f);

            // Randomize button
            Settings.currentButtonThemeIndex = UnityEngine.Random.Range(0, Settings.menuButtonThemes.Length);
            var btnTheme = Settings.menuButtonThemes[Settings.currentButtonThemeIndex];
            buttonColors[0].colors = GetSolidGradient(btnTheme.color, 1f);

            RecreateMenu();
        }

        public static void ResetMenuThemes()
        {
            // Reset theme indices to 0 (original)
            Settings.currentBackgroundThemeIndex = 0;
            Settings.currentButtonThemeIndex = 0;
            Settings.currentThemeNameIndex = 0;

            // Reset background and button colors to their defaults
            backgroundColor.colors = GetSolidGradient(Settings.menuBackgroundThemes[0].color, 1f);
            buttonColors[0].colors = GetSolidGradient(Settings.menuButtonThemes[0].color, 1f);
            RecreateMenu();
        }

        #endregion

        #region ========== UTILITY FUNCTIONS ==========
        // Misc helper functions

        public static void ColorLib2(Color color, object target = null)
        {
            PlayerPrefs.SetFloat("redValue", Mathf.Clamp(color.r, 0f, 1f));
            PlayerPrefs.SetFloat("greenValue", Mathf.Clamp(color.g, 0f, 1f));
            PlayerPrefs.SetFloat("blueValue", Mathf.Clamp(color.b, 0f, 1f));

            GorillaTagger.Instance.UpdateColor(color.r, color.g, color.b);
            PlayerPrefs.Save();

            try
            {
                if (target == null)
                    GorillaTagger.Instance.myVRRig.SendRPC("RPC_InitializeNoobMaterial", RpcTarget.All, new object[] { color.r, color.g, color.b });
                else
                {
                    if (target is NetPlayer player)
                        GorillaTagger.Instance.myVRRig.SendRPC("RPC_InitializeNoobMaterial", player, new object[] { color.r, color.g, color.b });
                    else if (target is RpcTarget targets)
                        GorillaTagger.Instance.myVRRig.SendRPC("RPC_InitializeNoobMaterial", targets, new object[] { color.r, color.g, color.b });
                }
            }
            catch { }
        }

        public static void Render(Vector3 position, float range, Color color)
        {
            GameObject what = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            UnityEngine.Object.Destroy(what, Time.deltaTime);
            UnityEngine.Object.Destroy(what.GetComponent<Collider>());
            UnityEngine.Object.Destroy(what.GetComponent<Rigidbody>());
            what.transform.position = position;
            what.transform.localScale = new Vector3(range, range, range);
            Color clr = color;
            clr.a = 0.25f;
            what.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
            what.GetComponent<Renderer>().material.color = clr;
        }

        public static Texture2D LoadTextureFromResource(string resourceName)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Texture2D texture2D;
            using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream("Xenon.Resources." + resourceName))
            {
                if (manifestResourceStream == null)
                {
                    Debug.LogError("Resource not found: " + resourceName);
                    texture2D = null;
                }
                else
                {
                    byte[] array;
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        manifestResourceStream.CopyTo(memoryStream);
                        array = memoryStream.ToArray();
                    }
                    Texture2D texture2D2 = new Texture2D(2, 2);
                    if (texture2D2.LoadImage(array))
                    {
                        Debug.Log("Loaded texture: " + resourceName);
                        texture2D = texture2D2;
                    }
                    else
                    {
                        Debug.LogError("Failed to load image from resource stream.");
                        texture2D = null;
                    }
                }
            }
            return texture2D;
        }


        #endregion
    }
}