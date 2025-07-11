using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using Verse.AI;

namespace BunkBeds
{
    [HarmonyPatch]
    public static class Toils_Bed_GotoBed_TickAction_Patch
    {
        public static MethodBase TargetMethod()
        {
            var closureType = typeof(Toils_Bed).GetNestedType("<>c__DisplayClass0_0", BindingFlags.NonPublic);
            if (closureType == null)
            {
                Log.Error("[BunkBeds] Failed to find the class for Toils_Bed.GotoBed. The patch will not be applied.");
                return null;
            }
            var targetMethod = closureType.GetMethod("<GotoBed>b__1", BindingFlags.Instance | BindingFlags.NonPublic);
            if (targetMethod == null)
            {
                Log.Error("[BunkBeds] Failed to find the target method '<GotoBed>b__1'. The patch will not be applied.");
                return null;
            }
            return targetMethod;
        }

        public static bool Prefix(Toil ___gotoBed, TargetIndex ___bedIndex)
        {
            var actor = ___gotoBed.actor;
            var building_Bed = actor.CurJob.GetTarget(___bedIndex).Thing as Building_Bed;
            if (building_Bed.IsBunkBed())
            {
                var cell = RestUtility.GetBedSleepingSlotPosFor(actor, building_Bed);
                if (actor.pather.Destination.Cell != cell)
                {
                    actor.pather.StartPath(cell, PathEndMode.OnCell);
                }
                return false;
            }
            return true;
        }
    }
}
