using HarmonyLib;
using RimWorld;
using Verse;
using System.Reflection;

namespace BunkBeds
{
    [HarmonyPatch]
    public static class BedUtility_OtherOwnerScore_Patch
    {
        public static bool Prepare() => ModsConfig.IsActive("Orion.Hospitality") && TargetMethod() != null;

        static MethodBase TargetMethod()
        {
            var bedUtilityType = AccessTools.TypeByName("Hospitality.Utilities.BedUtility");
            if (bedUtilityType != null)
            {
                var methodToPatch = AccessTools.Method(bedUtilityType, "OtherOwnerScore", new[] { typeof(Building_Bed), typeof(Pawn) });
                if (methodToPatch != null)
                {
                    return methodToPatch;
                }
            }

            return null;
        }

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
