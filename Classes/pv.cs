using PanckeUtilityPad.LegalMods;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PCGUI : MonoBehaviour
{
    public static bool showPCGUI = false;

    // Toggles for each mod
    public static bool speedboostRGToggle = false;
    public static bool longarmsLGTog = false;
    public static bool longarmsTog = false;
    public static bool speedboostTog = false;
    public static bool platformsTog = false;
    public static bool legitWallWalkTog = false;
    public static bool wallWalkTog = false;
    public static bool wallAssistTog = false;

    private Rect windowRect = new Rect(20, 20, 300, 420);

    void Update()
    {
        // Only allow GUI on PC, not VR
        if (!XRSettings.isDeviceActive)
        {
            if (Keyboard.current.oKey.wasPressedThisFrame)
                showPCGUI = !showPCGUI;
        }
    }

    void OnGUI()
    {
        if (!showPCGUI || XRSettings.isDeviceActive)
            return;

        windowRect = GUI.Window(0, windowRect, DrawWindow, "PC Mod GUI");
    }

    void DrawWindow(int id)
    {
        GUILayout.Space(10);

        speedboostRGToggle = GUILayout.Toggle(speedboostRGToggle, "Speedboost RG");
        if (speedboostRGToggle) ModFunction.speedboostRG();

        longarmsLGTog = GUILayout.Toggle(longarmsLGTog, "Long Arms LG");
        if (longarmsLGTog) ModFunction.longarmsLG();

        longarmsTog = GUILayout.Toggle(longarmsTog, "Long Arms");
        if (longarmsTog) ModFunction.longarms();

        speedboostTog = GUILayout.Toggle(speedboostTog, "Speedboost");
        if (speedboostTog) ModFunction.speedboost();

        platformsTog = GUILayout.Toggle(platformsTog, "Platforms");
        if (platformsTog) ModFunction.Platforms();

        legitWallWalkTog = GUILayout.Toggle(legitWallWalkTog, "Legitimate Wall Walk");
        if (legitWallWalkTog) ModFunction.LegitimateWallWalk();

        wallWalkTog = GUILayout.Toggle(wallWalkTog, "Wall Walk");
        if (wallWalkTog) ModFunction.WallWalk();

        wallAssistTog = GUILayout.Toggle(wallAssistTog, "Wall Assist");
        if (wallAssistTog) ModFunction.WallAssist();

        GUI.DragWindow();
    }
}
