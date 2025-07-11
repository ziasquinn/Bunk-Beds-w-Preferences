using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;

namespace BunkBeds
{
    [HotSwappable]
    [HarmonyPatch]
    public static class Building_Bed_DrawGUIOverlay_Patch
    {
        public static Type guestBedType;
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(Building_Bed), nameof(Building_Bed.DrawGUIOverlay));
            guestBedType = AccessTools.TypeByName("Hospitality.Building_GuestBed");
            if (guestBedType != null)
            {
                var guestMethod = AccessTools.Method(guestBedType, "DrawGUIOverlay");
                if (guestMethod != null)
                {
                    yield return guestMethod;
                }
            }
        }

        public static bool Prefix(Building_Bed __instance)
        {
            if (__instance.IsBunkBed(out var comp))
            {
                comp.DrawGUIOverlay();
                if (guestBedType != null && guestBedType.IsAssignableFrom(__instance.def.thingClass))
                {
                    if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
                    {
                        var defaultThingLabelColor = GenMapUI.DefaultThingLabelColor;
                        var owners = __instance.CompAssignableToPawn.AssignedPawnsForReading.Where(x => x != null);
                        if (!owners.Any())
                        {
                            var rentalFee = (int)Traverse.Create(__instance).Field("rentalFee").GetValue();
                            GenMapUI.DrawThingLabel(__instance, ((float)rentalFee).ToStringMoney(), defaultThingLabelColor);
                        }
                    }
                }
                return false;
            }
            return true;
        }
    }
}
