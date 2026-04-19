using System.Net;
using UnityEngine;

namespace Colorlib
{
    

    public class ColorLib
    {
      
        public static void UpdateClr()
        {
            float num = Mathf.PingPong(Time.time * 0.3f, 1f);
            float num2 = 0.75f;
            ColorLib.RGB.color = Color.HSVToRGB(num, 1f, num2);
            ColorLib.hexColor = "#" + ColorUtility.ToHtmlStringRGB(ColorLib.RGB.color);
            ColorLib.hexColor1 = "#" + ColorUtility.ToHtmlStringRGB(ColorLib.DFade.color);
            ColorLib.DFade.color = Color.Lerp(ColorLib.Indigo, ColorLib.DarkPurple, Mathf.PingPong(Time.time, 1f));
            ColorLib.DBreath.color = Color.Lerp(ColorLib.Indigo, ColorLib.DarkPurple, Mathf.PingPong(Time.time, 1.5f));
        }


        
        /// COLORS YOU MIGHT WANT, DO NOT DELETE ANY OF THESE
      
        
        public static Color32 purple123 = new Color32(25, 25, 25, byte.MaxValue);
        
        public static Color32 idk = new Color32(111, 252, 243, byte.MaxValue);
      
        public static Color32 Red = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);
      
        public static Color32 DarkRed = new Color32(180, 0, 0, byte.MaxValue);
        
        public static Color32 Salmon = new Color32(250, 128, 114, byte.MaxValue);
       
        public static Color32 WineRed = new Color32(123, 0, 0, byte.MaxValue);
      
        public static Color32 IndianRed = new Color32(205, 92, 92, byte.MaxValue);
       
        public static Color32 Crimson = new Color32(220, 20, 60, byte.MaxValue);

        public static Color32 FireBrick = new Color32(178, 34, 34, byte.MaxValue);

        public static Color32 Coral = new Color32(byte.MaxValue, 127, 80, byte.MaxValue);

        public static Color32 DarkCoral = new Color32(235, 107, 60, byte.MaxValue);

        public static Color32 Tomato = new Color32(byte.MaxValue, 99, 71, byte.MaxValue);

        public static Color32 Maroon = new Color32(128, 0, 0, byte.MaxValue);

        public static Color32 Green = new Color32(0, byte.MaxValue, 0, byte.MaxValue);

        public static Color32 Lime = new Color32(0, 128, 0, byte.MaxValue);

        public static Color32 DarkGreen = new Color32(0, 100, 0, byte.MaxValue);

        public static Color32 Olive = new Color32(128, 128, 0, byte.MaxValue);

        public static Color32 ForestGreen = new Color32(34, 139, 34, byte.MaxValue);

        public static Color32 SeaGreen = new Color32(46, 139, 87, byte.MaxValue);

        public static Color32 MediumSeaGreen = new Color32(60, 179, 113, byte.MaxValue);

        public static Color32 Aquamarine = new Color32(127, byte.MaxValue, 212, byte.MaxValue);

        public static Color32 MediumAquamarine = new Color32(102, 205, 170, byte.MaxValue);

        public static Color32 DarkSeaGreen = new Color32(143, 188, 143, byte.MaxValue);

        public static Color32 Blue = new Color32(0, 0, byte.MaxValue, byte.MaxValue);

        public static Color32 Navy = new Color32(0, 0, 128, byte.MaxValue);

        public static Color32 DarkBlue = new Color32(0, 0, 160, byte.MaxValue);

        public static Color32 RoyalBlue = new Color32(65, 105, 225, byte.MaxValue);

        public static Color32 DodgerBlue = new Color32(30, 144, byte.MaxValue, byte.MaxValue);

        public static Color32 DarkDodgerBlue = new Color32(8, 90, 177, byte.MaxValue);

        public static Color32 DeepSkyBlue = new Color32(0, 191, byte.MaxValue, byte.MaxValue);

        public static Color32 SkyBlue = new Color32(135, 206, 235, byte.MaxValue);

        public static Color32 SteelBlue = new Color32(70, 130, 180, byte.MaxValue);

        public static Color32 Cyan = new Color32(0, byte.MaxValue, byte.MaxValue, byte.MaxValue);

        public static Color32 Yellow = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);

        public static Color32 Gold = new Color32(byte.MaxValue, 215, 0, byte.MaxValue);

        public static Color32 LightYellow = new Color32(byte.MaxValue, byte.MaxValue, 224, byte.MaxValue);

        public static Color32 LemonChiffon = new Color32(byte.MaxValue, 250, 205, byte.MaxValue);

        public static Color32 Khaki = new Color32(240, 230, 140, byte.MaxValue);

        public static Color32 PaleGoldenrod = new Color32(238, 232, 170, byte.MaxValue);

        public static Color32 LightGoldenrodYellow = new Color32(250, 250, 210, byte.MaxValue);

        public static Color32 Orange = new Color32(byte.MaxValue, 165, 0, byte.MaxValue);

        public static Color32 DarkOrange = new Color32(byte.MaxValue, 140, 0, byte.MaxValue);

        public static Color32 RedOrange = new Color32(byte.MaxValue, 69, 0, byte.MaxValue);

        public static Color32 PeachPuff = new Color32(byte.MaxValue, 218, 185, byte.MaxValue);

        public static Color32 DarkGoldenrod = new Color32(184, 134, 11, byte.MaxValue);

        public static Color32 Peru = new Color32(205, 133, 63, byte.MaxValue);

        public static Color32 OrangeRed = new Color32(byte.MaxValue, 69, 0, byte.MaxValue);

        public static Color32 Magenta = new Color32(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue);

        public static Color32 Purple = new Color32(123, 3, 252, byte.MaxValue);

        public static Color32 DarkPurple = new Color32(38, 23, 77, byte.MaxValue);

        public static Color32 Lavender = new Color32(230, 230, 250, byte.MaxValue);

        public static Color32 Plum = new Color32(221, 160, 221, byte.MaxValue);

        public static Color32 Indigo = new Color32(75, 0, 130, byte.MaxValue);

        public static Color32 MediumOrchid = new Color32(186, 85, 211, byte.MaxValue);

        public static Color32 SlateBlue = new Color32(106, 90, 205, byte.MaxValue);

        public static Color32 DarkSlateBlue = new Color32(72, 61, 139, byte.MaxValue);

        public static Color32 Pink = new Color32(byte.MaxValue, 192, 203, byte.MaxValue);

        public static Color32 LightSalmon = new Color32(byte.MaxValue, 160, 122, byte.MaxValue);

        public static Color32 DarkSalmon = new Color32(233, 150, 122, byte.MaxValue);

        public static Color32 LightCoral = new Color32(240, 128, 128, byte.MaxValue);

        public static Color32 MistyRose = new Color32(byte.MaxValue, 228, 225, byte.MaxValue);

        public static Color32 HotPink = new Color32(byte.MaxValue, 105, 180, byte.MaxValue);

        public static Color32 DeepPink = new Color32(byte.MaxValue, 20, 147, byte.MaxValue);

        public static Color32 Brown = new Color32(139, 69, 19, byte.MaxValue);

        public static Color32 RosyBrown = new Color32(188, 143, 143, byte.MaxValue);

        public static Color32 SaddleBrown = new Color32(139, 69, 19, byte.MaxValue);

        public static Color32 Sienna = new Color32(160, 82, 45, byte.MaxValue);

        public static Color32 Chocolate = new Color32(210, 105, 30, byte.MaxValue);

        public static Color32 SandyBrown = new Color32(244, 164, 96, byte.MaxValue);

        public static Color32 DarkSandyBrown = new Color32(224, 144, 76, byte.MaxValue);

        public static Color32 BurlyWood = new Color32(222, 184, 135, byte.MaxValue);

        public static Color32 Tan = new Color32(210, 180, 140, byte.MaxValue);

        public static Color32 White = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

        public static Color32 Linen = new Color32(250, 240, 230, byte.MaxValue);

        public static Color32 OldLace = new Color32(253, 245, 230, byte.MaxValue);

        public static Color32 SeaShell = new Color32(byte.MaxValue, 245, 238, byte.MaxValue);

        public static Color32 MintCream = new Color32(245, byte.MaxValue, 250, byte.MaxValue);

        public static Color32 Black = new Color32(0, 0, 0, byte.MaxValue);

        public static Color32 Grey = new Color32(128, 128, 128, byte.MaxValue);

        public static Color32 LightGrey = new Color32(192, 192, 192, byte.MaxValue);

        public static Color32 DarkGrey = new Color32(80, 80, 80, byte.MaxValue);

        public static Color32 DarkerGrey = new Color32(40, 40, 40, byte.MaxValue);

        public static Color32 RedTransparent = new Color32(byte.MaxValue, 0, 0, 80);

        public static Color32 DarkRedTransparent = new Color32(180, 0, 0, 80);

        public static Color32 SalmonTransparent = new Color32(250, 128, 114, 80);

        public static Color32 IndianRedTransparent = new Color32(205, 92, 92, 80);

        public static Color32 CrimsonTransparent = new Color32(220, 20, 60, 80);

        public static Color32 WineRedTransparent = new Color32(123, 0, 0, 80);

        public static Color32 FireBrickTransparent = new Color32(178, 34, 34, 80);

        public static Color32 CoralTransparent = new Color32(byte.MaxValue, 127, 80, 80);

        public static Color32 TomatoTransparent = new Color32(byte.MaxValue, 99, 71, 80);

        public static Color32 MaroonTransparent = new Color32(128, 0, 0, 80);

        public static Color32 GreenTransparent = new Color32(0, byte.MaxValue, 0, 80);

        public static Color32 LimeTransparent = new Color32(0, 128, 0, 80);

        public static Color32 DarkGreenTransparent = new Color32(0, 100, 0, 80);

        public static Color32 OliveTransparent = new Color32(128, 128, 0, 80);

        public static Color32 ForestGreenTransparent = new Color32(34, 139, 34, 80);

        public static Color32 SeaGreenTransparent = new Color32(46, 139, 87, 80);

        public static Color32 MediumSeaGreenTransparent = new Color32(60, 179, 113, 80);

        public static Color32 AquamarineTransparent = new Color32(127, byte.MaxValue, 212, 80);

        public static Color32 MediumAquamarineTransparent = new Color32(102, 205, 170, 80);

        public static Color32 DarkSeaGreenTransparent = new Color32(143, 188, 143, 80);

        public static Color32 BlueTransparent = new Color32(0, 0, byte.MaxValue, 80);

        public static Color32 NavyTransparent = new Color32(0, 0, 128, 80);

        public static Color32 DarkBlueTransparent = new Color32(0, 0, 139, 80);

        public static Color32 RoyalBlueTransparent = new Color32(65, 105, 225, 80);

        public static Color32 DodgerBlueTransparent = new Color32(30, 144, byte.MaxValue, 80);

        public static Color32 DarkDodgerBlueTransparent = new Color32(8, 90, 177, 80);

        public static Color32 DeepSkyBlueTransparent = new Color32(0, 191, byte.MaxValue, 80);

        public static Color32 SkyBlueTransparent = new Color32(135, 206, 235, 80);

        public static Color32 SteelBlueTransparent = new Color32(70, 130, 180, 80);

        public static Color32 CyanTransparent = new Color32(0, byte.MaxValue, byte.MaxValue, 80);

        public static Color32 YellowTransparent = new Color32(byte.MaxValue, byte.MaxValue, 0, 80);

        public static Color32 GoldTransparent = new Color32(byte.MaxValue, 215, 0, 80);

        public static Color32 LightYellowTransparent = new Color32(byte.MaxValue, byte.MaxValue, 224, 80);

        public static Color32 LemonChiffonTransparent = new Color32(byte.MaxValue, 250, 205, 80);

        public static Color32 KhakiTransparent = new Color32(240, 230, 140, 80);

        public static Color32 PaleGoldenrodTransparent = new Color32(238, 232, 170, 80);

        public static Color32 LightGoldenrodYellowTransparent = new Color32(250, 250, 210, 80);

        public static Color32 OrangeTransparent = new Color32(byte.MaxValue, 165, 0, 80);

        public static Color32 DarkOrangeTransparent = new Color32(byte.MaxValue, 140, 0, 80);

        public static Color32 RedOrangeTransparent = new Color32(byte.MaxValue, 69, 0, 80);

        public static Color32 PeachPuffTransparent = new Color32(byte.MaxValue, 218, 185, 80);

        public static Color32 DarkGoldenrodTransparent = new Color32(184, 134, 11, 80);

        public static Color32 PeruTransparent = new Color32(205, 133, 63, 80);

        public static Color32 OrangeRedTransparent = new Color32(byte.MaxValue, 69, 0, 80);

        public static Color32 MagentaTransparent = new Color32(byte.MaxValue, 0, byte.MaxValue, 80);

        public static Color32 PurpleTransparent = new Color32(123, 3, 252, 80);

        public static Color32 LavenderTransparent = new Color32(230, 230, 250, 80);

        public static Color32 PlumTransparent = new Color32(221, 160, 221, 80);

        public static Color32 IndigoTransparent = new Color32(75, 0, 130, 80);

        public static Color32 MediumOrchidTransparent = new Color32(186, 85, 211, 80);

        public static Color32 SlateBlueTransparent = new Color32(106, 90, 205, 80);

        public static Color32 DarkSlateBlueTransparent = new Color32(72, 61, 139, 80);

        public static Color32 PinkTransparent = new Color32(byte.MaxValue, 192, 203, 80);

        public static Color32 LightSalmonTransparent = new Color32(byte.MaxValue, 160, 122, 80);

        public static Color32 DarkSalmonTransparent = new Color32(233, 150, 122, 80);

        public static Color32 LightCoralTransparent = new Color32(240, 128, 128, 80);

        public static Color32 MistyRoseTransparent = new Color32(byte.MaxValue, 228, 225, 80);

        public static Color32 HotPinkTransparent = new Color32(byte.MaxValue, 105, 180, 80);

        public static Color32 DeepPinkTransparent = new Color32(byte.MaxValue, 20, 147, 80);

        public static Color32 BrownTransparent = new Color32(165, 42, 42, 80);

        public static Color32 RosyBrownTransparent = new Color32(188, 143, 143, 80);

        public static Color32 SaddleBrownTransparent = new Color32(139, 69, 19, 80);

        public static Color32 SiennaTransparent = new Color32(160, 82, 45, 80);

        public static Color32 ChocolateTransparent = new Color32(210, 105, 30, 80);

        public static Color32 SandyBrownTransparent = new Color32(244, 164, 96, 80);

        public static Color32 BurlyWoodTransparent = new Color32(222, 184, 135, 80);

        public static Color32 TanTransparent = new Color32(210, 180, 140, 80);

        public static Color32 WhiteTransparent = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 80);

        public static Color32 LightWhiteTransparent = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 10);

        public static Color32 LinenTransparent = new Color32(250, 240, 230, 80);

        public static Color32 kiwi = new Color32(101, 158, 105, 255);

        public static Color32 kiwidark = new Color32(72, 112, 75, 255);

        public static Color32 OldLaceTransparent = new Color32(253, 245, 230, 80);

        public static Color32 SeaShellTransparent = new Color32(byte.MaxValue, 245, 238, 80);
      
        public static Color32 MintCreamTransparent = new Color32(245, byte.MaxValue, 250, 80);

        public static Color32 BlackTransparent = new Color32(0, 0, 0, 80);
        
        public static Color32 GreyTransparent = new Color32(80, 80, 80, 80);

        public static Color32 LightGreyTransparent = new Color32(192, 192, 192, 80);

        public static Color32 DarkGreyTransparent = new Color32(40, 40, 40, 80);
      
        public static Color32 DarkerGreyTransparent = new Color32(40, 40, 40, 80);
      
        public static Shader guiShader = Shader.Find("GUI/Text Shader");
     
        public static Shader uberShader = Shader.Find("GorillaTag/UberShader");
     
        public static Shader uiShader = Shader.Find("UI/Default");

        public static Material RGB = new Material(ColorLib.uberShader);

        public static Material DFade = new Material(ColorLib.uberShader);

        public static Material DBreath = new Material(ColorLib.uberShader);

        public static Material BlueFade = new Material(ColorLib.uberShader);

        public static string hexColor = "#" + ColorUtility.ToHtmlStringRGB(ColorLib.RGB.color);
    
        public static string hexColor1 = "#" + ColorUtility.ToHtmlStringRGB(ColorLib.DFade.color);
    }
}