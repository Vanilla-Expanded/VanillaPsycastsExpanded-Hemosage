
using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore.Abilities;
using Ability = VFECore.Abilities.Ability;

namespace VPEHemosage
{
    public class Ability_WordofOffering : Ability
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
            var target = targets[0].Thing as Pawn;
            HealthUtility.AdjustSeverity(target, HediffDefOf.BloodLoss, 0.55f);
            var hemogen = pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
            hemogen.ValuePercent += 0.2f;
        }
    }
}
