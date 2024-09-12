using HarmonyLib;
using RimWorld;

namespace BunkBeds
{
    [HarmonyPatch(typeof(Building_Bed), "GetSleepingSlotPos")]
    public static class Building_Bed_GetSleepingSlotPos_Patch
    {
        public static void Prefix(Building_Bed __instance)
        {
            BedUtility_GetSleepingSlotsCount_Patch.compBunkBed = __instance.GetComp<CompBunkBed>();
        }

        public static void Postfix()
        {
            BedUtility_GetSleepingSlotsCount_Patch.compBunkBed = null;
        }
    }

}
