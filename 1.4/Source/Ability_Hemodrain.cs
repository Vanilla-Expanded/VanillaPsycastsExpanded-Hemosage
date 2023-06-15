using RimWorld;
using RimWorld.Planet;
using VanillaPsycastsExpanded;
using Verse;
using Ability = VFECore.Abilities.Ability;

namespace VPEHemosage
{
    public class Ability_Hemodrain : Ability
    {
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            if (target.Thing is Pawn pawn && pawn.genes.HasGene(GeneDefOf.Hemogenic))
            {
                if (showMessages)
                {
                    Messages.Message("VPEH.OnlyNonHemogenic".Translate(), pawn, MessageTypeDefOf.CautionInput);
                }
                return false;
            }
            return base.ValidateTarget(target, showMessages);
        }
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            var pawnTarget = targets[0].Thing as Pawn;
            var mote = (MoteBetween)ThingMaker.MakeThing(VPEH_DefOf.VPEH_BloodmistMote) as Bloodmist;

            mote.ability = this;
            mote.Attach(pawnTarget, this.pawn);
            mote.exactPosition = pawnTarget.DrawPos;
            GenSpawn.Spawn(mote, pawnTarget.Position, this.pawn.Map);
            HealthUtility.AdjustSeverity(pawnTarget, HediffDefOf.BloodLoss, 0.6f);
        }
    }
}
