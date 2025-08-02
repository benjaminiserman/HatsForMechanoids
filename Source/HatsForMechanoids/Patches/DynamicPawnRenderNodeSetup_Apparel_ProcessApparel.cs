using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace HatsForMechanoids.Patches
{
    public class DynamicPawnRenderNodeSetup_Apparel_ProcessApparel
    {
        public static void Apply(Harmony harmony)
        {
            harmony.Patch(typeof(DynamicPawnRenderNodeSetup_Apparel)
                    .Inner("<ProcessApparel>d__5")
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                    .First(m => m.Name == "MoveNext"),
                transpiler: new HarmonyMethod(typeof(DynamicPawnRenderNodeSetup_Apparel_ProcessApparel).GetMethod(
                    nameof(Transpiler),
                    BindingFlags.Static | BindingFlags.NonPublic))
            );
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var found = false;
            var pawnTypeField = typeof(PawnRenderNodeProperties).GetField(nameof(PawnRenderNodeProperties.pawnType));

            foreach (var instruction in instructions)
            {
                yield return instruction;

                if (!found
                    && instruction.opcode == OpCodes.Newobj
                    && (ConstructorInfo)instruction.operand == typeof(PawnRenderNodeProperties).GetConstructors()[0])
                {
                    found = true;
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Stfld, pawnTypeField);
                }
            }

            if (!found)
            {
                Log.Error(
                    $"Failed to find FieldInfo {pawnTypeField} in patch {nameof(DynamicPawnRenderNodeSetup_Apparel_ProcessApparel)}");
            }
        }
    }
}