using BepInEx;
using HarmonyLib;

namespace LH_SVHealthAlert
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class LH_SVHealthAlert : BaseUnityPlugin
    {
        public const string pluginGuid = "LH_SVHealthAlert";
        public const string pluginName = "LH_SVHealthAlert";
        public const string pluginVersion = "0.0.1";

        static bool HullWarningPlayed = false;
        static bool ShieldWarningPlayed = false;

        public void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(LH_SVHealthAlert));
        }

        [HarmonyPatch(typeof(SpaceShip), nameof(SpaceShip.Apply_Damage))]
        [HarmonyPostfix]
        static void Postfix(SpaceShip __instance, PlayerControl ___pc)
        {
            if (___pc == null)
                return;

            if (__instance.currHP >= __instance.baseHP / 2 && HullWarningPlayed)
                HullWarningPlayed = false;
            else if (__instance.currHP < __instance.baseHP / 2 && !HullWarningPlayed)
            {
                InfoPanelControl.inst.ShowWarning("Hull Low", 7, true);
                HullWarningPlayed = true;
            }

            if (__instance.stats.currShield >= __instance.stats.baseShield * __instance.energyMmt.valueMod(1) / 2 && ShieldWarningPlayed)
                ShieldWarningPlayed = false;
            else if (__instance.stats.currShield < __instance.stats.baseShield * __instance.energyMmt.valueMod(1) / 2 && !ShieldWarningPlayed)
            {
                InfoPanelControl.inst.ShowWarning("Shield Low", 7, true);
                ShieldWarningPlayed = true;
            }
        }
    }
}



