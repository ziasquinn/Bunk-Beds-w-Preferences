using HarmonyLib;
using RimWorld;
using Verse;

namespace BunkBeds
{
    [HarmonyPatch(typeof(CompProperties_AssignableToPawn), "PostLoadSpecial")]
    public static class CompProperties_AssignableToPawn_PostLoadSpecial_Patch
    {
        public static void Postfix(CompProperties_AssignableToPawn __instance, ThingDef parent)
        {
            var compProps = parent.GetCompProperties<CompProperties_BunkBed>();
            if (compProps != null)
            {
                __instance.maxAssignedPawnsCount = compProps.pawnCount;
            }
        }
    }


}
