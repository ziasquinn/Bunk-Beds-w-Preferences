using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace BunkBeds
{
    [HarmonyPatch(typeof(RoomRoleWorker_Bedroom), "IsBedroomHelper")]
    public static class RoomRoleWorker_Bedroom_IsBedroomHelper_Patch
    {
        public static bool Prefix(List<Building_Bed> beds)
        {
            if (BunkBedsSettings.bunkBedsCountAsBarracks)
            {
                if (beds.Any(x => x.IsBunkBed()))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
