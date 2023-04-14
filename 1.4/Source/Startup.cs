using HarmonyLib;
using RimWorld;
using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace BunkBeds
{
    [StaticConstructorOnStartup]
    public static class Utils
    {
        public static bool IsBunkBed(this ThingWithComps bed)
        {
            return bed != null && CompBunkBed.bunkBeds.Contains(bed);
        }

        public static bool IsBunkBed(this ThingWithComps bed, out CompBunkBed comp)
        {
            comp = bed?.GetComp<CompBunkBed>();
            return comp != null;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class HotSwappableAttribute : Attribute
    {
    }
}
