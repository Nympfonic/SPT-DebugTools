using SPT.Reflection.Patching;
using EFT;
using HarmonyLib;
using System.Reflection;

namespace Arys.SPTDebugTools.Patches
{
    public class GameWorldOnGameStartedPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(GameWorld), nameof(GameWorld.OnGameStarted));
        }

        [PatchPostfix]
        public static void PatchPostfix(GameWorld __instance)
        {
            __instance.gameObject.AddComponent<DebugToolsManager>();
        }
    }
}
