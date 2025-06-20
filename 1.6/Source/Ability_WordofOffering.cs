
using RimWorld;
using RimWorld.Planet;
using Verse;
using Ability = VEF.Abilities.Ability;

namespace VPEHemosage
{
    public class Ability_WordofOffering : Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            var target = targets[0].Thing as Pawn;
            HealthUtility.AdjustSeverity(target, HediffDefOf.BloodLoss, 0.55f);
            var hemogen = pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
            hemogen.Value += 0.2f;
        }
    }
}
