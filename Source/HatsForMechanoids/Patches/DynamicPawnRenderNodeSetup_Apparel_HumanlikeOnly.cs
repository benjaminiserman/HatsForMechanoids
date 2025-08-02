using HarmonyLib;
using Verse;

namespace HatsForMechanoids.Patches
{
    [HarmonyPatch(typeof(DynamicPawnRenderNodeSetup_Apparel), "get_HumanlikeOnly")]
    public class DynamicPawnRenderNodeSetup_Apparel_HumanlikeOnly
    {
        public static bool Prefix(ref bool __result)
        {
            __result = false;
            return false;
        }
    }
}