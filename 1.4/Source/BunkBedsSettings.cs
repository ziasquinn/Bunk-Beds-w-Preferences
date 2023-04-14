using UnityEngine;
using Verse;

namespace BunkBeds
{
    public class BunkBedsSettings : ModSettings
    {
        public static bool bunkBedsCountAsBarracks = true;
        public static bool occupantsWillHaveShareBedDebuff = true;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref bunkBedsCountAsBarracks, "bunkBedsCountAsBarracks", true);
            Scribe_Values.Look(ref occupantsWillHaveShareBedDebuff, "occupantsWillHaveShareBedDebuff", true);
        }

        public void DoSettingsWindowContents(Rect inRect)
        {
            var ls = new Listing_Standard();
            ls.Begin(inRect);
            ls.CheckboxLabeled("BB.BunkBedsCountAsBarracks".Translate(), ref bunkBedsCountAsBarracks);
            ls.CheckboxLabeled("BB.OccupantsWillHaveShareBedDebuff".Translate(), ref occupantsWillHaveShareBedDebuff);
            ls.End();
        }
    }

}
