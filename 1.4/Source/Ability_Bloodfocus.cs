using RimWorld;
using Verse;
using Ability = VFECore.Abilities.Ability;

namespace VPEHemosage
{
    public class Ability_Bloodfocus : Ability
    {
        public override bool IsEnabledForPawn(out string reason)
        {
            var hemogen = pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
            if (hemogen.Resource.ValuePercent <= 0.1f)
            {
                reason = "VPEH.HemogenTooLow".Translate();
                return false;
            }
            return base.IsEnabledForPawn(out reason);
        }
    }
}
