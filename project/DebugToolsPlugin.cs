using Arys.SPTDebugTools.Patches;
using BepInEx;
using BepInEx.Configuration;

namespace Arys.SPTDebugTools
{
    [BepInPlugin("com.Arys.SPTDebugTools", "SPT-DebugTools", "1.0.0")]
    public class DebugToolsPlugin : BaseUnityPlugin
    {
        internal static ConfigEntry<KeyboardShortcut> TeleportAllBotsKeybind;

        private void Awake()
        {
            new GameWorldOnGameStartedPatch().Enable();

            TeleportAllBotsKeybind = Config.Bind("", "Teleport all bots to target location", KeyboardShortcut.Empty, "");
        }
    }
}
