using TvvPancke.Classes;
using TvvPancke.Menu;
using TvvPancke.Notifications;
using UnityEngine;
using static TvvPancke.Menu.Main;

namespace TvvPancke
{
    internal class Settings
    {
        #region ========== EASY SETTINGS - Change These! ==========
        // This section contains values beginners can safely change

        // How many buttons appear on each page of the menu
        public static int buttonsPerPage = 4;

        // Which hand holds the menu (false = left hand, true = right hand)
        public static bool rightHanded = false;
        public static bool righthand = false; // Duplicate for compatibility

        // Keyboard button to open menu (default is F key)
        public static KeyCode keyboardButton = KeyCode.F;

        // Feature toggles
        public static bool fpsCounter = true;          // Show FPS counter at top of menu
        public static bool disconnectButton = true;    // Show disconnect button
        public static bool disableNotifications = false; // Turn off notifications
        public static bool Ghostview = false;          // Ghost view mode

        // ESP (extra sensory perception) setting
        // 1 = Basic ESP, 2 = Advanced ESP, etc.
        public static int espSetting = 1;

        #endregion

        #region ========== MENU SIZE SETTINGS ==========
        // Controls the physical size of the menu in VR space

        // Main menu size (Depth, Width, Height)
        public static Vector3 menuSize = new Vector3(0.1f, 0.85f, 0.8f);

        // Outline/border size (slightly bigger than main menu)
        public static Vector3 menuSize2 = new Vector3(0.1f, 0.9f, 0.85f);

        #endregion

        #region ========== COLOR SETTINGS ==========
        // Colors for menu background, buttons, and text

        // Background color gradient
        public static ExtGradient backgroundColor = new ExtGradient { isRainbow = false };

        // Button colors [0] = disabled, [1] = enabled
        public static ExtGradient[] buttonColors = new ExtGradient[]
        {
            new ExtGradient { colors = GetSolidGradient(new Color(0.0f, 0.5f, 1.0f), 0f) },
            new ExtGradient { colors = GetSolidGradient(new Color(0.53f, 0.98f, 0.82f), 0f) }   // Enabled (blue)
        };

        // Text colors [0] = disabled, [1] = enabled
        public static Color[] textColors = new Color[]
        {
            Color.white, // Disabled text color
            Color.white  // Enabled text color
        };

        // Current menu background color
        public static Color32 CurrentBackgroundColor = new Color(0.56f, 0.27f, 0.68f);

        // Outline/border color
        public static Color32 OutlineColor = new Color(0.91f, 0.27f, 0.47f);

        // Cached menu background color
        public static Color MenuBG = backgroundColor.colors[0].color;

        #endregion

        #region ========== FONT SETTINGS ==========
        // Font used for all menu text

        public static Font currentFont = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);

        #endregion

        // Predefined color themes that users can cycle through

        // Current theme indices (which theme is selected)
        public static int currentThemeNameIndex = 0;
        public static int currentBackgroundThemeIndex = 0;
        public static int currentButtonThemeIndex = 0;

        // Background theme options
        // Each entry has: (name, color)
        public static readonly (string name, Color color)[] menuBackgroundThemes = new (string, Color)[]
        {
            ("a", new Color32(101, 97, 148, 255)),       // Purple-gray
            ("b", new Color(0.13f, 0.75f, 0.47f)),       // Teal
            ("c", new Color(0.023f, 0.580f, 0.145f)),    // Dark green
            ("d", new Color(0.98f, 0.53f, 0.22f)),       // Orange
            ("e", new Color(0.56f, 0.27f, 0.68f)),       // Purple
            ("f", new Color(0.91f, 0.27f, 0.47f)),       // Pink
            ("g", new Color(0.98f, 0.82f, 0.22f)),       // Yellow
            ("h", new Color(0.36f, 0.45f, 0.51f)),       // Blue-gray
            ("i", new Color(0.53f, 0.98f, 0.82f)),       // Mint
            ("j", new Color(1.0f, 0.49f, 0.38f)),        // Coral
            ("k", new Color(0.18f, 0.22f, 0.25f)),       // Dark gray
            ("l", new Color(0.8f, 0.07f, 0.2f)),         // Red
            ("m", new Color(0.1f, 0.2f, 0.7f)),          // Blue
            ("n", new Color(1.0f, 0.75f, 0.2f)),         // Gold
            ("o", new Color(0.13f, 0.55f, 0.13f)),       // Forest green
            ("p", Color.cyan),                            // Cyan
            ("q", Color.magenta),                         // Magenta
            ("r", new Color(1f, 1f, 0.94f)),             // Cream
            ("s", new Color(0.48f, 0.25f, 0.0f)),        // Brown
            ("t", new Color(0.5f, 0.5f, 0.0f)),          // Olive
            ("u", new Color(0.0f, 0.5f, 0.5f)),          // Teal dark
            ("v", new Color(0.9f, 0.89f, 0.89f)),        // Light gray
            ("w", new Color(0.29f, 0.0f, 0.51f)),        // Dark purple
            ("x", new Color(0.86f, 0.08f, 0.24f)),       // Crimson
            ("y", new Color(0.0f, 0.5f, 1.0f)),          // Sky blue
            ("z", new Color(0.94f, 0.92f, 0.84f)),       // Beige
            ("!", new Color(0.76f, 0.7f, 0.5f)),         // Tan
            (".", new Color(0.8f, 0.5f, 0.2f)),          // Burnt orange
            ("? ", new Color(0.27f, 0.51f, 0.71f)),      // Steel blue
            ("0", new Color(1.0f, 0.46f, 0.09f))         // Dark orange
        };

        // Button theme options (same colors, just for buttons)
        public static readonly (string name, Color color)[] menuButtonThemes = new (string, Color)[]
        {
            ("", new Color32(121, 116, 177, 255)),       // Purple-gray
            ("", new Color(0.13f, 0.75f, 0.47f)),        // Teal
            ("", new Color(0.98f, 0.53f, 0.22f)),        // Orange
            ("", new Color(0.56f, 0.27f, 0.68f)),        // Purple
            ("", new Color(0.91f, 0.27f, 0.47f)),        // Pink
            ("", new Color(0.98f, 0.82f, 0.22f)),        // Yellow
            ("", new Color(0.36f, 0.45f, 0.51f)),        // Blue-gray
            ("", new Color(0.53f, 0.98f, 0.82f)),        // Mint
            ("", new Color(1.0f, 0.49f, 0.38f)),         // Coral
            ("", new Color(0.18f, 0.22f, 0.25f)),        // Dark gray
            ("", new Color(0.8f, 0.07f, 0.2f)),          // Red
            ("", new Color(0.1f, 0.2f, 0.7f)),           // Blue
            ("", new Color(1.0f, 0.75f, 0.2f)),          // Gold
            ("", new Color(0.13f, 0.55f, 0.13f)),        // Forest green
            ("", Color.cyan),                             // Cyan
            ("", Color.magenta),                          // Magenta
            ("", new Color(1f, 1f, 0.94f)),              // Cream
            ("", new Color(0.48f, 0.25f, 0.0f)),         // Brown
            ("", new Color(0.5f, 0.5f, 0.0f)),           // Olive
            ("", new Color(0.0f, 0.5f, 0.5f)),           // Teal dark
            ("", new Color(0.9f, 0.89f, 0.89f)),         // Light gray
            ("", new Color(0.29f, 0.0f, 0.51f)),         // Dark purple
            ("", new Color(0.86f, 0.08f, 0.24f)),        // Crimson
            ("", new Color(0.0f, 0.5f, 1.0f)),           // Sky blue
            ("", new Color(0.94f, 0.92f, 0.84f)),        // Beige
            ("", new Color(0.76f, 0.7f, 0.5f)),          // Tan
            ("", new Color(0.8f, 0.5f, 0.2f)),           // Burnt orange
            (" ", new Color(0.27f, 0.51f, 0.71f)),       // Steel blue
            ("", new Color(1.0f, 0.46f, 0.09f)),         // Dark orange
            ("", Color.black)                             // Black
        };

    }
}