using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace HatsForMechanoids.Patches
{
    public class PawnRenderNode_Apparel_GraphicsFor
    {
        public static void Apply(Harmony harmony)
        {
            harmony.Patch(typeof(PawnRenderNode_Apparel)
                    .Inner("<GraphicsFor>d__5")
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                    .First(m => m.Name == "MoveNext"),
                transpiler: new HarmonyMethod(typeof(PawnRenderNode_Apparel_GraphicsFor).GetMethod(
                    nameof(Transpiler),
                    BindingFlags.Static | BindingFlags.NonPublic))
            );
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions,
            ILGenerator ilGenerator)
        {
            var found = false;
            var bodyTypeField = typeof(Pawn_StoryTracker).GetField(nameof(Pawn_StoryTracker.bodyType));

            var label = ilGenerator.DefineLabel();
            var label2 = ilGenerator.DefineLabel();

            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldfld && (FieldInfo)instruction.operand == bodyTypeField)
                {
                    found = true;
                    yield return new CodeInstruction(OpCodes.Dup);
                    yield return new CodeInstruction(OpCodes.Brtrue_S, label);
                    yield return new CodeInstruction(OpCodes.Pop);
                    yield return new CodeInstruction(OpCodes.Ldnull);
                    yield return new CodeInstruction(OpCodes.Br_S, label2);
                    yield return new CodeInstruction(instruction.opcode, instruction.operand)
                        { labels = new List<Label> { label } };
                    yield return new CodeInstruction(OpCodes.Nop)
                        { labels = new List<Label> { label2 } };
                }
                else
                {
                    yield return instruction;
                }
            }

            if (!found)
            {
                Log.Error(
                    $"Failed to find FieldInfo {bodyTypeField} in patch {nameof(PawnRenderNode_Apparel_GraphicsFor)}");
            }
        }
    }
}