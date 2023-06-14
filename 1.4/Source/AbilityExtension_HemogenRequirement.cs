using RimWorld;
using Verse;
using VFECore.Abilities;
using Ability = VFECore.Abilities.Ability;

namespace VPEHemosage
{
    public class AbilityExtension_HemogenRequirement : AbilityExtension_AbilityMod
    {
        public float hemogenRequirement;
        public override bool IsEnabledForPawn(Ability ability, out string reason)
        {
            var hemogen = ability.pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
            if (hemogen.Resource.ValuePercent <= hemogenRequirement)
            {
                reason = "VPEH.HemogenTooLow".Translate((int)(hemogenRequirement * 100));
                return false;
            }
            return base.IsEnabledForPawn(ability, out reason);
        }
    }
}
