using HarmonyLib;
using RimWorld;
using Verse;
using System.Reflection;

namespace BunkBeds
{
    [HarmonyPatch]
    public static class BedUtility_OtherOwnerScore_Patch
    {
        // Target the method dynamically by name, allowing the patch to only apply if the method exists
        static MethodBase TargetMethod()
        {
            // Attempt to find the method in the mod's BedUtility class
            var bedUtilityType = AccessTools.TypeByName("Hospitality.Utilities.BedUtility");
            if (bedUtilityType != null)
            {
                var methodToPatch = AccessTools.Method(bedUtilityType, "OtherOwnerScore", new[] { typeof(Building_Bed), typeof(Pawn) });
                if (methodToPatch != null)
                {
                    return methodToPatch;
                }
            }

            Log.Warning("Hospitality.Utilities.BedUtility.OtherOwnerScore method not found. Patch not applied.");
            return null;  // Return null to prevent patching if the method is not found
        }

        // This prefix will only run if the TargetMethod finds the method
        public static bool Prefix(Building_Bed bed, Pawn guest, ref int __result)
        {
            // Check if the bed is a bunk bed
            if (bed.IsBunkBed())
            {
                __result = 0;  // Set result to 0 or any other default value
                return false;  // Skip the original method
            }

            // Allow original method to run if not a bunk bed
            return true;
        }
    }

}
