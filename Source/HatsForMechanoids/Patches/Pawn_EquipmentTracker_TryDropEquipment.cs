using HarmonyLib;
using RimWorld;
using Verse;

namespace HatsForMechanoids.Patches
{
    [HarmonyPatch(typeof(Pawn_EquipmentTracker), nameof(Pawn_EquipmentTracker.TryDropEquipment))]
    public class Pawn_EquipmentTracker_TryDropEquipment
    {
        public static bool Prefix(ref bool __result, Pawn_EquipmentTracker __instance, ThingWithComps eq,
            out ThingWithComps resultingEq)
        {
            resultingEq = null;
            if (__instance.pawn.IsColonyMech && !(eq is Apparel))
            {
                return false;
            }

            return true;
        }
    }
}