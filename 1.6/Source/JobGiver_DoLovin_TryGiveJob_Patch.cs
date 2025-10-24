using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace BunkBeds
{
    [HotSwappable]
    [HarmonyPatch(typeof(JobGiver_DoLovin), "TryGiveJob")]
    public static class JobGiver_DoLovin_TryGiveJob_Patch
    {
        public static void Postfix(ref Job __result, Pawn pawn)
        {
            if (__result != null && pawn.CurrentBed().IsBunkBed()) 
            {
                __result = null;
            }
        }
    }

}
