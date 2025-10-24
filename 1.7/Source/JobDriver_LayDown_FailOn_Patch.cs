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
            // Apply a thought based on bunk slot (higher bunks get a small positive mood)
            try
            {
                if (__instance.Bed != null && __instance.Bed.IsBunkBed())
                {
                    var compAssignable = __instance.Bed.CompAssignableToPawn;
                    if (compAssignable != null)
                    {
                        int slotIndex = compAssignable.AssignedPawnsForReading.IndexOf(__instance.pawn);
                        ThoughtDef thought = null;
                        if (slotIndex > 0)
                        {
                            thought = DefDatabase<ThoughtDef>.GetNamedSilentFail("BB_SleptHigherBunk");
                        }
                        else
                        {
                            thought = DefDatabase<ThoughtDef>.GetNamedSilentFail("BB_SleptLowerBunk");
                        }
                        if (thought != null && __instance.pawn.needs?.mood != null)
                        {
                            __instance.pawn.needs.mood.thoughts.memories.TryGainMemory(thought);
                            // Debug log so modders can verify which thought was applied and which slot index was used
                            try
                            {
                                Log.Message($"[BunkBeds] {__instance.pawn.LabelShort} gained thought {thought.defName} (slot {slotIndex})");
                            }
                            catch
                            {
                                // ignore logging failures
                            }
                        }
                    }
                }
            }
            catch
            {
                // Swallow any errors to avoid breaking jobs; this is a best-effort mood tweak.
            }
        }
    }
}
