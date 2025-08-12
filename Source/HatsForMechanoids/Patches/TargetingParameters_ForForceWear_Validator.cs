using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace HatsForMechanoids.Patches
{
    public class TargetingParameters_ForForceWear_Validator
    {
        public static void Apply(Harmony harmony)
        {
            harmony.Patch(typeof(TargetingParameters)
                    .Inner("<>c__DisplayClass42_0")
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                    .First(m => m.Name == "<ForForceWear>b__0"),
                prefix: new HarmonyMethod(typeof(TargetingParameters_ForForceWear_Validator).GetMethod(nameof(Prefix),
                    BindingFlags.Static | BindingFlags.NonPublic))
            );
        }

        static bool Prefix(ref bool __result, TargetInfo targ)
        {
            if (targ.Thing is Pawn pawn && pawn.IsColonyMech)
            {
                // pawn.story = pawn.story ?? new Pawn_StoryTracker(pawn);
                // pawn.story.bodyType = pawn.story.bodyType ?? BodyTypeDefOf.Female;
                __result = true;
                return false;
            }

            return true;
        }
    }
}