using HarmonyLib;
using UnityEngine;
using Verse;

namespace BunkBeds
{
    public class BunkBedsMod : Mod
    {
        public static BunkBedsSettings settings;
        public BunkBedsMod(ModContentPack pack) : base(pack)
        {
            settings = GetSettings<BunkBedsSettings>();
            new Harmony("BunkBedsMod").PatchAll();
        }
        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            settings.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return Content.Name;
        }
    }

}
