using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace HatsForMechanoids.Patches
{
    [HarmonyPatch(typeof(TargetingParameters), nameof(TargetingParameters.ForForceWear))]
    public class TargetingParameters_ForForceWear
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Log.Message("Transpiler start");
            var canTargetMechs = typeof(TargetingParameters).GetField(nameof(TargetingParameters.canTargetMechs));
            var found = false;

            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Stfld && (FieldInfo)instruction.operand == canTargetMechs)
                {
                    yield return new CodeInstruction(OpCodes.Pop);
                    yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                    found = true;
                    Log.Message("found");
                }

                yield return instruction;
            }

            if (!found)
            {
                Log.Error($"Failed to find FieldInfo {canTargetMechs} in patch {nameof(TargetingParameters_ForForceWear)}");
            }
        }
    }
}