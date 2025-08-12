using HarmonyLib;
using RimWorld;

namespace HatsForMechanoids.Patches
{
    [HarmonyPatch(typeof(ApparelGraphicRecordGetter), nameof(ApparelGraphicRecordGetter.TryGetGraphicApparel))]
    public class ApparelGraphicRecordGetter_TryGetGraphicApparel
    {
        public static void Prefix(ref BodyTypeDef bodyType)
        {
            bodyType = bodyType ?? BodyTypeDefOf.Female;
        }
    }
}