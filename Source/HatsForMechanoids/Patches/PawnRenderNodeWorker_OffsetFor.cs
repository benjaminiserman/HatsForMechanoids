using HarmonyLib;
using UnityEngine;
using Verse;

namespace HatsForMechanoids.Patches
{
    [HarmonyPatch(typeof(PawnRenderNodeWorker), nameof(PawnRenderNodeWorker.OffsetFor))]
    public class PawnRenderNodeWorker_OffsetFor
    {
        public static void Postfix(PawnRenderNode node, PawnRenderNodeWorker __instance, ref Vector3 __result)
        {
            if (!(__instance is PawnRenderNodeWorker_Apparel_Head) || !node.tree.pawn.IsColonyMech)
            {
                return;
            }

            var def = DefDatabase<HatWearerDef>.GetNamed(node.tree?.pawn?.def?.defName ?? "", errorOnFail: false);
            if (def == null)
            {
                return;
            }

            __result.x += def.xOffset;
            __result.z += def.zOffset;

            var rotation = node.tree?.pawn?.Rotation;
            if (rotation == null)
            {
                return;
            }

            switch (rotation.Value.AsInt)
            {
                case Rot4.NorthInt:
                {
                    __result.x += def.north.xOffset;
                    __result.z += def.north.zOffset;
                    break;
                }
                case Rot4.EastInt:
                {
                    __result.x += def.east.xOffset;
                    __result.z += def.east.zOffset;
                    break;
                }
                case Rot4.SouthInt:
                {
                    __result.x += def.south.xOffset;
                    __result.z += def.south.zOffset;
                    break;
                }
                case Rot4.WestInt:
                {
                    __result.x += def.west.xOffset;
                    __result.z += def.west.zOffset;
                    break;
                }
            }
        }
    }
}