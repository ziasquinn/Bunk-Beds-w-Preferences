using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace BunkBeds
{
    [HotSwappable]
    [HarmonyPatch(typeof(PawnRenderer), "GetBodyPos")]
    public static class PawnRenderer_GetBodyPos_Patch
    {
        public static void Postfix(PawnRenderer __instance, ref Vector3 __result)
        {
            if (__instance.pawn.CurrentBed(out var slotInd).IsBunkBed(out var bunkBed) && slotInd.HasValue)
            {
                __result.y = bunkBed.parent.DrawPos.y + slotInd.Value;
                __result = bunkBed.GetDrawOffsetForPawns(slotInd.Value, __result);
            }
        }
    }
}
