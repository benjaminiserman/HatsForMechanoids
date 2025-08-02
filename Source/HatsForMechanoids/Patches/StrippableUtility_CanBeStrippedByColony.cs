using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace HatsForMechanoids.Patches
{
    [HarmonyPatch(typeof(StrippableUtility), nameof(StrippableUtility.CanBeStrippedByColony))]
    public class StrippableUtility_CanBeStrippedByColony
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions,
            ILGenerator ilGenerator)
        {
            var instructionsList = new List<CodeInstruction>(instructions);
            var isColonyMech = typeof(Pawn).GetProperty(nameof(Pawn.IsColonyMech))?.GetGetMethod();

            if (isColonyMech == null)
            {
                Log.Error(
                    $"Failed to find Pawn.IsColonyMech in patch {nameof(StrippableUtility_CanBeStrippedByColony)}");
            }

            var lastReturnIndex = instructionsList.FindLastIndex(instruction => instruction.opcode == OpCodes.Ret);

            var label = ilGenerator.DefineLabel();

            instructionsList.InsertRange(lastReturnIndex - 1, new[]
            {
                new CodeInstruction(OpCodes.Ldloc_1),
                new CodeInstruction(OpCodes.Callvirt, isColonyMech),
                new CodeInstruction(OpCodes.Brfalse_S, label),
                new CodeInstruction(OpCodes.Ldc_I4_1),
                new CodeInstruction(OpCodes.Ret),
                new CodeInstruction(OpCodes.Nop) { labels = new List<Label> { label } }
            });

            foreach (var instruction in instructionsList)
            {
                yield return instruction;
            }
        }
    }
}