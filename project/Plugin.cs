using BepInEx;
using BepInEx.Configuration;

namespace Arys.DebugTools
{
    [BepInPlugin("com.Arys.DebugTools", "Arys.DebugTools", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ConfigEntry<KeyboardShortcut> TeleportAllBotsKeybind;

        private void Awake()
        {
            new GameWorldPatch().Enable();

            TeleportAllBotsKeybind = Config.Bind("", "Teleport all bots to target location", KeyboardShortcut.Empty, "");
        }
    }
}
