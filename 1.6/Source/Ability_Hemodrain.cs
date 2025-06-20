using RimWorld;
using RimWorld.Planet;
using VanillaPsycastsExpanded;
using Verse;
using Ability = VEF.Abilities.Ability;

namespace VPEHemosage
{
    public class Ability_Hemodrain : Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            var pawnTarget = targets[0].Thing as Pawn;
            var mote = (MoteBetween)ThingMaker.MakeThing(VPEH_DefOf.VPEH_BloodmistMote) as Bloodmist;

            mote.ability = this;
            mote.Attach(pawnTarget, this.pawn);
            mote.exactPosition = pawnTarget.DrawPos;
            GenSpawn.Spawn(mote, pawnTarget.Position, this.pawn.Map);
            HealthUtility.AdjustSeverity(pawnTarget, HediffDefOf.BloodLoss, 0.55f);
        }
    }
}
