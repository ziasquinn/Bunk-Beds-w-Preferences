using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace BunkBeds
{
    [HotSwappable]
    [HarmonyPatch(typeof(Building_Bed), "GetCurOccupant")]
    public static class Building_Bed_GetCurOccupant_Patch
    {
        public static bool Prefix(Building_Bed __instance, ref Pawn __result, int slotIndex)
        {
            if (__instance.IsBunkBed())
            {
                __result = GetCurOccupant(__instance, slotIndex);
                return false;
            }
            return true;
        }

        public static Pawn GetCurOccupant(Building_Bed __instance, int slotIndex)
        {
            if (!__instance.Spawned)
            {
                return null;
            }
            IntVec3 sleepingSlotPos = __instance.Position;
            List<Thing> list = __instance.Map.thingGrid.ThingsListAt(sleepingSlotPos);
            var comp = __instance.CompAssignableToPawn;
            for (int i = 0; i < list.Count; i++)
            {
                Pawn pawn = list[i] as Pawn;
                if (pawn != null && pawn.CurJob != null && pawn.GetPosture().InBed())
                {
                    if (comp.AssignedPawnsForReading.IndexOf(pawn) == slotIndex)
                    {
                        return pawn;
                    }
                }
            }
            return null;
        }
    }

}
