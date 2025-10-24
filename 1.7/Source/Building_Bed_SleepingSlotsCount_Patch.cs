using HarmonyLib;
using RimWorld;

namespace BunkBeds
{
    [HarmonyPatch(typeof(Building_Bed), "SleepingSlotsCount", MethodType.Getter)]
    public static class Building_Bed_SleepingSlotsCount_Patch
    {
        public static bool Prefix(Building_Bed __instance, ref int __result)
        {
            if (__instance.IsBunkBed(out var bunkBed)) 
            {
                __result = bunkBed.Props.pawnCount;
                return false;
            }
            return true;
        }
    }
}
