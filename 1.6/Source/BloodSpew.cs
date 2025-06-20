using System.Collections.Generic;
using RimWorld;
using Verse;
using VEF.Weapons;
using Ability = VEF.Abilities.Ability;

namespace VPEHemosage
{
    public class BloodSpew : ExpandableProjectile
    {
        public Ability ability;

        public HashSet<IntVec3> bloodCells = new HashSet<IntVec3>();
        protected override void Tick()
        {
            base.Tick(); 
            if (Map != null && Position.IsValid)
            {
                if (Find.TickManager.TicksGame % 4 == 0)
                {
                    var projectileLine = MakeProjectileLine(StartingPosition, DrawPos, this.Map);
                    foreach (var pos in projectileLine)
                    {
                        if (!bloodCells.Contains(pos))
                        {
                            FilthMaker.TryMakeFilth(pos, Map, ThingDefOf.Filth_Blood);
                            bloodCells.Add(pos);
                        }
                    }
                }
            }
        }
        public override void DoDamage(IntVec3 pos)
        {
            base.DoDamage(pos);
            if (pos != this.launcher.Position && this.launcher.Map != null && GenGrid.InBounds(pos, this.launcher.Map))
            {
                foreach (var cell in GenRadial.RadialCellsAround(pos, 1.5f, true))
                {
                    if (cell.InBounds(this.launcher.Map))
                    {
                        if (cell.DistanceTo(this.launcher.Position) <= ability.def.range + 1)
                        {
                            var list = this.launcher.Map.thingGrid.ThingsListAt(cell);
                            for (int num = list.Count - 1; num >= 0; num--)
                            {
                                if (IsDamagable(list[num]))
                                {
                                    if (list[num] is Pawn pawn && pawn.IsHemogenic() is false && pawn.RaceProps.Humanlike)
                                    {
                                        pawn.health.AddHediff(VPEH_DefOf.VPEH_Bloodmist);
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        public override bool IsDamagable(Thing t)
        {
            return t is Pawn && t != this && base.IsDamagable(t);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref ability, "ability");
            Scribe_Collections.Look(ref bloodCells, "bloodCells", LookMode.Value);
        }
    }
}
