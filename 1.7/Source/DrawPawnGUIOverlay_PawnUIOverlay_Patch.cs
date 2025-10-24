using HarmonyLib;
using RimWorld;
using Verse;

namespace BunkBeds
{
    [HarmonyPatch(typeof(PawnUIOverlay), "DrawPawnGUIOverlay")]
    public static class DrawPawnGUIOverlay_DrawPawnGUIOverlay_Patch
    {
        public static bool Prefix(PawnUIOverlay __instance)
        {
            if (__instance.pawn.CurrentBed().IsBunkBed())
            {
                return false;
            }
            return true;
        }
    }
}
