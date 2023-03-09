using HarmonyLib;
using RimWorld;

namespace BunkBeds
{
    [HarmonyPatch(typeof(CompAssignableToPawn_Bed), "IdeoligionForbids")]
    public static class CompAssignableToPawn_Bed_IdeoligionForbids_Patch
    {
        public static bool Prefix(CompAssignableToPawn_Bed __instance, ref bool __result)
        {
            if (__instance.parent.IsBunkBed())
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
