using Aki.Reflection.Patching;
using EFT;
using HarmonyLib;
using System.Reflection;

namespace Arys.DebugTools
{
    public class GameWorldPatch : ModulePatch
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
