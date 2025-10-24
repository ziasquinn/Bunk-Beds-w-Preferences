using HarmonyLib;
using RimWorld;
using System.Linq;
using System.Reflection;
using Verse;

namespace BunkBeds
{
    [HarmonyPatch]
    public static class JobDriver_LayDown_FailOn_Patch
    {
        public static MethodBase TargetMethod()
        {
            return typeof(JobDriver_LayDown).GetMethods(AccessTools.all).FirstOrDefault(x => x.Name.Contains("<MakeNewToils>b__16_0"));
        }

        public static void Prefix(JobDriver_LayDown __instance, out IntVec3 __state)
        {
            __state = __instance.pawn.Position;
            if (__instance.Bed.IsBunkBed())
            {
                __instance.pawn.Position = __instance.Bed.Position;
            }
        }

        public static void Postfix(JobDriver_LayDown __instance, IntVec3 __state)
        {
            __instance.pawn.Position = __state;
        }
    }
}
