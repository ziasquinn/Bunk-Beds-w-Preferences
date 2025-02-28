using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace BunkBeds
{
    [HotSwappable]
    public class CompProperties_BunkBed : CompProperties
    {
        public List<GraphicData> bedTopGraphicDatas;

        public int pawnCount;
        public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
        {
            for (var i = 1; i < pawnCount; i++)
            {
                var drawPos = CompBunkBed.GetDrawOffsetForBunkBeds(rot, i, center.ToVector3ShiftedWithAltitude(drawAltitude));
                if (rot == Rot4.East)
                {
                    drawPos.x += 0.5f;
                }
                else if (rot == Rot4.West)
                {
                    drawPos.x -= 0.5f;
                }
                else if (rot == Rot4.North)
                {
                    drawPos.z += 0.5f;
                }
                else if (rot == Rot4.South)
                {
                    drawPos.z -= 0.5f;
                }
                GhostUtility.GhostGraphicFor(bedTopGraphicDatas[i - 1].Graphic, thingDef, ghostCol).DrawFromDef(drawPos, rot, thingDef);
            }
        }
        public CompProperties_BunkBed()
        {
            this.compClass = typeof(CompBunkBed);
        }
    }

    [HotSwappable]
    public class CompBunkBed : ThingComp
    {
        public static HashSet<ThingWithComps> bunkBeds = new HashSet<ThingWithComps>();
        public CompProperties_BunkBed Props => props as CompProperties_BunkBed;
        public int BunkBedLevel => Props.pawnCount - 1;

        public List<Graphic> topGraphics;
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            bunkBeds.Add(this.parent);
        }
        public override void PostDraw()
        {
            base.PostDraw();
            bool needsRebuild = false;
            if (topGraphics is null)
            {
                needsRebuild = true;
            }
            else
            {
                for (var i = 0; i < topGraphics.Count; i++)
                {
                    if (topGraphics[i].color != this.parent.DrawColor 
                        || topGraphics[i].colorTwo != this.parent.DrawColorTwo)
                    {
                        needsRebuild = true;
                        break;
                    }
                }
            }

            if (needsRebuild)
            {
                topGraphics = new List<Graphic>();
                foreach (var graphicData in Props.bedTopGraphicDatas)
                {
                    topGraphics.Add(graphicData.GraphicColoredFor(this.parent));
                }
            }

            for (var i = 1; i < BunkBedLevel + 1; i++)
            {
                var drawPos = GetDrawOffsetForBunkBeds(this.parent.Rotation, i, this.parent.DrawPos);
                topGraphics[i - 1].Draw(drawPos, parent.Rotation, parent);
            }
        }

        public static Vector3 GetDrawOffsetForBunkBeds(Rot4 rotation, int bunkLevel, Vector3 drawPos)
        {
            drawPos.y += 1 + bunkLevel;
            if (rotation == Rot4.South || rotation == Rot4.North)
            {
                switch (bunkLevel)
                {
                    case 1:
                        {
                            drawPos += new Vector3(0, 0, 0.0f);
                            break;
                        }
                    case 2:
                        {
                            drawPos += new Vector3(0, 0, 0.75f);
                            break;
                        }
                }
            }
            else
            {
                switch (bunkLevel)
                {
                    case 1:
                        {
                            drawPos += new Vector3(0, 0, 0.25f);
                            break;
                        }
                    case 2:
                        {
                            drawPos += new Vector3(0, 0, 1);
                            break;
                        }
                }
            }
            return drawPos;
        }

        public Vector3 GetDrawOffsetForPawns(int bunkLevel, Vector3 drawPos)
        {
            drawPos.y += 0.5f + bunkLevel;
            if (parent.Rotation == Rot4.South)
            {
                switch (bunkLevel)
                {
                    case 1:
                        {
                            drawPos += new Vector3(0, 0, 0.0f);
                            break;
                        }
                    case 2:
                        {
                            drawPos += new Vector3(0, 0, 0.9f);
                            break;
                        }
                }
            }
            else if (parent.Rotation == Rot4.North)
            {
                switch (bunkLevel)
                {
                    case 1:
                        {
                            drawPos += new Vector3(0, 0, 0.5f);
                            break;
                        }
                    case 2:
                        {
                            drawPos += new Vector3(0, 0, 1.25f);
                            break;
                        }
                }
            }
            else
            {
                switch (bunkLevel)
                {
                    case 1:
                        {
                            drawPos += new Vector3(0, 0, 0.68f);
                            break;
                        }
                    case 2:
                        {
                            drawPos += new Vector3(0, 0, 1.4f);
                            break;
                        }
                }
            }
            return drawPos;
        }

        public override void DrawGUIOverlay()
        {
            base.DrawGUIOverlay();
            var bed = this.parent as Building_Bed;
            if (bed.Medical || Find.CameraDriver.CurrentZoom != 0 || !bed.PlayerCanSeeOwners)
            {
                return;
            }
            Color defaultThingLabelColor = GenMapUI.DefaultThingLabelColor;
            if (!bed.OwnersForReading.Any() && (Building_Bed_DrawGUIOverlay_Patch.guestBedType is null
                || Building_Bed_DrawGUIOverlay_Patch.guestBedType.IsAssignableFrom(this.parent.def.thingClass) is false))
            {
                GenMapUI.DrawThingLabel(bed, "Unowned".Translate(), defaultThingLabelColor);
                return;
            }
            if (bed.OwnersForReading.Count == 1)
            {
                Pawn pawn = bed.OwnersForReading[0];
                if ((!pawn.InBed() || pawn.CurrentBed() != bed) && (!pawn.RaceProps.Animal || Prefs.AnimalNameMode.ShouldDisplayAnimalName(pawn)))
                {
                    GenMapUI.DrawThingLabel(this.parent, pawn.LabelShort, defaultThingLabelColor);
                }
                return;
            }
            for (int i = 0; i < bed.OwnersForReading.Count; i++)
            {
                Pawn pawn2 = bed.OwnersForReading[i];
                GenMapUI.DrawThingLabel(GetMultiOwnersLabelScreenPosFor(i), pawn2.LabelShort, defaultThingLabelColor);
            }
        }

        private Vector3 GetMultiOwnersLabelScreenPosFor(int slotIndex)
        {
            Vector3 drawPos = this.parent.DrawPos;
            var result = this.GetDrawOffsetForLabels(slotIndex, drawPos).MapToUIPosition();
            return result;
        }

        public Vector3 GetDrawOffsetForLabels(int bunkLevel, Vector3 drawPos)
        {
            if (parent.Rotation.IsHorizontal is false)
            {
                switch (bunkLevel)
                {
                    case 0:
                        {
                            drawPos += new Vector3(0, 0, -0.7f);
                            break;
                        }
                    case 1:
                        {
                            drawPos += new Vector3(0, 0, -0.1f);
                            break;
                        }
                    case 2:
                        {
                            drawPos += new Vector3(0, 0, 0.55f);
                            break;
                        }
                }
            }
            else
            {
                switch (bunkLevel)
                {
                    case 0:
                        {
                            drawPos += new Vector3(0, 0, -0.2f);
                            break;
                        }
                    case 1:
                        {
                            drawPos += new Vector3(0, 0, 0.5f);
                            break;
                        }
                    case 2:
                        {
                            drawPos += new Vector3(0, 0, 1.2f);
                            break;
                        }
                }
            }
            return drawPos;
        }
    }
}
