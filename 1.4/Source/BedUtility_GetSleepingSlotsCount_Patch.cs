using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;

namespace BunkBeds
{
    [HarmonyPatch(typeof(BedUtility), "GetSleepingSlotsCount")]
    public static class BedUtility_GetSleepingSlotsCount_Patch
    {
        public static CompBunkBed compBunkBed;
        public static bool Prefix(ref int __result)
        {
            if (compBunkBed != null)
            {
                __result = compBunkBed.Props.pawnCount;
                return false;
            }
            return true;
        } 
    }

}
