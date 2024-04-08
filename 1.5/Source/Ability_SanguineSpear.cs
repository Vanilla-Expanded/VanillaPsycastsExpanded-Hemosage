using RimWorld;
using Verse;
using VFECore.Abilities;

namespace VPEHemosage
{
    public class Ability_SanguineSpear : Ability_ShootProjectile
    {
        public override bool IsEnabledForPawn(out string reason)
        {
            if (pawn.Position.Roofed(pawn.Map))
            {
                reason = "CannotFire".Translate() + ": " + "Roofed".Translate().CapitalizeFirst();
                return false;
            }
            return base.IsEnabledForPawn(out reason);
        }

        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            if (target.Cell.Roofed(pawn.Map))
            {
                if (showMessages)
                {
                    Messages.Message("CannotFire".Translate() + ": " + "Roofed".Translate().CapitalizeFirst(), MessageTypeDefOf.CautionInput);
                }
                return false;
            }
            return base.ValidateTarget(target, showMessages);
        }
    }
}
