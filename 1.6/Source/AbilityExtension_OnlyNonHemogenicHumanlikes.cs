
using RimWorld;
using Verse;
using VEF.Abilities;
using Ability = VEF.Abilities.Ability;

namespace VPEHemosage;

public class AbilityExtension_OnlyNonHemogenicHumanlikes : AbilityExtension_AbilityMod
{
    public override bool ValidateTarget(LocalTargetInfo target, Ability ability, bool throwMessages = false)
    {
        if (target.Thing is Pawn pawn)
        {
            if (pawn.IsHemogenic())
            {
                if (throwMessages)
                {
                    Messages.Message("VPEH.OnlyNonHemogenic".Translate(), pawn, MessageTypeDefOf.CautionInput);
                }
                return false;
            }
            else if (pawn.RaceProps.Humanlike is false)
            {
                if (throwMessages)
                {
                    Messages.Message("VPEH.OnlyHumanlikes".Translate(), pawn, MessageTypeDefOf.CautionInput);
                }
                return false;
            }
        }
        return base.ValidateTarget(target, ability, throwMessages);
    }
}