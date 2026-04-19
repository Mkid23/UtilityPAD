using BepInEx;
using System.ComponentModel;

namespace TvvPancke.Patches
{
    [Description(TvvPancke.Mods.PluginInfo.Description)]
    [BepInPlugin(TvvPancke.Mods.PluginInfo.GUID, TvvPancke.Mods.PluginInfo.Name, TvvPancke.Mods.PluginInfo.Version)]
    public class HarmonyPatches : BaseUnityPlugin
    {
        private void OnEnable()
        {
            Menu.ApplyHarmonyPatches();
        }

        private void OnDisable()
        {
            Menu.RemoveHarmonyPatches();
        }
    }
}
