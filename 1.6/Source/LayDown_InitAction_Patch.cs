using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace BunkBeds
{
    using HarmonyLib;
    using RimWorld;
    using System.Collections.Generic;
    using Verse;

    [HarmonyPatch]
    public static class LayDown_InitAction_Patch
    {
        public static MethodBase TargetMethod()
        {
            foreach (var nested in typeof(Toils_LayDown).GetNestedTypes(AccessTools.all))
            {
                var method = nested.GetMethods(AccessTools.all).Where(x => x.Name.Contains("<LayDown>b__0")).FirstOrDefault();
                if (method != null)
                {
                    return method;
                }
            }
            return null;
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var cellRectContains = AccessTools.Method(typeof(CellRect), nameof(CellRect.Contains), new Type[] { typeof(IntVec3) });
            var codes = codeInstructions.ToList();
            for (var i = 0; i < codes.Count; i++)
            {
                var code = codes[i];
                yield return code;
                if (code.opcode == OpCodes.Brtrue_S && codes[i - 1].Calls(cellRectContains))
                {
                    yield return new CodeInstruction(OpCodes.Ldloc_1);
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(LayDown_InitAction_Patch), nameof(IsNotBunkBed)));
                    yield return new CodeInstruction(OpCodes.Brfalse_S, code.operand);
                }
            }
        }

        public static bool IsNotBunkBed(Building_Bed bed, Pawn pawn)
        {
            if (bed.IsBunkBed())
            {
                pawn.Position = bed.Position;
                return false;
            }
            return true;
        }
    }
}
