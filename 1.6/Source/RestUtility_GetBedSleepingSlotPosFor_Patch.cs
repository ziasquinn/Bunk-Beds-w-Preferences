using HarmonyLib;
using RimWorld;
using Verse;

namespace BunkBeds
{
    [HarmonyPatch(typeof(RestUtility), "GetBedSleepingSlotPosFor")]
    public static class RestUtility_GetBedSleepingSlotPosFor_Patch
    {
        public static void Postfix(ref IntVec3 __result, Pawn pawn, Building_Bed bed)
        {
            if (bed.IsBunkBed())
            {
                __result = bed.Position;
            }
        }
    }

}
